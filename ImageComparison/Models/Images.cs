using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Drawing;
using ImageComparison.Properties;

namespace ImageComparison.Models
{
    class Images: INotifyPropertyChanged
    {
        public Images()
        {
            Img1 = Settings.Default["Img1"].ToString();
            Img2 = Settings.Default["Img2"].ToString();
            Img3 = Settings.Default["Img3"].ToString();
           
            
        }

        private string img1;
        public string Img1
        {
            get { return img1; }
            set
            {
                img1 = value;
                Settings.Default["Img1"] = value;
                OnPropertyChanged("Img1");
            }
        }

        private string img2;
        public string Img2
        {
            get { return img2; }
            set
            {
                img2 = value;
                Settings.Default["Img2"] = value;
                OnPropertyChanged("Img2");
            }
        }

        private string img3;
        public string Img3
        {
            get { return img3; }
            set
            {
                img3 = value;
                Settings.Default["Img3"] = value;
                OnPropertyChanged("Img3");
            }
        }

        private Bitmap img;
        public Bitmap Img
        {
            get { return img; }
            set
            {
                img = value;
                OnPropertyChanged("Img");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
