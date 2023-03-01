namespace CarRentingSystem.Controllers
{
    using AutoMapper;
    using CarRentingSystem.Models.Home;
    using CarRentingSystem.Services.Cars;
    using CarRentingSystem.Services.Statistics;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly ICarService cars;
        private readonly IStatisticsService statistics;

        public HomeController(
            IStatisticsService statistics,
            ICarService cars)
        {
            this.statistics = statistics;
            this.cars = cars;
        }

        public IActionResult Index()
        {
            var latestCars = cars.Latest();

            var totalStatistics = this.statistics.Total();    

            return View(new IndexViewModel
            {
                TotalCars = totalStatistics.TotalCars,
                TotalUsers = totalStatistics.TotalUsers,
                Cars = latestCars.ToList()
            });
        }

        public IActionResult Error() => View();
    }
}