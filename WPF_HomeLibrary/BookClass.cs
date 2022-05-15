using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WPF_HomeLibrary
{
    public class BookClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        string name;
        string author;
        string publishing;
        int year;
        int page;
        public string Name 
        { get => name;
            set 
            { 
                name = value;
                OnPropertyChanged("Name");
            } 
        }
        public string Author 
        { 
            get => author;
            set
            { 
                author = value;
                OnPropertyChanged("Author");
            } 
        }
        public string Publishing 
        { 
            get => publishing;
            set
            {
                publishing = value;
                OnPropertyChanged("Publishing");
            }
        }
        public int Year
        {
            get => year;
            set
            { 
                year = value;
                OnPropertyChanged("Year");
            }
        }
        public int Page
        {
            get => page;
            set 
            { 
                page = value;
                OnPropertyChanged("Page");
            }
        }
        public BookClass(string name, string author, int year, int page, string publishing = null)
        {
            Name = name;
            Author = author;
            Publishing = publishing;
            Year = year;
            Page = page;
        }
        public BookClass()
        { 
            Name = "";
            Author = "";
            Publishing = "";
            Year = 0;
            Page = 0;
        }
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
