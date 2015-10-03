using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImageComparison.Commands;
using ImageComparison.Models;
using ImageComparison.Properties;
using System.Drawing;
using System.IO;
using System.Windows.Input;
using System.Windows;
using System.Threading;

namespace ImageComparison.ViewModels
{
    internal class ImageComparisonViewModel
    {
        public ImageComparisonViewModel()
        {
            _Images = new Images();
            OpenResultCommand = new OpenCompareResultCommand(this);
            CompareCommand = new CompareTwoImagesCommand(this);
            File.Delete(Settings.Default["Img3"].ToString());
        }

        private Images _Images;
        public Images Images
        {
            get { return _Images; }
        }

        public ICommand CompareCommand
        {
            get;
            private set;
        }

        public ICommand OpenResultCommand
        {
            get;
            private set;
        }

        public bool CanCompare
        {
            get
            {
                if ((_Images.Img1.Length > 3) && (_Images.Img2.Length > 3))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool CanOpen
        {
            get
            {
                if (File.Exists(_Images.Img3))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Open()
        {

            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.DefaultExt = ".jpg";
            sfd.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            Nullable<bool> result = sfd.ShowDialog();
            if (result == true)
            {
                string saveFilePath = sfd.FileName;
                MessageBox.Show(saveFilePath);
                Bitmap imgSave = new Bitmap(Settings.Default["Img3"].ToString());
                imgSave.Save(saveFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                // Open document 
                //Settings.Default["Img2"] = dlg.FileName;
                //image2.Source = new BitmapImage(new Uri(dlg.FileName));
                //Settings.Default.Save();
            }


        }

        public void Compare()
        {
            Bitmap img1 = new Bitmap(Settings.Default["Img1"].ToString());
            Bitmap img2 = new Bitmap(Settings.Default["Img2"].ToString());

            int width = img1.Width;
            int height = img1.Height;
            Bitmap img3 = new Bitmap(width, height);

            List<int> xs = new List<int>();
            List<int> ys = new List<int>();
            List<Coordinates> xy = new List<Coordinates>();

            bool are_identical = true;
            int d = 25;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int firstRed = img1.GetPixel(x, y).R;
                    int firstGreen = img1.GetPixel(x, y).G;
                    int firstBlue = img1.GetPixel(x, y).B;
                    int secondRed = img2.GetPixel(x, y).R;
                    int secondGreen = img2.GetPixel(x, y).G;
                    int secondBlue = img2.GetPixel(x, y).B;

                    if (firstRed >= secondRed - d && firstRed <= secondRed + d)
                    {
                        if (firstBlue >= secondBlue - d && firstBlue <= secondBlue + d)
                        {
                            if (firstRed >= secondRed - d && firstRed <= secondRed + d)
                            {

                            }
                        }
                    }
                    else
                    {
                        xs.Add(x);
                        ys.Add(y);
                        xy.Add(new Coordinates { xValue = x, yValue = y });
                        are_identical = false;
                    }
                }
            }

            Pen redPen = new Pen(Color.Red);

            List<Coordinates> xyPart = new List<Coordinates>();
            List<List<Coordinates>> xyTemp = new List<List<Coordinates>>();
            List<List<Coordinates>> xyList = new List<List<Coordinates>>();

            for (int i = 0; i < xy.Count; i++)
            {
                xyPart.Add(xy[i]);
                if (i != 0)
                {
                    if ((xy[i].xValue - xy[i - 1].xValue) > 2)
                    {
                        xyPart.RemoveAt(xyPart.Count - 1);
                        xyTemp.Add(xyPart);
                        xyPart = new List<Coordinates>();
                    }
                }
            }
            xyTemp.Add(xyPart);

            List<List<Coordinates>> xyFinal = new List<List<Coordinates>>();
            for (int i = 0; i < xyTemp.Count; i++)
            {
                List<Coordinates> Part = new List<Coordinates>();

                List<Coordinates> lst = new List<Coordinates>();
                lst = xyTemp[i].OrderBy(y => y.yValue).ToList();
                for (int j = 0; j < lst.Count; j++)
                {

                    Part.Add(lst[j]);
                    if (j != 0)
                    {
                        if ((lst[j].yValue - lst[j - 1].yValue) > 2)
                        {
                            Part.RemoveAt(Part.Count - 1);
                            xyFinal.Add(Part);
                            Part = new List<Coordinates>();
                        }
                    }
                }
                xyFinal.Add(Part);
            }

            List<int> xsPart = new List<int>();
            List<int> ysPart = new List<int>();

            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            string savePath = appPath + Settings.Default["ImageNumber"].ToString() + ".jpg";
            Settings.Default["ImageNumber"] = (int)Settings.Default["ImageNumber"] + 1;
            
            img3 = new Bitmap(img2);
            if (!are_identical)
            {
                using (var graphics = Graphics.FromImage(img3))
                {
                    for (int i = 0; i < xyFinal.Count; i++)
                    {
                        foreach (var item in xyFinal[i])
                        {
                            xsPart.Add(item.xValue);
                            ysPart.Add(item.yValue);
                        }
                        try
                        {
                            int startPartX = xsPart.Min();
                            int startPartY = ysPart.Min();
                            int rectPartWidth = xsPart.Max() - xsPart.Min();
                            int rectPartHeight = ysPart.Max() - ysPart.Min();
                            graphics.DrawRectangle(redPen, startPartX, startPartY, rectPartWidth, rectPartHeight);
                            xsPart.Clear();
                            ysPart.Clear();
                        }
                        catch (Exception)
                        {
                            
                        }
                    }
                }
            }

            Bitmap bmp1 = (Bitmap)img3.Clone();
            img3 = img1;

            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }

            bmp1.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);
            bmp1.Dispose();

            Images.Img3 = savePath;
            Settings.Default["Img3"] = savePath;
            Settings.Default.Save();
           
            
        }

    }
}
