using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;

namespace ImageViewer
{
    public partial class Form1 : Form
    {
        private readonly VideoCapture capture;
        bool loop;

        public Form1()
        {
            InitializeComponent();
            capture = new VideoCapture();
            loop = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            capture.Open(0, VideoCaptureAPIs.ANY);
            if (!capture.IsOpened())
            {
                return;
            }
            ClientSize = new System.Drawing.Size(capture.FrameWidth, capture.FrameHeight);
            Run();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            loop = false;
            capture.Dispose();
        }

        private async void Run()
        {
            var task = Task.Run(() => CaptureAsync());
            await task;
        }

        private void CaptureAsync()
        {
            while (loop)
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