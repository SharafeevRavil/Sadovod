using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gestures;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SkiaSharp;
using SkiaSharp.Views.Android;

namespace SadovodMobile
{

    [Activity(Label = "Activity1")]
    public class Activity1 : Activity, IDrawableActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.BedsLayout);

            gestureRecognizerView = new GestureRecognizerView(this);
            FindViewById<FrameLayout>(Resource.Id.frameLayout1).AddView(gestureRecognizerView);

            canvasView = FindViewById<SKCanvasView>(Resource.Id.sKCanvasView1);
            canvasView.PaintSurface += OnPainting;

            paths = new List<SKPath>();
            screenSize = new Point();
            WindowManager.DefaultDisplay.GetSize(screenSize);
            bitmap = new SKBitmap(screenSize.X, screenSize.Y);
            shapesCanvas = new SKCanvas(bitmap);
            scaleCanvas = new SKCanvas(bitmap);
        }
        private SKCanvasView canvasView;
        private SKBitmap bitmap;
        private Point screenSize;

        private SKCanvas shapesCanvas;
        private SKCanvas scaleCanvas;

        private GestureRecognizerView gestureRecognizerView;

        private List<SKPath> paths;

        public void Touch(float inputX, float inputY)
        {

            var matrix = shapesCanvas.TotalMatrix;
            SKMatrix inverse;
            matrix.TryInvert(out inverse);
            float x = inverse.ScaleX * inputX + inverse.SkewX * inputY + inverse.TransX;
            float y = inverse.SkewY * inputX + inverse.ScaleY * inputY + inverse.TransY;

            x -= 60;
            y -= 60;

            var path = new SKPath { FillType = SKPathFillType.EvenOdd };
            path.MoveTo(60 + x, 0 + y);
            path.LineTo(80 + x, 40 + y);
            path.LineTo(120 + x, 60 + y);
            path.LineTo(80 + x, 80 + y);
            path.LineTo(60 + x, 120 + y);
            path.LineTo(40 + x, 80 + y);
            path.LineTo(0 + x, 60 + y);
            path.LineTo(40 + x, 40 + y);
            path.LineTo(60 + x, 0 + y);
            path.Close();

            paths.Add(path);
            /***/
            Random r = new Random();
            var pathStroke = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.StrokeAndFill,
                Color = new SKColor((byte)r.Next(256), (byte)r.Next(256), (byte)r.Next(256)),
                StrokeWidth = 5
            };
            shapesCanvas.DrawPath(path, pathStroke);
            Redraw();
        }

        private void Redraw()
        {
            bitmap.Erase(SKColors.White);
            Random r = new Random();
            shapesCanvas.Clear(SKColors.White);
            foreach (var path in paths)
            {
                var pathStroke = new SKPaint
                {
                    IsAntialias = true,
                    Style = SKPaintStyle.StrokeAndFill,
                    Color = new SKColor((byte)r.Next(256), (byte)r.Next(256), (byte)r.Next(256)),
                    StrokeWidth = 5
                };
                shapesCanvas.DrawPath(path, pathStroke);
            }

            RedrawScale();

            canvasView.Invalidate();
        }

        private void RedrawScale()
        {
            SKPoint leftPoint = new SKPoint(20, 20);
            SKPoint rightPoint = leftPoint + new SKPoint(100, 0);
            SKPoint borderOffset = new SKPoint(0, 5);
            SKPoint textOffset = new SKPoint(0, 20);

            //scaleCanvas.Clear();

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

        private void OnPainting(object sender, SKPaintSurfaceEventArgs e)
        {
            e.Surface.Canvas.DrawBitmap(bitmap, 0, 0);
        }
    }
}