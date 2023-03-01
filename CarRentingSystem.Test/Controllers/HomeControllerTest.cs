namespace CarRentingSystem.Test.Controllers
{
    using CarRentingSystem.Controllers;
    using CarRentingSystem.Data.Models;
    using CarRentingSystem.Models.Home;
    using CarRentingSystem.Services.Cars;
    using CarRentingSystem.Services.Statistics;
    using CarRentingSystem.Test.Mocks;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using Xunit;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModel()
        {
            //Arrange
            var data = DatabaseMock.Instance;
            var mapper = MapperMock.Instance;

            var cars = Enumerable
                .Range(0, 10)
                .Select(i => new Car());

            data.Cars.AddRange(cars);
            data.Users.Add(new User());

            data.SaveChanges();

            var carService = new CarService(data, mapper);
            var statisticsService = new StatisticsService(data);

            var homeController = new HomeController(statisticsService, carService);

            //Act
            var result = homeController.Index();

            //Assert
            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            var indexViewModel = Assert.IsType<IndexViewModel>(model);

            Assert.Equal(3, indexViewModel.Cars.Count);
            Assert.Equal(3, indexViewModel.Cars.Count);
            Assert.Equal(1, indexViewModel.TotalUsers);
        }

        [Fact]
        public void ErrorShouldReturnView()
        {
            //Arrange
            var homeController = new HomeController(null, null);

            //Act
            var result = homeController.Error();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
