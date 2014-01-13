using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AForge.Controls;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math;
using AForge.Vision;


namespace proj2
{
   
    public partial class Form1 : Form
    {

        
        FilterInfoCollection videoDevices;
        public Form1()
        {
            InitializeComponent();
        
        }

       
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // enumerate video devices
                  videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (videoDevices.Count == 0)
                    throw new Exception();
            
            }
            catch
                {
                  
                  MessageBox.Show("no cameras found");
                }
        }


        // On form closing
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopCameras();
        }

        // On "Start" button click
        private void button1_Click(object sender, EventArgs e)
        {
            StartCameras();

            StartButton.Enabled = false;
            StopButton.Enabled = true;

        }

        //On "Stop" button click
        private void button2_Click(object sender, EventArgs e)
        {
            StopCameras();

            StartButton.Enabled = true;

        }

         // Start cameras
        private void StartCameras()
        {
            // create first video source
            VideoCaptureDevice videoSource1 = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource1.DesiredFrameRate = 25;

            videoSourcePlayer1.VideoSource = videoSource1;
              videoSourcePlayer1.Start();
                 // start timer
            timer.Start();
        }


        // Stop cameras
        private void StopCameras()
        {
            timer.Stop();

            videoSourcePlayer1.SignalToStop();
            videoSourcePlayer1.WaitForStop();

        }

       
        private void timer1_Tick(object sender, EventArgs e)
        {
            IVideoSource videoSource1 = videoSourcePlayer1.VideoSource;
            Bitmap currentVideoFrame = videoSourcePlayer1.GetCurrentVideoFrame();
            pictureBox1.Image = currentVideoFrame;
                        
            if (currentVideoFrame != null)
            {
                Crop filter1 = new Crop(new Rectangle(1, 1, 319, 479));
                Crop filter2 = new Crop(new Rectangle(321, 1, 639, 479));
                Bitmap leftimage = filter1.Apply(currentVideoFrame);
                Bitmap rightimage = filter2.Apply(currentVideoFrame);
              

                // get grayscale image
                IFilter grayscaleFilter = new GrayscaleRMY();
                leftimage = grayscaleFilter.Apply(leftimage);
                rightimage = grayscaleFilter.Apply(rightimage);

                // apply threshold filter
                Threshold th = new Threshold(trackBar1.Value);
                Bitmap filteredImage1 = th.Apply(leftimage);
                pictureBox2.Image = filteredImage1;
                Bitmap filteredImage2 = th.Apply(rightimage);
                pictureBox3.Image = filteredImage2;
                label6.Text = trackBar1.Value.ToString();

                
                ImageStatistics lftstat = new ImageStatistics(filteredImage1);
                int lftpxlcntwthoutblck = lftstat.PixelsCountWithoutBlack;
                ImageStatistics rghtstat = new ImageStatistics(filteredImage2);
                int rghtpxlcntwthoutblck = rghtstat.PixelsCountWithoutBlack;

                int val = trackBar1.Value;
                
                if (((lftpxlcntwthoutblck - rghtpxlcntwthoutblck) > val) || ((rghtpxlcntwthoutblck - lftpxlcntwthoutblck) > val)) 
                     {
                        if ((lftpxlcntwthoutblck-rghtpxlcntwthoutblck) >val)
                            {
                                //label4.Text = "left";
                                label4.Text = "right";
                             }
                         if ((rghtpxlcntwthoutblck-lftpxlcntwthoutblck) >val)
                            {
                                //label4.Text = "right";
                                label4.Text = "left";
                            }
                     }
                else if ((lftpxlcntwthoutblck == 0) && (rghtpxlcntwthoutblck == 0))
                {
                    label4.Text = "Stop!! No  space ahead";
                }
                else    {
                    label4.Text = "Forward";

                        }
                    
           }
        }
                    
              
         // Apply filter to the source image and show the filtered image
        
        private void ApplyFilter(IFilter filter)
         {
           
           
         }
        
         
     
        private void videoSourcePlayer1_Click(object sender, EventArgs e)
        {
        
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void histogram1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

     
                        
    }


}
