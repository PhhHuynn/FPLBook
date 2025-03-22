using System.Runtime.CompilerServices;
using System.CommandLine;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        // --inputFile: Lấy vị trí file và đọc để xử lý
        var fileOption = new Option<FileInfo?>(
            name: "--inputFile",
            description: "Lấy vị trí file và đọc để xử lý.");

        var rootCommand = new RootCommand("CLI xử lý file");
        rootCommand.AddOption(fileOption);

        rootCommand.SetHandler((file) =>
        {
            
            //ReadFile(file!);
        },
            fileOption);

        return await rootCommand.InvokeAsync(args);
    }
}