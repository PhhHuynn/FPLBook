using System.Runtime.CompilerServices;
using System.CommandLine;
using FPLBook.Modules;
using System.CommandLine.Parsing;

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
            description: "Lấy vị trí file và đọc để xử lý.")
            {
                IsRequired = true,
            };
        var outputFileOption = new Option<FileInfo?>(
            name: "--outputFile",
            description: "Vị trí file xuất sau khi xử lý.")
            {
                IsRequired = true,
            };

        // 2: Gán mã số cho sách và tạo file mới
        var indexAfterOption = new Option<int>(
            name: "--after",
            description: "Đặt vị trí cột index ở sau một cột bất kỳ (Nếu để là -1, cột sẽ được đặt ở vị trí đầu tiên)",
            getDefaultValue: () => -1);

        // 3: Sắp xếp sách theo nhà xuất bản và tiêu đề
        var sortColumnsOption = new Option<string[]>(
            name: "--sortColumns",
            description: "Tên cột để xếp theo thứ tự (tách nhau bởi dấu cách)")
            {
                IsRequired = true,
                AllowMultipleArgumentsPerToken = true,
            };

        // 4: Tìm kiếm sách theo từ khóa (lọc từ khóa)
        var searchKeywordOption = new Option<string>(
            name: "--searchKeyword",
            description: "Từ khóa tìm kiếm")
            {
                IsRequired = true,
            };
        var searchColumnsOption = new Option<string[]>(
            name: "--searchColumns",
            description: "Tên cột để tìm kiếm (tách nhau bởi dấu cách)")
            {
                IsRequired = true,
                AllowMultipleArgumentsPerToken = true,
            };

        // 5: Thống kê số lượng sách và chủ đề theo nhà xuất bản
        // Không có flag

        // 6: Xử lý sách trùng lặp
        var duplicateColumnsOption = new Option<string[]>(
            name: "--duplicateColumns",
            description: "Tên cột để lọc trùng lặp (tách nhau bằng dấu cách)")
            {
                IsRequired = true,
                AllowMultipleArgumentsPerToken = true,
            };
        

        // 7, 8
        var encryptionKeyOption = new Option<string>(
            name: "--encryptionKey",
            description: "Đặt chìa khóa mã hóa cho tệp tin xuất ra.")
            {
                IsRequired = true,
            };
        var decryptionKeyOption = new Option<string>(
            name: "--decryptionKey",
            description: "Đặt chìa khóa giải mã cho tệp tin xuất ra.")
            {
                IsRequired = true,
            };

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
                sortColumnsOption,
                outputFileOption,
            };

        // 4
        var searchCommand = new Command("search", "Tìm kiếm tệp tin theo cột được cho.")
            {
                inputFileOption,
                searchKeywordOption,
                searchColumnsOption,
            };

        // 5
        var analyticsPublisherTitleCommand = new Command("analytics", "Thống kê tệp tin theo cột được cho.")
            {
                inputFileOption,
            };

        // 6
        var duplicateCommand = new Command("duplicate", "Lọc dữ liệu bị trùng trong tệp tin theo cột được cho và xuất ra tệp tin.")
            {
                inputFileOption,
                duplicateColumnsOption,
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
                decryptionKeyOption,
                outputFileOption,
            };

        // Thêm lệnh vào root
        rootCommand.AddCommand(indexCommand); // 2
        rootCommand.AddCommand(sortCommand); // 3
        rootCommand.AddCommand(searchCommand); // 4
        rootCommand.AddCommand(analyticsPublisherTitleCommand); // 5
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
        }, inputFileOption, indexAfterOption, outputFileOption);

        // 3
        sortCommand.SetHandler(async (input, sortColumns, output) =>
        {
            await Task.Run(() => ReadWriteCsvHelper.WriteCsvToFile(SapXep.SapXepDanhSach(ReadWriteCsvHelper.ReadCsvFromFile(input), sortColumns), output));
        }, inputFileOption, sortColumnsOption, outputFileOption);

        // 4
        searchCommand.SetHandler(async (input, searchKeyword, searchColumns, output) =>
        {
            await Task.Run(() => TimKiem.TimKiemTheoTuKhoa(ReadWriteCsvHelper.ReadCsvFromFile(input), searchKeyword, searchColumns));
        }, inputFileOption, searchKeywordOption, searchColumnsOption, outputFileOption);

        // 5
        analyticsPublisherTitleCommand.SetHandler(async (input) =>
        {
            await Task.Run(() => ThongKe.ThongKeTheoNhaXuatBanVaChuDe(ReadWriteCsvHelper.ReadCsvFromFile(input)));
        }, inputFileOption);

        // 6
        duplicateCommand.SetHandler(async (input, duplicateColumns, output) =>
        {
            await Task.Run(() => ReadWriteCsvHelper.WriteCsvToFile(TrungLap.LocTrungLap(ReadWriteCsvHelper.ReadCsvFromFile(input), duplicateColumns), output));
        }, inputFileOption, duplicateColumnsOption, outputFileOption);

        // 7
        encryptCommand.SetHandler(async (input, encryptionKey, output) =>
        {
            await Task.Run(() => ReadWriteCsvHelper.EncryptFile(input.FullName, output.FullName, encryptionKey));
        }, inputFileOption, encryptionKeyOption, outputFileOption);

        // 8
        decryptCommand.SetHandler(async (input, decryptionKey, output) =>
        {
            await Task.Run(() => ReadWriteCsvHelper.DecryptFile(input.FullName, output.FullName, decryptionKey));
        }, inputFileOption, decryptionKeyOption, outputFileOption);

        return await rootCommand.InvokeAsync(args);
    }
}