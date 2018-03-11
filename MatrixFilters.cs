using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filters_Kudrin;
using System.ComponentModel;

namespace MatrixFilters
{


    class MatrixFilter : Filters
    {
        protected float[,] kernel = null;
        protected MatrixFilter() { }
        public MatrixFilter(float[,] kernel)
        { this.kernel = kernel; }
        public Color calculateNewPixelColorMin(Bitmap sourceImage, int x, int y)
        {
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;

            Color min = Color.FromArgb(255, 255, 255);

            for (int i = -radiusY; i <= radiusY; i++)
            {
                for (int j = -radiusX; j <= radiusX; j++)
                {
                    Color curr = sourceImage.GetPixel(Clamp(x + i, 0, sourceImage.Width - 1), Clamp(y + j, 0, sourceImage.Height - 1));
                    if ((kernel[j + radiusX, i + radiusY] != 0) && (Math.Sqrt(curr.R * curr.R + curr.G * curr.G + curr.B * curr.B) <
                                                Math.Sqrt(min.R * min.R + min.G * min.G + min.B * min.B)))
                        min = curr;
                }
            }
            return min;
        }
        public int claculatemaxchanel(int c,Bitmap source)
        {
            int max = 0;
            if (c == 0)
            {
                for (int i = 0; i < source.Width; i++)
                    for (int j = 0; j < source.Height; j++)
                        if (max < source.GetPixel(i, j).R)
                            max = source.GetPixel(i, j).R;
            }
            else
            if (c == 1)
            {
                for (int i = 0; i < source.Width; i++)
                    for (int j = 0; j < source.Height; j++)
                        if (max < source.GetPixel(i, j).R)
                            max = source.GetPixel(i, j).R;
            }

            if (c == 2)
            {
                for (int i = 0; i < source.Width; i++)
                    for (int j = 0; j < source.Height; j++)
                        if (max < source.GetPixel(i, j).R)
                            max = source.GetPixel(i, j).R;
            }
            return Clamp(max, 0, 255);
        }
        public Color calculateNewPixelColorMax(Bitmap sourceImage, int x, int y)
        {
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;

            Color max = Color.FromArgb(0, 0, 0);

            for (int i = -radiusY; i <= radiusY; i++)
            {
                for (int j = -radiusX; j <= radiusX; j++)
                {
                    Color curr = sourceImage.GetPixel(Clamp(x + i, 0, sourceImage.Width - 1), Clamp(y + j, 0, sourceImage.Height - 1));
                    if ((kernel[j + radiusX, i + radiusY] != 0) && (Math.Sqrt(curr.R * curr.R + curr.G * curr.G + curr.B * curr.B) >
                                                Math.Sqrt(max.R * max.R + max.G * max.G + max.B * max.B)))
                        max = curr;
                }
            }
            return max;
        }
        public void setkernel(float[,] arr, int size)
        {
            kernel = new float[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    kernel[i, j] = arr[i, j];
                }
        }
        protected override Color CalculateNewPixelColor(Bitmap source, int x, int y)
        {
            int RadiusX = kernel.GetLength(0) / 2;
            int RadiusY = kernel.GetLength(1) / 2;
            float ResultR = 0;
            float ResultG = 0;
            float ResultB = 0;
            for (int l = -RadiusY; l <= RadiusY; l++)
                for (int k = -RadiusX; k <= RadiusX; k++)
                {
                    int idx = Clamp(x + k, 0, source.Width - 1);
                    int idy = Clamp(y + l, 0, source.Height - 1);
                    Color neighborcolor = source.GetPixel(idx, idy);
                    ResultR += neighborcolor.R * kernel[k + RadiusX, l + RadiusY];
                    ResultG += neighborcolor.G * kernel[k + RadiusX, l + RadiusY];
                    ResultB += neighborcolor.B * kernel[k + RadiusX, l + RadiusY];
                }
            return Color.FromArgb(Clamp((int)ResultR, 0, 255), Clamp((int)ResultG, 0, 255), Clamp((int)ResultB, 0, 255));
        }
    }

    class BlurFilter : MatrixFilter
    {
        public BlurFilter()
        {
            int sizeX = 3;
            int sizeY = 3;
            kernel = new float[sizeX, sizeY];
            for (int i = 0; i < sizeX; i++)
                for (int j = 0; j < sizeY; j++)
                    kernel[i, j] = 1.0f / (float)(sizeX * sizeY);

        }
    }
    class GaussianFilter : MatrixFilter
    {
        public GaussianFilter()
        {
            createGaussianKernel(3, 2);
        }
        public void createGaussianKernel(int radius, float sigma)
        {
            int size = 2 * radius + 1;
            kernel = new float[size, size];
            float norm = 0;
            for (int i = -radius; i <= radius; i++)
                for (int j = -radius; j <= radius; j++)
                {
                    kernel[i + radius, j + radius] = (float)(Math.Exp(-(i * i + j * j) / (sigma * sigma)));
                    norm += kernel[i + radius, j + radius];
                }
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    kernel[i, j] /= norm;

        }
    }

    class SobelFilter : MatrixFilter

    {
        public SobelFilter() { }
        protected override Color CalculateNewPixelColor(Bitmap source, int x, int y)
        {
            float resultR1 = 0;
            float resultG1 = 0;
            float resultB1 = 0;
            float resultR2 = 0;
            float resultG2 = 0;
            float resultB2 = 0;
            kernel = new float[3, 3] {
             { -1,-2,-1},
             {0,0,0},
            { 1,2,1} };

            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;
            for (int l = -radiusY; l < radiusY; l++)
                for (int k = -radiusX; k < radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, source.Width - 1);
                    int idY = Clamp(y + l, 0, source.Height - 1);
                    Color neighborcolor = source.GetPixel(idX, idY);
                    resultR1 += neighborcolor.R * kernel[k + radiusX, l + radiusY];
                    resultG1 += neighborcolor.G * kernel[k + radiusX, l + radiusY];
                    resultB1 += neighborcolor.B * kernel[k + radiusX, l + radiusY];
                }
            kernel = new float[3, 3] {
                {-1,0,1},
                {-2,0,2},
                {-1,0,1}
            };
            for (int l = -radiusY; l < radiusY; l++)
                for (int k = -radiusX; k < radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, source.Width - 1);
                    int idY = Clamp(y + l, 0, source.Height - 1);
                    Color neighborcolor = source.GetPixel(idX, idY);
                    resultR2 += neighborcolor.R * kernel[k + radiusX, l + radiusY];
                    resultG2 += neighborcolor.G * kernel[k + radiusX, l + radiusY];
                    resultB2 += neighborcolor.B * kernel[k + radiusX, l + radiusY];
                }
            int sum = (int)Math.Sqrt(resultR1 * resultR1 + resultG1 * resultG1 + resultB1 * resultB1 + resultR2 * resultR2 + resultB2 * resultB2 + resultG2 * resultG2);
            int C = Clamp(sum, 0, 255);
            return Color.FromArgb(C, C, C);

        }
    }
    class Sharpness : MatrixFilter
    {
        public Sharpness()
        {
            kernel = new float[3, 3] {
                {0,-1,0},
                {-1,5,-1},
                {0,-1,0}
            };

        }
    }

    class Sharra : MatrixFilter
    {
        public Sharra() { }
        protected override Color CalculateNewPixelColor(Bitmap source, int x, int y)
        {
            float resultR1 = 0;
            float resultG1 = 0;
            float resultB1 = 0;
            float resultR2 = 0;
            float resultG2 = 0;
            float resultB2 = 0;
            kernel = new float[3, 3] {
             { 3,10,3},
             {0,0,0},
            { -3,-10,-3} };

            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;
            for (int l = -radiusY; l < radiusY; l++)
                for (int k = -radiusX; k < radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, source.Width - 1);
                    int idY = Clamp(y + l, 0, source.Height - 1);
                    Color neighborcolor = source.GetPixel(idX, idY);
                    resultR1 += neighborcolor.R * kernel[k + radiusX, l + radiusY];
                    resultG1 += neighborcolor.G * kernel[k + radiusX, l + radiusY];
                    resultB1 += neighborcolor.B * kernel[k + radiusX, l + radiusY];
                }
            kernel = new float[3, 3] {
                {3,0,-3},
                {10,0,10},
                {3,0,-3}
            };
            for (int l = -radiusY; l < radiusY; l++)
                for (int k = -radiusX; k < radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, source.Width - 1);
                    int idY = Clamp(y + l, 0, source.Height - 1);
                    Color neighborcolor = source.GetPixel(idX, idY);
                    resultR2 += neighborcolor.R * kernel[k + radiusX, l + radiusY];
                    resultG2 += neighborcolor.G * kernel[k + radiusX, l + radiusY];
                    resultB2 += neighborcolor.B * kernel[k + radiusX, l + radiusY];
                }
            int sum = (int)Math.Sqrt(resultR1 * resultR1 + resultG1 * resultG1 + resultB1 * resultB1 + resultR2 * resultR2 + resultB2 * resultB2 + resultG2 * resultG2);
            int C = Clamp(sum, 0, 255);
            return Color.FromArgb(C, C, C);


        }
    }

    class Pruit : MatrixFilter
    {

        public Pruit() { }
        protected override Color CalculateNewPixelColor(Bitmap source, int x, int y)
        {
            float resultR1 = 0;
            float resultG1 = 0;
            float resultB1 = 0;
            float resultR2 = 0;
            float resultG2 = 0;
            float resultB2 = 0;
            kernel = new float[3, 3] {
             {-1,-1,-1},
             {0,0,0},
             {1,1,1}
            };

            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;
            for (int l = -radiusY; l < radiusY; l++)
                for (int k = -radiusX; k < radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, source.Width - 1);
                    int idY = Clamp(y + l, 0, source.Height - 1);
                    Color neighborcolor = source.GetPixel(idX, idY);
                    resultR1 += neighborcolor.R * kernel[k + radiusX, l + radiusY];
                    resultG1 += neighborcolor.G * kernel[k + radiusX, l + radiusY];
                    resultB1 += neighborcolor.B * kernel[k + radiusX, l + radiusY];
                }
            kernel = new float[3, 3] {
             {-1,0,1},
             {-1,0,1},
             {-1,0,1}
            };
            for (int l = -radiusY; l < radiusY; l++)
                for (int k = -radiusX; k < radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, source.Width - 1);
                    int idY = Clamp(y + l, 0, source.Height - 1);
                    Color neighborcolor = source.GetPixel(idX, idY);
                    resultR2 += neighborcolor.R * kernel[k + radiusX, l + radiusY];
                    resultG2 += neighborcolor.G * kernel[k + radiusX, l + radiusY];
                    resultB2 += neighborcolor.B * kernel[k + radiusX, l + radiusY];
                }
            int sum = (int)Math.Sqrt(resultR1 * resultR1 + resultG1 * resultG1 + resultB1 * resultB1 + resultR2 * resultR2 + resultB2 * resultB2 + resultG2 * resultG2);
            int C = Clamp(sum, 0, 255);
            return Color.FromArgb(C, C, C);


        }
    }
    class Embosing : MatrixFilter
    {
        public Embosing()
        {
            kernel = new float[3, 3]
            {
                {0,1,0 },
                {1,0,-1 },
                {0,-1,0 }
            };
        }
        protected override Color CalculateNewPixelColor(Bitmap source, int x, int y)
        {
            float R = 0;
            float G = 0;
            float B = 0;
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;
            for (int l = -radiusY; l < radiusY; l++)
                for (int k = -radiusX; k < radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, source.Width);
                    int idY = Clamp(y + l, 0, source.Height);
                    Color neighborColor = source.GetPixel(idX, idY);
                    R += neighborColor.R * kernel[k + radiusX, l + radiusY];
                    G += neighborColor.G * kernel[k + radiusX, l + radiusY];
                    B += neighborColor.B * kernel[k + radiusX, l + radiusY];
                }
            int R1 = Clamp((int)R, 0, 255);
            int G1 = Clamp((int)G, 0, 255);
            int B1 = Clamp((int)B, 0, 255);
            return Color.FromArgb(R1, G1, B1);
        }
    }
    class Dillation : MatrixFilter
    {
        protected override Color CalculateNewPixelColor(Bitmap source, int x, int y)
        {
            int RadiusX = kernel.GetLength(0) / 2;
            int RadiusY = kernel.GetLength(1) / 2;
            Color max = Color.FromArgb(0, 0, 0);
            for (int i = -RadiusX; i < RadiusX; i++)
                for (int j = -RadiusY; j < RadiusY; j++)
                {
                    Color curr = source.GetPixel(Clamp((x + i), 0, source.Width - 1), Clamp((y + j), 0, source.Height - 1));
                    if (kernel[i + RadiusX, j + RadiusY] != 0 && (Math.Sqrt(curr.R * curr.R + curr.B * curr.B + curr.G * curr.B) > Math.Sqrt(max.R * max.R + max.B * max.B + max.G * max.B)))
                        max = curr;
                }
            return max;
        }
    }
    class Erosion : MatrixFilter
    {
        protected override Color CalculateNewPixelColor(Bitmap source, int x, int y)
        {
            int RadiusX = kernel.GetLength(0) / 2;
            int RadiusY = kernel.GetLength(1) / 2;
            Color min = Color.FromArgb(255, 255, 255);
            for (int i = -RadiusX; i < RadiusX; i++)
                for (int j = -RadiusY; j < RadiusY; j++)
                {
                    Color curr = source.GetPixel(Clamp((x + i), 0, source.Width - 1), Clamp((y + j), 0, source.Height - 1));
                    if (kernel[i + RadiusX, j + RadiusY] != 0 && Math.Sqrt(curr.R * curr.R + curr.B * curr.B + curr.G * curr.B) < Math.Sqrt(min.R * min.R + min.B * min.B + min.G * min.B))
                        min = curr;
                }
            return min;
        }
    }
    class Opening : MatrixFilter
    {
        public override Bitmap processImage(Bitmap im, BackgroundWorker worker)
        {

            Bitmap resultImage = new Bitmap(im.Width, im.Height);

            for (int i = 0; i < im.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 50));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < im.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColorMin(im, i, j));
                }
            }

            Bitmap resultImage1 = new Bitmap(im.Width, im.Height);

            for (int i = 0; i < im.Width; i++)
            {
                worker.ReportProgress((int)(50 + (float)i / resultImage.Width * 50));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < im.Height; j++)
                {
                    resultImage1.SetPixel(i, j, calculateNewPixelColorMax(resultImage, i, j));
                }
            }

            return resultImage1;

        }
    }
    class Closing : MatrixFilter
    {
        public override Bitmap processImage(Bitmap im, BackgroundWorker worker)
        {

            Bitmap resultImage = new Bitmap(im.Width, im.Height);

            Bitmap resultImage1 = new Bitmap(im.Width, im.Height);

            for (int i = 0; i < im.Width; i++)
            {
                worker.ReportProgress((int)(50 + (float)i / resultImage.Width * 50));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < im.Height; j++)
                {
                    resultImage1.SetPixel(i, j, calculateNewPixelColorMax(im, i, j));
                }
            }
            for (int i = 0; i < im.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 50));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < im.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColorMin(resultImage1, i, j));
                }
            }
            return resultImage;
        }
    }
    class GradFilter : MatrixFilter
    {

        public override Bitmap processImage(Bitmap im, BackgroundWorker worker)
        {

            Bitmap resultImage = new Bitmap(im.Width, im.Height);

            for (int i = 0; i < im.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 33));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < im.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColorMax(im, i, j));
                }
            }

            Bitmap resultImage1 = new Bitmap(im.Width, im.Height);

            for (int i = 0; i < im.Width; i++)
            {
                worker.ReportProgress((int)(33 + (float)i / resultImage1.Width * 33));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < im.Height; j++)
                {
                    resultImage1.SetPixel(i, j, calculateNewPixelColorMin(im, i, j));
                }
            }

            for (int i = 0; i < im.Width; i++)
            {
                worker.ReportProgress((int)(66 + (float)i / resultImage1.Width * 34));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < im.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColorMin(resultImage1, i, j));
                }
            }


            return resultImage;

        }
    }
    class LinearStretching : MatrixFilter
    {
        private int maxR, minR, maxG, minG, maxB, minB;
        public LinearStretching()
        {
            maxR = new int();
            minR = new int();
            maxG = new int();
            minG = new int();
            maxB = new int();
            minB = new int();
        }

        public override Bitmap processImage(Bitmap im, BackgroundWorker worker)
        {
            maxR = minR = im.GetPixel(0, 0).R;
            maxG = minG = im.GetPixel(0, 0).G;
            maxB = minB = im.GetPixel(0, 0).B;

            for (int i = 0; i < im.Width; i++)
                for (int j = 0; j < im.Height; j++)
                {
                    if (im.GetPixel(i, j).R > maxR)
                        maxR = im.GetPixel(i, j).R;
                    if (im.GetPixel(i, j).G > maxG)
                        maxG = im.GetPixel(i, j).G;
                    if (im.GetPixel(i, j).B > maxB)
                        maxB = im.GetPixel(i, j).B;
                    if (im.GetPixel(i, j).R < minR)
                        minR = im.GetPixel(i, j).R;
                    if (im.GetPixel(i, j).G < minG)
                        minG = im.GetPixel(i, j).G;
                    if (im.GetPixel(i, j).B < minB)
                        minB = im.GetPixel(i, j).B;
                }

            Bitmap resultImage = new Bitmap(im.Width, im.Height);

            for (int i = 0; i < im.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < im.Height; j++)
                {
                    resultImage.SetPixel(i, j,
                        Color.FromArgb(Clamp((int)(255 * (im.GetPixel(i, j).R - minR) / (maxR - minR)), 0, 255),
                                       Clamp((int)(255 * (im.GetPixel(i, j).G - minG) / (maxG - minG)), 0, 255),
                                       Clamp((int)(255 * (im.GetPixel(i, j).B - minB) / (maxB - minB)), 0, 255)));
                }
            }
            return resultImage;
        }
    }


}