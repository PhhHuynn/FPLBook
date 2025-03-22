using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPLBook.Modules
{
    public static class TimKiem
    {

        public static void TimKiemTheoTieuDe(string keyword, string filePath) 
        {
            List<string[]> books = new List<string[]>();

            using (var reader = new StreamReader(filePath))
            {
                string headerLine = reader.ReadLine();
                string[] headers = headerLine.Split(',');

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] columns = ParseCsvLine(line);

                    while (columns.Length < headers.Length)
                    {
                        columns = columns.Append("N/A").ToArray();
                    }

                    books.Add(columns);
                }
            }

            keyword = keyword.Trim().ToLower();

            var searchResults = books.Where(b => b[0].ToLower().Contains(keyword)).ToList();

            if (searchResults.Count == 0)
            {
                Console.WriteLine("❌ Không tìm thấy sách nào!");
            }
            else
            {
                PrintTable(searchResults);
            }
        }

        // Xử lý dòng CSV chứa dấu ngoặc kép (dùng cho những ô có dấu phẩy)
        static string[] ParseCsvLine(string line)
        {
            List<string> result = new List<string>();
            bool inQuotes = false;
            string current = "";

            foreach (char c in line)
            {
                if (c == '"' && inQuotes)
                {
                    inQuotes = false;
                }
                else if (c == '"')
                {
                    inQuotes = true; 
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(current.Trim());
                    current = "";
                }
                else
                {
                    current += c;
                }
            }
            result.Add(current.Trim());

            return result.ToArray();
        }

        static void PrintTable(List<string[]> books)
        {
            int[] columnWidths = { 40, 30, 20, 10, 25 };

            string header = string.Format(
                "| {0,-" + columnWidths[0] + "} | {1,-" + columnWidths[1] + "} | {2,-" + columnWidths[2] + "} | {3,-" + columnWidths[3] + "} | {4,-" + columnWidths[4] + "} |",
                "Title", "Author", "Genre", "Height", "Publisher");

            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));

            foreach (var book in books)
            {
                Console.WriteLine(string.Format(
                    "| {0,-" + columnWidths[0] + "} | {1,-" + columnWidths[1] + "} | {2,-" + columnWidths[2] + "} | {3,-" + columnWidths[3] + "} | {4,-" + columnWidths[4] + "} |",
                    book[0], book[1], book[2], book[3], book[4]));
            }
        }

    }
}
