using RestaurantOrderManager.Controllers;

namespace RestaurantOrderManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var app = new RestaurantController();
            app.Run();
        }
    }
}
