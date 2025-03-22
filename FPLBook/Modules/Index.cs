using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Linq;
namespace FPLBook.Modules
{
   internal class Index
    {
        public static List<Dictionary<string, string>> MoveIndexColumn(List<Dictionary<string, string>> records, int afterIndex)
        {
            if (records.Count == 0) return records;
            var headers = records.First().Keys.ToList();

            // ✅ Nếu không có "Index", tự động thêm
            if (!headers.Any(h => h.Trim().Equals("Index", StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Không tìm thấy cột 'Index', tự động thêm...");
                headers.Insert(0, "Index"); // Thêm "Index" vào đầu danh sách cột

                // Gán số thứ tự cho từng dòng
                for (int i = 0; i < records.Count; i++)
                {
                    records[i]["Index"] = (i + 1).ToString();
                }
            }

            // 🚀 Di chuyển cột "Index" về vị trí mong muốn
            headers.Remove("Index");
            if (afterIndex == -1)
                headers.Insert(0, "Index");
            else if (afterIndex < headers.Count)
                headers.Insert(afterIndex + 1, "Index");
            else
                headers.Add("Index");

            return records.Select(row => headers.ToDictionary(h => h, h => row.ContainsKey(h) ? row[h] : "")).ToList();
        }
    }
}
