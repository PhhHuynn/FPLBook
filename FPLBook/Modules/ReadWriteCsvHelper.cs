using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace FPLBook.Modules
{
    internal class ReadWriteCsvHelper
    {
        // Đọc file csv không có mã hóa
        public static List<Dictionary<string, string>> ReadCsvFromFile(FileInfo inputFile)
        {
            var records = new List<Dictionary<string, string>>();

            try
            {
                using (var fileStream = new FileStream(inputFile.FullName, FileMode.Open))
                {
                    Stream dataStream = fileStream;

                    using (var reader = new StreamReader(dataStream))
                    {
                        string[] headers = ParseCsvLine(reader.ReadLine());
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] values = ParseCsvLine(line);
                            var record = new Dictionary<string, string>();
                            for (int i = 0; i < headers.Length && i < values.Length; i++)
                            {
                                record[headers[i]] = values[i];
                            }
                            records.Add(record);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi đọc file CSV: {ex.Message}");
                return new List<Dictionary<string, string>>();
            }
            return records;
        }

        public static string[] ParseCsvLine(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return new string[0];
            }

            var fields = new List<string>();
            var currentField = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char currentChar = line[i];

                if (currentChar == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (currentChar == ',' && !inQuotes)
                {
                    fields.Add(TrimQuotes(currentField.ToString()));
                    currentField.Clear();
                }
                else
                {
                    currentField.Append(currentChar);
                }
            }

            fields.Add(TrimQuotes(currentField.ToString()));
            return fields.ToArray();
        }

        private static string TrimQuotes(string field)
        {
            if (field.StartsWith('"') && field.EndsWith('"'))
            {
                return field.Substring(1, field.Length - 2);
            }
            return field;
        }

        // Viết file csv không mã hóa
        public static void WriteCsvToFile(List<Dictionary<string, string>> records, FileInfo outputFile)
        {
            using (var fileStream = new FileStream(outputFile.FullName, FileMode.Create))
            {
                Stream dataStream = fileStream;

                using (var writer = new StreamWriter(dataStream))
                {
                    if (records.Count > 0)
                    {
                        // Write header row (keys), quoting if spaces exist
                        var headers = records[0].Keys.Select(key =>
                            key.Contains(" ") ? $"\"{key}\"" : key);
                        writer.WriteLine(string.Join(",", headers));

                        // Write data rows, quoting values with spaces
                        foreach (var record in records)
                        {
                            var values = record.Values.Select(value =>
                                value.Contains(" ") ? $"\"{value}\"" : value);
                            writer.WriteLine(string.Join(",", values));
                        }
                    }
                }
            }
        }

        // Mã hóa dòng dữ liệu bằng XOR cipher
        public static void EncryptFile(string inputFile, string outputFile, string key)
        {
            try
            {
                byte[] inputBytes = File.ReadAllBytes(inputFile);
                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
                byte[] encryptedBytes = XorCipher(inputBytes, keyBytes);
                File.WriteAllBytes(outputFile, encryptedBytes);
                Console.WriteLine("Đã mã hóa tệp csv.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi mã hóa: {ex.Message}");
            }
        }

        public static void DecryptFile(string inputFile, string outputFile, string key)
        {
            try
            {
                byte[] inputBytes = File.ReadAllBytes(inputFile);
                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
                byte[] decryptedBytes = XorCipher(inputBytes, keyBytes);
                File.WriteAllBytes(outputFile, decryptedBytes);
                Console.WriteLine("Đã giải mã tệp csv mã hóa.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi giải mã: {ex.Message}");
            }
        }

        private static byte[] XorCipher(byte[] data, byte[] key)
        {
            byte[] result = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                result[i] = (byte)(data[i] ^ key[i % key.Length]);
            }
            return result;
        }
    }
}
