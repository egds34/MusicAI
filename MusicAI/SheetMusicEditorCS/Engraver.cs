//Contains all the functions for drawing or "engraving" a bitmap.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SheetMusicEditorCS
{
    internal class Engraver
    {
        private ScoreRuler paper;
        private RenderTargetBitmap bitmap;

        public Engraver(ScoreRuler paper)
        {
            this.paper = paper;
            GenerateBitmap();
        }

        ~Engraver()
        {
        }

        public void GenerateBitmap(double DPI = 96)
        {
            int width = (int)paper.paperWidth;
            int height = (int)paper.paperHeight;

            PixelFormat pf = PixelFormats.Pbgra32;

            bitmap = new RenderTargetBitmap(width, height, DPI, DPI, pf);
        }

        public void SetPaper(ScoreRuler paper)
        {
            this.paper = paper;
            GenerateBitmap();
        }

        public RenderTargetBitmap GetScoreBitmap()
        {
            //testing

            bitmap.Clear();

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawRectangle(Brushes.White, new Pen(Brushes.Black, 4), new Rect(new Point(0, 0), new Size((int)paper.paperWidth, (int)paper.paperHeight)));
            drawingContext.DrawLine(new Pen(Brushes.Black, 4), new Point(2, 2), new Point(100, 100));
            drawingContext.Close();

            bitmap.Render(drawingVisual);
            //testing

            return bitmap;
        }

        public Size GetPaperSize()
        {
            Size result = new Size(0.0, 0.0);
            result.Width = paper.paperWidth;
            result.Height = paper.paperHeight;
            return result;
        }
    }
}