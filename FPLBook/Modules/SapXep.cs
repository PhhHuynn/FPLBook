using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPLBook.Modules
{
    public static class SapXep
    {
        public static List<Dictionary<string, string>> SapXepDanhSach(List<Dictionary<string, string>> records)
        {

            // Sắp xếp theo Publisher rồi Title
            var sortedBooks = records
                .OrderBy(b => b.ContainsKey("Publisher") ? b["Publisher"] : "")
                .ThenBy(b => b.ContainsKey("Title") ? b["Title"] : "")
                .ToList();

            return sortedBooks;
        }
    }
}
