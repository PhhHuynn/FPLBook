using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using System.Reflection;
using System.Globalization;
namespace FPLBook.Modules
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int Helght { get; set; }
        public int Publisher { get; set; }
    }
}
