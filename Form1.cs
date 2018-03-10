using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Filters_Kudrin;
using MatrixFilters;
namespace KG1
{
    public partial class Form1 : Form
    {
        Bitmap image;
        public Form1()
        {
            InitializeComponent();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files  | *.png ; *.jpg ; *.bmp | All Files (*.*) | *.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(dialog.FileName);
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }

        }

        private void инверсияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InvertFilter filter = new InvertFilter();
            Bitmap result = filter.processImage(image,backgroundWorker1);
           pictureBox1.Image = result;
           pictureBox1.Refresh();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void чернобелыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GrayScaleFilter filter = new GrayScaleFilter();
            Bitmap result = filter.processImage(image, backgroundWorker1);
            backgroundWorker1.RunWorkerAsync(filter);
        }
        private void сепияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SepiaFilter filter = new SepiaFilter();
            Bitmap result = filter.processImage(image, backgroundWorker1);
            backgroundWorker1.RunWorkerAsync(filter);
        }
        private void добавитьЯркостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddBrightness filter = new AddBrightness();
            Bitmap result = filter.processImage(image, backgroundWorker1);
            backgroundWorker1.RunWorkerAsync(filter);
        }


        private void поОсиYToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void поОсиXToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void собольToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SobelFilter filter = new SobelFilter();
            Bitmap result = filter.processImage(image, backgroundWorker1); 
            backgroundWorker1.RunWorkerAsync(filter);
        }
        private void резкостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sharpness filter = new Sharpness();
            Bitmap result = filter.processImage(image, backgroundWorker1);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Bitmap newImage = ((Filters)e.Argument).processImage(image, backgroundWorker1);
            if (backgroundWorker1.CancellationPending != true)
                image = newImage;
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {  }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        { 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void размытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new BlurFilter();
            backgroundWorker1.RunWorkerAsync(filter);
         
        }

        private void фильтрГауссаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GaussianFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void открытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Erosion();
            Filters filter2 = new Dillation();
            backgroundWorker1.RunWorkerAsync(filter);
            backgroundWorker1.RunWorkerAsync(filter2);
        }

        private void сужениеToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void расширениеToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void закрытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void backgroundWorker1_ProgressChanged_1(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            progressBar1.Refresh();
        }

        private void backgroundWorker1_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }
            progressBar1.Value = 0;
        }

  
        private void переносToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Transfer filter = new Transfer();
            Bitmap result = filter.processImage(image, backgroundWorker1);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void поворотToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Turn filter = new Turn();
            Bitmap result = filter.processImage(image, backgroundWorker1);
            backgroundWorker1.RunWorkerAsync(filter);

        }

        private void фильтрЩарраToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sharra filter = new Sharra();
            Bitmap result = filter.processImage(image, backgroundWorker1);
            backgroundWorker1.RunWorkerAsync(filter);
        }
        private void операторПрюитаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pruit filter = new Pruit();
            Bitmap result = filter.processImage(image, backgroundWorker1);
            backgroundWorker1.RunWorkerAsync(filter);
        }
        private void тиснениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Embosing filter = new Embosing();
            Bitmap result = filter.processImage(image, backgroundWorker1);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

    }
}
