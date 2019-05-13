using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using SadovodMobile.Activities;
using SkiaSharp;
using SkiaSharp.Views.Android;

namespace SadovodMobile
{
    [Activity(Label = "DrawBedsActivity")]
    public class DrawBedsActivity : AppCompatActivity, IDrawableActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DrawSteadLayout);

            gestureRecognizerView = new GestureRecognizerView(this);
            FindViewById<FrameLayout>(Resource.Id.frameLayout1).AddView(gestureRecognizerView, 0);

            canvasView = FindViewById<SKCanvasView>(Resource.Id.sKCanvasView1);
            canvasView.PaintSurface += OnPainting;

            screenSize = new Point();
            WindowManager.DefaultDisplay.GetSize(screenSize);
            bitmap = new SKBitmap(screenSize.X, screenSize.Y);

            steadPoints = new List<SKPoint>();
            foreach (var a in UserSingleton.Instance.CurrentStead.Points)
            {
                steadPoints.Add(new SKPoint(a.X, a.Y));
            }
            allBeds = new List<List<SKPoint>>();
            foreach (var bed in UserSingleton.Instance.CurrentStead.GardenBeds)
            {
                var ans = new List<SKPoint>();
                foreach (var a in bed.Points)
                {
                    ans.Add(new SKPoint(a.X, a.Y));
                }
                allBeds.Add(ans);
            }

            shapesCanvas = new SKCanvas(bitmap);
            scaleCanvas = new SKCanvas(bitmap);

            Redraw();
        }
        private SKCanvasView canvasView;
        private SKBitmap bitmap;
        private Point screenSize;

        private SKCanvas shapesCanvas;
        private SKCanvas scaleCanvas;

        private GestureRecognizerView gestureRecognizerView;

        private void OnPainting(object sender, SKPaintSurfaceEventArgs e)
        {
            e.Surface.Canvas.DrawBitmap(bitmap, 0, 0);
        }

        private List<SKPoint> steadPoints;
        private List<List<SKPoint>> allBeds;

        public void OnPlus(object sender, EventArgs eventArgs)
        {
            //включаю добавление грядок
            StartActivity(new Intent(this, typeof(AddBedActivity)));
        }

        public void Touch(float inputX, float inputY)
        {
            var point = Utilities.ToRealCoords(inputX, inputY, shapesCanvas.TotalMatrix);
            //Проверяем грядки
            for (int i = 0; i < allBeds.Count; i++)
            {
                if (Utilities.CheckInside(point, allBeds[i]))
                {
                    Log.Debug("SUKAAAAAAAA", i.ToString());
                }
            }
        }

        private float lastScale = 1;
        public void Scale(float inputX, float inputY, float scale)
        {
            var point = Utilities.ToRealCoords(inputX, inputY, shapesCanvas.TotalMatrix);
            shapesCanvas.Scale(scale / lastScale, scale / lastScale, point.X, point.Y);
            lastScale = scale;
            Redraw();
        }

        public void Swipe(float distX, float distY)
        {
            var matrix = shapesCanvas.TotalMatrix;
            float x = distX / matrix.ScaleX;
            float y = distY / matrix.ScaleY;
            shapesCanvas.Translate(-x, -y);
            Redraw();
        }

        private void Redraw()
        {
            bitmap.Erase(SKColors.White);
            shapesCanvas.Clear(SKColors.White);

            var pathStroke = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.StrokeAndFill,
                Color = new SKColor(239, 180, 56),
                StrokeWidth = 5
            };

            /*ПЕРЕРИСОВКА УЧАСТКА*/
            var path = new SKPath { FillType = SKPathFillType.EvenOdd };
            if (steadPoints.Count > 0)
            {
                path.MoveTo(steadPoints[0]);
                for (int i = 1; i < steadPoints.Count; i++)
                {
                    path.LineTo(steadPoints[i]);
                }
                path.LineTo(steadPoints[0]);
                shapesCanvas.DrawPath(path, pathStroke);
            }
            /**/
            /*ПЕРЕРИСОВКА ГРЯДОК*/
            foreach (var a in allBeds)
            {
                path = new SKPath { FillType = SKPathFillType.EvenOdd };
                path.MoveTo(a[0]);
                for (int i = 1; i < a.Count; i++)
                {
                    path.LineTo(a[i]);
                }
                path.LineTo(a[0]);
                shapesCanvas.DrawPath(path, pathStroke);
            }
            /**/

            RedrawScale();

            canvasView.Invalidate();
        }

        private void RedrawScale()
        {
            SKPoint leftPoint = new SKPoint(20, 20);
            SKPoint rightPoint = leftPoint + new SKPoint(100, 0);
            SKPoint borderOffset = new SKPoint(0, 5);
            SKPoint textOffset = new SKPoint(0, 20);

            var pathStroke = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Black,
                StrokeWidth = 5
            };

            var path = new SKPath();
            path.MoveTo(leftPoint);
            path.LineTo(rightPoint);
            scaleCanvas.DrawPath(path, pathStroke);

            var path1 = new SKPath();
            path1.MoveTo(leftPoint - borderOffset);
            path1.LineTo(leftPoint + borderOffset);
            scaleCanvas.DrawPath(path1, pathStroke);

            var path2 = new SKPath();
            path2.MoveTo(rightPoint - borderOffset);
            path2.LineTo(rightPoint + borderOffset);
            scaleCanvas.DrawPath(path2, pathStroke);

            var textStroke = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Black,
                StrokeWidth = 1
            };

            var delta = Utilities.ToRealCoords(rightPoint, shapesCanvas.TotalMatrix) - Utilities.ToRealCoords(leftPoint, shapesCanvas.TotalMatrix);
            string text = delta.X.ToString("N1") + " m";

            scaleCanvas.DrawText(text, leftPoint + textOffset, textStroke);
        }
    }
}