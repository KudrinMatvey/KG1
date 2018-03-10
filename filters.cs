using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace Filters_Kudrin
{
    public abstract class Filters
    {
        protected abstract Color CalculateNewPixelColor(Bitmap source, int x, int y);

        public int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
        public Bitmap processImage(Bitmap source, BackgroundWorker worker)
        {



            Bitmap result = new Bitmap(source.Width, source.Height);
            for (int i = 0; i < source.Width; i++)
            {
                worker.ReportProgress((int)((float)i / result.Width * 100));
                if (worker.CancellationPending) return null;
                for (int j = 0; j < source.Height; j++)
                {

                    result.SetPixel(i, j, CalculateNewPixelColor(source, i, j));
                }
            }
            return result;
        }

    }
    public class InvertFilter : Filters
    {
        protected override Color CalculateNewPixelColor(Bitmap source, int x, int y)
        {
            Color SourceColor = source.GetPixel(x, y);
            Color ResultColor = Color.FromArgb(255 - SourceColor.R, 255 - SourceColor.G, 255 - SourceColor.B);
            return ResultColor;
        }
    }
    class GrayScaleFilter : Filters
    {
        protected override Color CalculateNewPixelColor(Bitmap source, int x, int y)
        {
            Color SourceColor = source.GetPixel(x, y);
            int Intensity = Convert.ToInt32(0.36 * SourceColor.R + 0.53 * SourceColor.G + 0.11 * SourceColor.B);
            Intensity = Clamp(Intensity, 0, 255);
            return Color.FromArgb(Intensity, Intensity, Intensity);
        }
    }
    class SepiaFilter : Filters
    {
        protected override Color CalculateNewPixelColor(Bitmap source, int x, int y)
        {
            int k = 20;
            Color SourceColor = source.GetPixel(x, y);
            int Intensity = Convert.ToInt32(0.36 * SourceColor.R + 0.53 * SourceColor.G + 0.11 * SourceColor.B);
            int R = Clamp(Intensity + 2 * k, 0, 255);
            int G = Clamp(Convert.ToInt32(Intensity + 0.5 * k), 0, 255);
            int B = Clamp(Intensity - k, 0, 255);
            return Color.FromArgb(R, G, B);
        }
    }
    class AddBrightness : Filters
    {
        protected override Color CalculateNewPixelColor(Bitmap source, int x, int y)
        {
            int k = 20;
            Color SourceColor = source.GetPixel(x, y);
            int R = Clamp(SourceColor.R + k, 0, 255);
            int G = Clamp(SourceColor.G, 0, 255);
            int B = Clamp(SourceColor.B, 0, 255);
            return Color.FromArgb(R, G, B);
        }
    }
    class Transfer : Filters
    {
        protected override Color CalculateNewPixelColor(Bitmap source, int x, int y)
        {
            int newX = Clamp((int)(x + 50), 0, source.Width - 1);
            int newY = y;
            return source.GetPixel(newX, newY);
        }
    }
    class Turn : Filters
    {
        protected override Color CalculateNewPixelColor(Bitmap source, int x, int y)
        {
            int x0, y0, k;
            x0 = 150;
            y0 = 150;
            k = 90;
            int newX = Clamp(((int)((x - x0) * Math.Cos(k) - (y - y0) * Math.Sin(k) + x0)), 0, source.Width - 1);
            int newY = Clamp(((int)((x - x0) * Math.Sin(k) - (y - y0) * Math.Cos(k)) + y0), 0, source.Height - 1);
            return source.GetPixel(newX, newY);
        }
    }
    class Glass : Filters
    {
        private Random rnd;
        public Glass()
        {
            rnd = new Random();
        }
        protected override Color CalculateNewPixelColor(Bitmap source, int x, int y)
        {

            int newX = Clamp(((int)(x + (rnd.NextDouble() - 0.5f) * 10)), 0, source.Width - 1);
            int newY = Clamp(((int)(y + (rnd.NextDouble() - 0.5f) * 10)), 0, source.Height - 1);
            return source.GetPixel(newX, newY);
        }
    }
}

    