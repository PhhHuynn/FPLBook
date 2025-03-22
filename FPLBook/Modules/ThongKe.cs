using System;
using System.Collections.Generic;
using System.IO;

namespace FPLBook.Modules
{
    public static class ThongKe
    {
        public static void ThongKeTheoNhaXuatBan(string filePath)
        {
            var stats = new Dictionary<string, (int bookCount, HashSet<string> uniqueGenres)>();
            string[] headers;
            int publisherIndex, genreIndex;

            using (var reader = new StreamReader(filePath))
            {
                headers = ParseCsvLine(reader.ReadLine());
                publisherIndex = Array.FindIndex(headers, h => h.Trim().ToLower() == "publisher");
                genreIndex = Array.FindIndex(headers, h => h.Trim().ToLower() == "genre");

                if (publisherIndex == -1 || genreIndex == -1)
                {
                    Console.WriteLine("Không tìm thấy cột 'Publisher' hoặc 'Genre' trong file CSV!");
                    return;
                }

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = ParseCsvLine(line);
                    if (values.Length <= Math.Max(publisherIndex, genreIndex)) continue;

                    string publisher = values[publisherIndex].Trim();
                    string genre = values[genreIndex].Trim();

                    if (string.IsNullOrEmpty(publisher)) continue;

                    if (!stats.ContainsKey(publisher))
                    {
                        stats[publisher] = (0, new HashSet<string>());
                    }

                    var (bookCount, uniqueGenres) = stats[publisher];
                    uniqueGenres.Add(genre); // Thêm vào HashSet để loại bỏ trùng lặp
                    stats[publisher] = (bookCount + 1, uniqueGenres);
                }
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
    }
}
