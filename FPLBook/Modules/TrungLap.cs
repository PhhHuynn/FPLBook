using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPLBook.Modules
{
    internal class TrungLap
    {
        public static List<Dictionary<string, string>> LocTrungLap(List<Dictionary<string, string>> records, string[] columnTrungLap)
        {
            // HashSet chỉ cho phép string không lặp lại
            var uniqueCombinations = new HashSet<string>();

            // Danh sách các dữ liệu đã được lọc ra
            var deduplicatedRecords = new List<Dictionary<string, string>>();

            foreach (var record in records)
            {
                var combination = new StringBuilder();
                bool allColumnsPresent = true;

                foreach (var column in columnTrungLap)
                {
                    if (record.ContainsKey(column))
                    {
                        combination.Append(record[column]).Append("|");
                    }
                    else
                    {
                        allColumnsPresent = false;
                        break;
                    }
                }

                if (allColumnsPresent)
                {
                    if (uniqueCombinations.Add(combination.ToString()))
                    {
                        deduplicatedRecords.Add(record);
                    }
                }
                else
                {
                    deduplicatedRecords.Add(record);
                }
            }
            return deduplicatedRecords;
        }
    }
}
