using System.Runtime.CompilerServices;
using System.CommandLine;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
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

        // 4: Tìm kiếm sách theo từ khóa
        var searchKeywordOption = new Option<string[]>(
            name: "--searchKeyword",
            description: "Từ khóa tìm kiếm");
        var searchColumnOption = new Option<string[]>(
            name: "--searchColumn",
            description: "Tên cột để tìm kiếm (tách nhau bởi dấu phẩy (,))");

        // 5: Thống kê số lượng sách và chủ đề theo nhà xuất bản



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

        // 1
        var indexCommand = new Command("index", "Tạo cột index cho tệp tin sau cột được cho.")
            {
                inputFileOption,
                indexAfterOption,
                outputFileOption,
            };
        var sortCommand = new Command("sort", "Sắp xếp tệp tin theo cột được cho.")
            {
                inputFileOption,
                sortColumnOption,
                outputFileOption,
            };

        var encryptCommand = new Command("encrypt", "Mã hóa tệp tin.")
            {
                inputFileOption,
                encryptionKeyOption,
                outputFileOption,
            };
        var decryptCommand = new Command("decrypt", "Giải mã hóa tệp tin.")
            {
                inputFileOption,
                encryptionKeyOption,
                outputFileOption,
            };
        rootCommand.AddCommand(indexCommand);
        rootCommand.AddCommand(sortCommand);
        rootCommand.AddCommand(encryptCommand);
        rootCommand.AddCommand(decryptCommand);

        indexCommand.SetHandler(async (input, indexAfter, output) =>
        {
            // Cho lệnh ở dưới
            //await 
        }, inputFileOption, indexAfterOption, outputFileOption);

        return await rootCommand.InvokeAsync(args);
    }
}