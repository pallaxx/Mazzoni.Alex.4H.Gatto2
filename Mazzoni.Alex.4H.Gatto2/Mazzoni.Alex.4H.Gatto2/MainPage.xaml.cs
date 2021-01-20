using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Mazzoni.Alex._4H.Gatto2
{
    public partial class MainPage : ContentPage
    {
        SKPath gatto = new SKPath();
        List<SKPoint> disegno = new List<SKPoint>();
        public MainPage()
        {
            InitializeComponent();
        }
        private void SKCanvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;

            // Preleva le Tela e la cancello
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            //// Disegno
            SKPath pathDisegno = new SKPath();

            SKPoint p = new SKPoint(0, 0);
            pathDisegno.MoveTo(p);

            foreach (SKPoint punto in disegno)
                pathDisegno.LineTo(punto);

            SKPaint paint1 = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.White,
                StrokeWidth = 3
            };

            SKPaint paint2 = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Red,
                StrokeWidth = 3
            };

            canvas.DrawPath(gatto, paint1);
            canvas.DrawPath(pathDisegno, paint2);
            //canvas.DrawPath(path, paint2);
        }

        private async void btnCaricaGatto_Clicked(object sender, EventArgs e)
        {
            var cacheDir = FileSystem.CacheDirectory;
            var mainDir = FileSystem.AppDataDirectory;

            using (var stream = await FileSystem.OpenAppPackageFileAsync("gatto.csv"))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        string str = reader.ReadLine();
                        string[] colonne = str.Split(';');
                        float X, Y; 
                        float.TryParse(colonne[1], out X);
                        float.TryParse(colonne[2], out Y);

                        SKPoint p = new SKPoint(X, Y);

                        if (colonne[0] == "L")
                            gatto.LineTo(p);

                        else if(colonne[0] =="M")
                            gatto.MoveTo(p);

                    }
                    Disegno.InvalidateSurface();
                }
            }
        }

        private async void btnCaricaQuadrato_Clicked(object sender, EventArgs e)
        {
            var cacheDir = FileSystem.CacheDirectory;
            var mainDir = FileSystem.AppDataDirectory;

            using (var stream = await FileSystem.OpenAppPackageFileAsync("quadrato.dis"))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        string str = reader.ReadLine();
                        string[] colonne = str.Split(';');
                        float X, Y;
                        float.TryParse(colonne[0], out X);
                        float.TryParse(colonne[1], out Y);

                        SKPoint p = new SKPoint(X, Y);

                        disegno.Add(p);
                    }
                    Disegno.InvalidateSurface();
                }
            }
        }
    }
}
