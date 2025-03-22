using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FPLBook.Modules
{
    public static class TimKiem
    {
        public static void TimKiemTheoTuKhoa(string keyword, FileInfo file, string columnSearch)
        {
            List<Dictionary<string, string>> records = ReadWriteCsvHelper.ReadCsvFromFile(file);

            if (records.Count == 0)
            {
                Console.WriteLine("Không có dữ liệu để tìm kiếm!");
                return;
            }

            if(columnSearch == "-1")
            {
                columnSearch = records[0].Keys.First();
            }

            // Kiểm tra cột có tồn tại không
            if (!records[0].ContainsKey(columnSearch))
            {
                Console.WriteLine($"Không tìm thấy cột '{columnSearch}' trong file CSV!");
                return;
            }

            keyword = keyword.Trim().ToLower();
            var searchResults = records.Where(record => record[columnSearch].ToLower().Contains(keyword)).ToList();

            if (searchResults.Count == 0)
            {
                Console.WriteLine("Không tìm thấy kết quả phù hợp!");
            }
            else
            {
                PrintTable(records[0].Keys.ToList(), searchResults);
            }
        }


        static void PrintTable(List<string> headers, List<Dictionary<string, string>> records)
        {
            int columnCount = headers.Count;
            int[] columnWidths = new int[columnCount];

            for (int i = 0; i < columnCount; i++)
            {
                string columnName = headers[i];
                columnWidths[i] = Math.Max(columnName.Length, records.Max(r => r[columnName]?.Length ?? 0)) + 2;
            }

            string headerRow = "| " + string.Join(" | ", headers.Select((h, i) => h.PadRight(columnWidths[i]))) + " |";
            Console.WriteLine(headerRow);
            Console.WriteLine(new string('-', headerRow.Length));

            foreach (var record in records)
            {
                string row = "| " + string.Join(" | ", headers.Select((h, i) => (record[h] ?? "").PadRight(columnWidths[i]))) + " |";
                Console.WriteLine(row);
            }
        }
    }
}
