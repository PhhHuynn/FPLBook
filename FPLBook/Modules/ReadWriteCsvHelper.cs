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
            return records;
        }

        // Đọc file csv có mã hóa
        public static List<Dictionary<string, string>> ReadCsvFromFile(FileInfo inputFile, string decryptionKey)
        {
            var records = new List<Dictionary<string, string>>();
            using (var fileStream = new FileStream(inputFile.FullName, FileMode.Open))
            {
                Stream dataStream = fileStream;
                if (!string.IsNullOrEmpty(decryptionKey))
                {
                    dataStream = DecryptStream(fileStream, decryptionKey);
                }

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
            return records;
        }

        private static string[] ParseCsvLine(string line)
        {
            var fields = new List<string>();
            var currentField = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (line[i] == ',' && !inQuotes)
                {
                    fields.Add(currentField.ToString());
                    currentField.Clear();
                }
                else
                {
                    currentField.Append(line[i]);
                }
            }
            fields.Add(currentField.ToString());
            return fields.ToArray();
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
                        writer.WriteLine(string.Join(",", records[0].Keys));
                        foreach (var record in records)
                        {
                            writer.WriteLine(string.Join(",", record.Values));
                        }
                    }
                }
            }
        }

        // Viết file csv với mã hóa
        public static void WriteCsvToFile(List<Dictionary<string, string>> records, FileInfo outputFile, string encryptionKey)
        {
            using (var fileStream = new FileStream(outputFile.FullName, FileMode.Create))
            {
                Stream dataStream = fileStream;
                if (!string.IsNullOrEmpty(encryptionKey))
                {
                    dataStream = EncryptStream(fileStream, encryptionKey);
                }

                using (var writer = new StreamWriter(dataStream))
                {
                    if (records.Count > 0)
                    {
                        writer.WriteLine(string.Join(",", records[0].Keys));
                        foreach (var record in records)
                        {
                            writer.WriteLine(string.Join(",", record.Values));
                        }
                    }
                }
            }
        }

        // Mã hõa dòng dữ liệu bằng AES
        private static Stream EncryptStream(Stream inputStream, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key.PadRight(32, '\0').Substring(0, 32));
                aesAlg.IV = new byte[16];

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                CryptoStream cryptoStream = new CryptoStream(inputStream, encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(aesAlg.IV, 0, 16);
                return cryptoStream;
            }
        }

        private static Stream DecryptStream(Stream inputStream, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key.PadRight(32, '\0').Substring(0, 32));
                byte[] iv = new byte[16];
                inputStream.Read(iv, 0, 16);
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                return new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read);
            }
        }
    }
}
