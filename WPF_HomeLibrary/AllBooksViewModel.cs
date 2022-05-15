using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace WPF_HomeLibrary
{
    public class AllBooksViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        BookClass selectedBook;
        BaseCommand addCommand;
        BaseCommand removeCommand;
        BaseCommand saveCommand;
        BaseCommand openCommand;
        DialogWindow dialogW;
        public BookClass SelectedBook
        {
            get => selectedBook;
            set
            {
                selectedBook = value;
                OnPropertyChanged("SelectedBook");
            }
        }
        public ObservableCollection<BookClass> AllBooks { get; set; }
        public AllBooksViewModel(DialogWindow d)
        {
            dialogW = d;
            AllBooks = new ObservableCollection<BookClass>
            {
                 new BookClass {Name="Коллекционер", Author="Джон Фаулз", Year=2011, Publishing="Эксмо-Пресс", Page=400},
                 new BookClass {Name="Норвежский лес", Author="Харуки Мураками", Year=2015, Publishing="Эксмо", Page=368 },
                 new BookClass {Name="Джейн Эйр", Author="Шарлотта Бронте", Year=2008, Page=512 },
                 new BookClass {Name="Над пропастью во ржи", Author="Джером Д. Сэлинджер", Year=2013,  Publishing="Эксмо", Page=224 },
                 new BookClass {Name="Праздник, который всегда с тобой", Author="Эрнест Хемингуэй", Year=2015,  Publishing="АСТ", Page=288 }
            };
        }

        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public BaseCommand AddCommand
        {
            get
            {
                return addCommand ??
                (addCommand = new BaseCommand(obj =>
                {
                    BookClass Book = new BookClass();
                    AllBooks.Add( Book);
                    SelectedBook = Book;
                }));
            }
        }
        public BaseCommand RemoveCommand
        {
            get
            {
                if (removeCommand != null)
                    return removeCommand;
                else
                {
                    Action<object> Execute = o =>
                    {
                        BookClass b = (BookClass)o;
                        AllBooks.Remove(b);
                    };
                    Func<object, bool> CanExecute = o => AllBooks.Count > 0;
                    removeCommand = new BaseCommand(Execute, CanExecute);
                    return removeCommand;
                }

            }
        }
        public BaseCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                (saveCommand = new BaseCommand(obj =>
                {
                    if (dialogW.SaveFileDialog() == true)
                    {
                        switch (dialogW.FilterIndex)
                        {
                            case 1:
                                XMLSave(dialogW.FilePath, AllBooks);
                                break;
                            case 2:
                                JSONSave(dialogW.FilePath, AllBooks);
                                break;
                        }
                    }
                }));
            }
        }
        public BaseCommand OpenCommand
        {
            get
            {
                return openCommand ?? (openCommand = new BaseCommand(obj =>
                {

                    if (dialogW.OpenFileDialog() == true)
                    {
                        switch (dialogW.FilterIndex)
                        {
                            case 1:
                                var xmlBookFile = XMLOpen(dialogW.FilePath);
                                AllBooks.Clear();
                                foreach (var p in xmlBookFile)
                                    AllBooks.Add(p);
                                break;
                            case 2:
                                var jsonBookFile = JSONOpen(dialogW.FilePath);
                                AllBooks.Clear();
                                foreach (var p in jsonBookFile)
                                    AllBooks.Add(p);
                                break;
                        }
                    }
                }));
            }
        }
        public ObservableCollection<BookClass> XMLOpen(string filename)
        {
            TextReader tr = null;
            try
            {
                tr = new StreamReader(filename);
                ObservableCollection<BookClass> doneBase = new ObservableCollection<BookClass>();
                var read = tr.ReadToEnd();
                var x = XElement.Parse(read);
                var res = from e in x.Elements()
                          select new BookClass
                          (e.Element("Name").Value, e.Element("Author").Value, Convert.ToInt32(e.Element("Year").Value), Convert.ToInt32(e.Element("Page").Value), e.Element("Publishing").Value);
                foreach (var book in res)
                {
                    doneBase.Add(book);
                }
                return doneBase;
            }
            finally
            {
                if (tr != null)
                    tr.Close();
            }
        }
        public void XMLSave(string filename, ObservableCollection<BookClass> books)
        {
            TextWriter writer = null;
            try
            {
                var x = new XElement("AllBooks",
                            from book in books
                            select new XElement("Library",
                                new XElement("Name", book.Name),
                                new XElement("Author", book.Author),
                                new XElement("Publishing", book.Publishing),
                                new XElement("Year", book.Year),
                                new XElement("Page", book.Page)));
                string s = x.ToString();
                writer = new StreamWriter(filename);
                writer.Write(s);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
        public ObservableCollection<BookClass> JSONOpen(string filename)
        {
            TextReader tr = null;
            try
            {
                tr = new StreamReader(filename);
                var read = tr.ReadToEnd();
                return JsonConvert.DeserializeObject<ObservableCollection<BookClass>>(read);
            }
            finally
            {
                if (tr != null)
                    tr.Close();
            }
        }

        public void JSONSave(string filename, ObservableCollection<BookClass> books)
        {
            TextWriter writer = null;
            try
            {
                string s = JsonConvert.SerializeObject(books, Formatting.Indented);
                writer = new StreamWriter(filename);
                writer.Write(s);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
    }
}
