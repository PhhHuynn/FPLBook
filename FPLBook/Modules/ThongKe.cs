using System;
using System.Collections.Generic;
using System.IO;

namespace FPLBook.Modules
{
    public static class ThongKe
    {
        public static void ThongKeTheoNhaXuatBanVaChuDe(List<Dictionary<string, string>> records)
        {
            if (records.Count == 0)
            {
                Console.WriteLine("Không có dữ liệu để tìm kiếm!");
                return;
            }

            var stats = new Dictionary<string, (int bookCount, HashSet<string> uniqueGenres)>();

            if (!records[0].ContainsKey("Publisher") || !records[0].ContainsKey("Genre"))
            {
                Console.WriteLine("Không tìm thấy cột 'Publisher' hoặc 'Genre' trong file CSV!");
                return;
            }

            foreach (var record in records)
            {
                if (!record.ContainsKey("Publisher") || !record.ContainsKey("Genre")) continue;

                string publisher = record["Publisher"].Trim();
                string genre = record["Genre"].Trim();

                if (string.IsNullOrEmpty(publisher)) continue;

                if (!stats.ContainsKey(publisher))
                {
                    stats[publisher] = (0, new HashSet<string>());
                }

                var (bookCount, uniqueGenres) = stats[publisher];
                uniqueGenres.Add(genre);
                stats[publisher] = (bookCount + 1, uniqueGenres);
            }

            PrintStatistics(stats);
        }

        static void PrintStatistics(Dictionary<string, (int bookCount, HashSet<string> uniqueGenres)> stats)
        {
            Console.WriteLine("\nThống kê số lượng sách và chủ đề theo nhà xuất bản:\n");
            Console.WriteLine($"{"Publisher",-25} | {"Số lượng sách",-15} | {"Số lượng chủ đề"}");
            Console.WriteLine(new string('-', 60));

            foreach (var entry in stats)
            {
                Console.WriteLine($"{entry.Key,-25} | {entry.Value.bookCount,-15} | {entry.Value.uniqueGenres.Count}");
            }
        }
    }
}
