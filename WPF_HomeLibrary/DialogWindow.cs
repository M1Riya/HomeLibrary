using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_HomeLibrary
{
    public class DialogWindow
    {
        public string FilePath { get; set; }
        public int FilterIndex { get; set; }
        public bool OpenFileDialog()
        {
            OpenFileDialog f = new OpenFileDialog();
            f.InitialDirectory = @"C:\Users\Ия\Документы";
            f.Filter = "Файл в xml|*.xml|Файл в json|*.json";
            if (f.ShowDialog() == true)
            {
                FilePath = f.FileName;
                FilterIndex = f.FilterIndex;
                return true;
            }
            return false;
        }

        public bool SaveFileDialog()
        {
            SaveFileDialog f = new SaveFileDialog();
            f.InitialDirectory = @"C:\Users\Ия\Документы";
            f.Filter = "Файл в xml|*.xml|Файл в json|*.json";
            if (f.ShowDialog() == true)
            {
                FilterIndex = f.FilterIndex;
                FilePath = f.FileName;
                return true;
            }

            return false;
        }
    }
}
