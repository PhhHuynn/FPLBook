﻿using System.Runtime.CompilerServices;
using System.CommandLine;
using FPLBook.Modules;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        /*
         * Vị trí thêm option cho lệnh
         */

        // Flag đọc và xuất file (1)
        var inputFileOption = new Option<FileInfo?>(
            name: "--inputFile",
            description: "Lấy vị trí file và đọc để xử lý.");
        var outputFileOption = new Option<FileInfo?>(
            name: "--outputFile",
            description: "Vị trí file xuất sau khi xử lý.");
        // 2: Gán mã số cho sách và tạo file mới
        var indexAfterOption = new Option<int>(
            name: "--after",
            description: "Đặt vị trí cột index ở sau một cột bất kỳ (Nếu để là -1, cột sẽ được đặt ở vị trí đầu tiên)",
            getDefaultValue: () => -1);
        // 3: Sắp xếp sách theo nhà xuất bản và tiêu đề
        var sortColumnOption = new Option<string[]>(
            name: "--sortColumn",
            description: "Tên cột để xếp theo thứ tự (tách nhau bởi dấu phẩy (,))");

        // 4: Tìm kiếm sách theo từ khóa (lọc từ khóa)
        var searchKeywordOption = new Option<string[]>(
            name: "--searchKeyword",
            description: "Từ khóa tìm kiếm");
        var searchColumnOption = new Option<string[]>(
            name: "--searchColumn",
            description: "Tên cột để tìm kiếm (tách nhau bởi dấu phẩy (,))");

        // 5: Thống kê số lượng sách và chủ đề theo nhà xuất bản
        // Không có flag

        // 6: Xử lý sách trùng lặp
        // Không có flag

        // 7, 8
        var encryptionKeyOption = new Option<string>(
            name: "--encryptionKey",
            description: "Đặt chìa khóa mã hóa cho tệp tin xuất ra.");
        var decryptionKeyOption = new Option<string>(
            name: "--decryptionKey",
            description: "Đặt chìa khóa giải mã cho tệp tin xuất ra.");

        var rootCommand = new RootCommand("CLI FPLBook xử lý file");
        //rootCommand.AddOption(fileOption);


        /*
         * Vị trí thêm lệnh
         */

        // 2
        var indexCommand = new Command("index", "Tạo cột index cho tệp tin sau cột được cho.")
            {
                inputFileOption,
                indexAfterOption,
                outputFileOption,
            };

        // 3
        var sortCommand = new Command("sort", "Sắp xếp tệp tin theo cột được cho.")
            {
                inputFileOption,
                sortColumnOption,
                outputFileOption,
            };

        // 4
        var searchCommand = new Command("search", "Tìm kiếm tệp tin theo cột được cho.")
            {
                inputFileOption,
                searchKeywordOption,
                searchColumnOption,
                outputFileOption,
            };

        // 5
        var analyticsCommand = new Command("analytics", "Thống kê tệp tin theo cột được cho.")
            {
                inputFileOption,
                outputFileOption,
            };

        // 6
        var duplicateCommand = new Command("duplicate", "Lọc dữ liệu bị trùng trong tệp tin theo cột được cho và xuất ra tệp tin.")
            {
                inputFileOption,
                outputFileOption,
            };

        // 7
        var encryptCommand = new Command("encrypt", "Mã hóa tệp tin.")
            {
                inputFileOption,
                encryptionKeyOption,
                outputFileOption,
            };

        // 8
        var decryptCommand = new Command("decrypt", "Giải mã hóa tệp tin.")
            {
                inputFileOption,
                encryptionKeyOption,
                outputFileOption,
            };

        // Thêm lệnh vào root
        rootCommand.AddCommand(indexCommand); // 2
        rootCommand.AddCommand(sortCommand); // 3
        rootCommand.AddCommand(searchCommand); // 4
        rootCommand.AddCommand(analyticsCommand); // 5
        rootCommand.AddCommand(duplicateCommand); // 6
        rootCommand.AddCommand(encryptCommand); // 7
        rootCommand.AddCommand(decryptCommand); // 8
        
        /*
         * Thêm phần xử lý cho lệnh
         */

        // 2
        indexCommand.SetHandler(async (input, indexAfter, output) =>
        {
            // Cho lệnh ở dưới
            //await 
           var records = ReadWriteCsvHelper.ReadCsvFromFile(input);

            records = FPLBook.Modules.Index.MoveIndexColumn(records, indexAfter);
            ReadWriteCsvHelper.WriteCsvToFile(records, output);

        }, inputFileOption, indexAfterOption, outputFileOption);

        // 3
        sortCommand.SetHandler(async (input, sortColumn, output) =>
        {
            // Cho lệnh ở dưới
            //await 
            
        }, inputFileOption, sortColumnOption, outputFileOption);

        // 4
        searchCommand.SetHandler(async (input, searchKeyword, searchColumn, output) =>
        {
            // Cho lệnh ở dưới
            //await 
        }, inputFileOption, searchKeywordOption, searchColumnOption, outputFileOption);

        // 5
        analyticsCommand.SetHandler(async (input, output) =>
        {
            // Cho lệnh ở dưới
            //await 
        }, inputFileOption, outputFileOption);

        // 6
        duplicateCommand.SetHandler(async (input, output) =>
        {
            // Cho lệnh ở dưới
            //await 
        }, inputFileOption, outputFileOption);

        // 7
        encryptCommand.SetHandler(async (input, encryptionKey, output) =>
        {
            ReadWriteCsvHelper.WriteCsvToFile(ReadWriteCsvHelper.ReadCsvFromFile(input), output, encryptionKey);
        }, inputFileOption, encryptionKeyOption, outputFileOption);

        // 8
        decryptCommand.SetHandler(async (input, decryptionKey, output) =>
        {
            ReadWriteCsvHelper.WriteCsvToFile(ReadWriteCsvHelper.ReadCsvFromFile(input, decryptionKey), output);
        }, inputFileOption, decryptionKeyOption, outputFileOption);


        return await rootCommand.InvokeAsync(args);
    }
}