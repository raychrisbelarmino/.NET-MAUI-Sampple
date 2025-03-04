using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiSample
{
    public class GPUModel: INotifyPropertyChanged
    {
        public GPUModel()
        {
        }
        private int _id { get; set; }
        private string _brand { get; set; }
        private string _model { get; set; }
        private string _image { get; set; }

        public int id { 
            get { return _id; } 
            set { _id = value; OnPropertyChanged(nameof(id));} 
        }
        public string model { 
            get { return _model; } 
            set { _model = value; OnPropertyChanged(nameof(model));} 
        }
        public string image { 
            get { return _image; } 
            set { _image = value; OnPropertyChanged(nameof(image));} 
        }
        public string brand { 
            get { return _brand; } 
            set { _brand = value; OnPropertyChanged(nameof(brand));} 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}