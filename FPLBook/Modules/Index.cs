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
        public static List<Dictionary<string, string>> MoveIndexColumn(List<Dictionary<string, string>> records, int afterIndex, string byUniqueColumnName)
        {
            if (records.Count == 0) return records;
            var headers = records.First().Keys.ToList();

            // Nếu không có "BookIndex", tự động thêm
            if (!headers.Any(h => h.Trim().Equals("BookIndex", StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Không tìm thấy cột 'BookIndex', tự động thêm...");
                headers.Insert(0, "BookIndex"); // Thêm "BookIndex" vào đầu danh sách cột

                // Gán số thứ tự cho từng dòng, nhóm theo tiêu đề sách
                Dictionary<string, int> titleIndexMap = new Dictionary<string, int>();
                int currentIndex = 1;

                for (int i = 0; i < records.Count; i++)
                {
                    if (records[i].ContainsKey(byUniqueColumnName))
                    {
                        string title = records[i][byUniqueColumnName];
                        if (!titleIndexMap.ContainsKey(title))
                        {
                            titleIndexMap[title] = currentIndex;
                            currentIndex++;
                        }
                        records[i]["BookIndex"] = titleIndexMap[title].ToString();
                    }
                    else
                    {
                        // Kiểm tra trường hợp khi dữ liệu cột Title tại dòng hiện tại trống
                        records[i]["BookIndex"] = (i + 1).ToString();
                    }
                }
            }

            // Di chuyển cột "BookIndex" về vị trí đc cho
            headers.Remove("BookIndex");
            if (afterIndex == -1)
                headers.Insert(0, "BookIndex");
            else if (afterIndex < headers.Count)
                headers.Insert(afterIndex + 1, "BookIndex");
            else
                headers.Add("BookIndex");

            return records.Select(row => headers.ToDictionary(h => h, h => row.ContainsKey(h) ? row[h] : "")).ToList();
        }
    }
}
