using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPLBook.Modules
{
    public static class SapXep
    {
        public static List<Dictionary<string, string>> SapXepDanhSach(List<Dictionary<string, string>> records, string[] sortColumns)
        {
            if (records == null || records.Count == 0 || sortColumns == null || sortColumns.Length == 0)
            {
                Console.WriteLine("Không có dữ liệu hoặc danh sách cột sắp xếp bị trống.");
                return records ?? new List<Dictionary<string, string>>();
            }

            // Kiểm tra xem các cột có tồn tại không
            var validSortColumns = new List<string>();
            if (records != null && records.Count > 0 && records[0] != null)
            {
                validSortColumns = sortColumns
                    .Where(col => records[0].ContainsKey(col))
                    .ToList();
            }
            if (validSortColumns.Count == 0)
            {
                Console.WriteLine("Các cột sắp xếp không hợp lệ!");
                return records;
            }

            foreach (var col in validSortColumns)
            {
                records = records.OrderBy(row => row[col]).ToList();
            }
            return records.ToList();
        }


    }
}