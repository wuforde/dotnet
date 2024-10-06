using Microsoft.AspNetCore.Builder;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/test",() => "hello world");
        app.Run();
    }
}