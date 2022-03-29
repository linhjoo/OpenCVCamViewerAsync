using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace ImageViewer
{
    public partial class Form1 : Form
    {
        private readonly VideoCapture capture;
        public Form1()
        {
            InitializeComponent();
            capture = new VideoCapture();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            capture.Open(0, VideoCaptureAPIs.ANY);
            if (!capture.IsOpened())
            {
                Close();
                return;
            }
            ClientSize = new System.Drawing.Size(capture.FrameWidth, capture.FrameHeight);
            backgroundWorker1.RunWorkerAsync();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            backgroundWorker1.CancelAsync();
            capture.Dispose();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                using (var frameMat = capture.RetrieveMat())
                {
                    var frameBitmap = BitmapConverter.ToBitmap(frameMat);

                    pictureBox1.Image?.Dispose();
                    pictureBox1.Image = frameBitmap;
                }
            }
        }
    }
}