using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FPLBook.Modules
{
    public static class TimKiem
    {

        public static void TimKiemTheoTieuDe(string keyword, string filePath)
        {
            List<string[]> books = new List<string[]>();
            string[] headers;
            
            int titleIndex = -1;
            using (var reader = new StreamReader(filePath))
            {
                headers = ParseCsvLine(reader.ReadLine());
                titleIndex = Array.FindIndex(headers, h => h.Trim().ToLower() == "title");

                if (titleIndex == -1)
                {
                    Console.WriteLine("❌ Không tìm thấy cột 'Title' trong file!");
                    return;
                }
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
            var searchResults = books.Where(b => b[titleIndex].ToLower().Contains(keyword)).ToList();

            if (searchResults.Count == 0)
            {
                Console.WriteLine("Không tìm thấy sách nào!");
            }
            else
            {
                PrintTable(headers, searchResults);
            }
        }

        static string[] ParseCsvLine(string line)
        {
            List<string> result = new List<string>();
            bool inQuotes = false;
            string current = "";

            foreach (char c in line)
            {
                if (c == '"' && inQuotes)
                    inQuotes = false;
                else if (c == '"')
                    inQuotes = true;
                else if (c == ',' && !inQuotes)
                {
                    result.Add(current.Trim());
                    current = "";
                }
                else
                    current += c;
            }
            result.Add(current.Trim());

            return result.ToArray();
        }

        static void PrintTable(string[] headers, List<string[]> books)
        {
            int columnCount = headers.Length;
            int[] columnWidths = new int[columnCount];

            for (int i = 0; i < columnCount; i++)
            {
                columnWidths[i] = Math.Max(headers[i].Length, books.Max(b => b[i].Length)) + 2;
            }

            string headerRow = "| " + string.Join(" | ", headers.Select((h, i) => h.PadRight(columnWidths[i]))) + " |";
            Console.WriteLine(headerRow);
            Console.WriteLine(new string('-', headerRow.Length));

            foreach (var book in books)
            {
                string row = "| " + string.Join(" | ", book.Select((b, i) => b.PadRight(columnWidths[i]))) + " |";
                Console.WriteLine(row);
            }
        }

    }
}
