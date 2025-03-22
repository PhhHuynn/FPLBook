using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FPLBook.Modules
{
    public static class TimKiem
    {
        public static void TimKiemTheoTuKhoa(List<Dictionary<string, string>> records, string keyword, string[] columnSearch)
        {
            if (records.Count == 0)
            {
                Console.WriteLine("Không có dữ liệu để tìm kiếm!");
                return;
            }

            List<string> searchColumns = new List<string>();

            // Nếu -1 thì tìm kiếm trên tất cả cột
            if (columnSearch[0] == "-1")
            {
                searchColumns = records[0].Keys.ToList();
            }
            else
            {
                foreach (var col in columnSearch)
                {
                    if (!records[0].ContainsKey(col))
                    {
                        Console.WriteLine($"Không tìm thấy cột '{col}' trong file CSV!");
                        return;
                    }
                    searchColumns.Add(col);
                }
            }

            keyword = keyword.Trim().ToLower();
            var searchResults = records.Where(record =>
                searchColumns.Any(col => record[col].ToLower().Contains(keyword))
            ).ToList();

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
