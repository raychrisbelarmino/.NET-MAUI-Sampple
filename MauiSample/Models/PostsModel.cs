using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiSample
{
    public class PostsModel : INotifyPropertyChanged
    {
        public PostsModel()
        {
        }
        private int _userId { get; set; }
        private int _id { get; set; }
        private string _title { get; set; }
        private string _body { get; set; }


        public int userId
        {
            get { return _userId; }
            set { _userId = value; OnPropertyChanged(nameof(userId)); }
        }

        public int id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(id)); }
        }

        public string title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(nameof(title)); }
        }

        public string body
        {
            get { return _body; }
            set { _body = value; OnPropertyChanged(nameof(body)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}