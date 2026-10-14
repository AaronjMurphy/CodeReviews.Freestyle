using System.Text;
namespace StoreManagerProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var app = new StoreApp();
            app.Run();
        }
    }
}
