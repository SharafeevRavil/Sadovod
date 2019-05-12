using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SadovodMobile.Activities;

namespace SadovodMobile
{
    public class GestureRecognizerView : View
    {
        private static readonly int InvalidPointerId = -1;

        private readonly ScaleGestureDetector _scaleDetector;
        private readonly GestureDetector _swipeDetector;

        private int _activePointerId = InvalidPointerId;
        public float _lastTouchX;
        public float _lastTouchY;
        public float _posX;
        public float _posY;
        public float _scaleFactor = 1.0f;

        private IDrawableActivity activiy;

        public override bool OnTouchEvent(MotionEvent ev)
        {
            _scaleDetector.OnTouchEvent(ev);
            _swipeDetector.OnTouchEvent(ev);

            MotionEventActions action = ev.Action & MotionEventActions.Mask;
            int pointerIndex;

            switch (action)
            {
                case MotionEventActions.Down:
                    _lastTouchX = ev.GetX();
                    _lastTouchY = ev.GetY();
                    _activePointerId = ev.GetPointerId(0);
                    activiy.Touch(_lastTouchX, _lastTouchY);
                    break;

                case MotionEventActions.Move:
                    pointerIndex = ev.FindPointerIndex(_activePointerId);
                    float x = ev.GetX(pointerIndex);
                    float y = ev.GetY(pointerIndex);
                    if (!_scaleDetector.IsInProgress)
                    {
                        // Only move the ScaleGestureDetector isn't already processing a gesture.
                        float deltaX = x - _lastTouchX;
                        float deltaY = y - _lastTouchY;
                        _posX += deltaX;
                        _posY += deltaY;
                        Invalidate();
                    }

                    _lastTouchX = x;
                    _lastTouchY = y;
                    break;

                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    // We no longer need to keep track of the active pointer.
                    _activePointerId = InvalidPointerId;
                    break;

                case MotionEventActions.PointerUp:
                    // check to make sure that the pointer that went up is for the gesture we're tracking.
                    pointerIndex = (int)(ev.Action & MotionEventActions.PointerIndexMask) >> (int)MotionEventActions.PointerIndexShift;
                    int pointerId = ev.GetPointerId(pointerIndex);
                    if (pointerId == _activePointerId)
                    {
                        // This was our active pointer going up. Choose a new
                        // action pointer and adjust accordingly
                        int newPointerIndex = pointerIndex == 0 ? 1 : 0;
                        _lastTouchX = ev.GetX(newPointerIndex);
                        _lastTouchY = ev.GetY(newPointerIndex);
                        _activePointerId = ev.GetPointerId(newPointerIndex);
                    }
                    break;

            }
            return true;
        }
        public GestureRecognizerView(Context context) : base(context, null, 0)
        {
            activiy = (IDrawableActivity)context;
            _scaleDetector = new ScaleGestureDetector(context, new MyScaleListener(this));
            _swipeDetector = new GestureDetector(context, new MySwipeListener(this));
        }

        public GestureRecognizerView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {/*
            _icon = context.Resources.GetDrawable(Resource.Drawable.Splash);
            _icon.SetBounds(0, 0, _icon.IntrinsicWidth, _icon.IntrinsicHeight);
            _scaleDetector = new ScaleGestureDetector(context, new MyScaleListener(this));*/
        }

        public GestureRecognizerView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {/*
            _icon = context.Resources.GetDrawable(Resource.Drawable.Splash);
            _icon.SetBounds(0, 0, _icon.IntrinsicWidth, _icon.IntrinsicHeight);
            _scaleDetector = new ScaleGestureDetector(context, new MyScaleListener(this));*/
        }

        private class MySwipeListener : GestureDetector.SimpleOnGestureListener
        {
            private readonly GestureRecognizerView _view;
            private IDrawableActivity activity;

            public MySwipeListener(GestureRecognizerView view)
            {
                _view = view;
                activity = _view.activiy;
            }

            public override bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
            {
                activity.Swipe(distanceX, distanceY);
                return true;
            }
        }

        private class MyScaleListener : ScaleGestureDetector.SimpleOnScaleGestureListener
        {
            private readonly GestureRecognizerView _view;
            private IDrawableActivity activity;

            public MyScaleListener(GestureRecognizerView view)
            {
                _view = view;
                activity = _view.activiy;
            }

            public override bool OnScale(ScaleGestureDetector detector)
            {
                _view._scaleFactor *= detector.ScaleFactor;

                // put a limit on how small or big the image can get.
                if (_view._scaleFactor > 5.0f)
                {
                    _view._scaleFactor = 5.0f;
                }
                if (_view._scaleFactor < 0.1f)
                {
                    _view._scaleFactor = 0.1f;
                }

                _view.Invalidate();
                activity.Scale(_view._lastTouchX, _view._lastTouchY, _view._scaleFactor);
                return true;
            }
        }
    }
}