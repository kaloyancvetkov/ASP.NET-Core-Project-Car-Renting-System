namespace CarRentingSystem.Controllers
{
    using AutoMapper;
    using CarRentingSystem.Models.Cars;
    using CarRentingSystem.Services.Cars;
    using CarRentingSystem.Services.Dealers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using static Infrastructure.ClaimsPrincipalExtensions;

    public class CarsController : Controller
    {
        private readonly ICarService cars;
        private readonly IDealerService dealers;
        private readonly IMapper mapper;

        public CarsController(
            ICarService cars,
            IDealerService dealers, IMapper mapper)
        {
            this.cars = cars;
            this.dealers = dealers;
            this.mapper = mapper;
        }

        public IActionResult All([FromQuery]AllCarsQueryModel query)
        {
            var queryResult = this.cars.All(
                query.Brand,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllCarsQueryModel.CarsPerPage);

            var carBrands = this.cars.AllBrands();

            query.Brands = carBrands;
            query.TotalCars = queryResult.TotalCars;
            query.Cars = queryResult.Cars;

            return View(query);
        }

        [Authorize]
        public IActionResult Mine()
        {
            var myCars = this.cars.ByUser(this.User.Id());

            return View(myCars);
        }

        [Authorize]
        public IActionResult Add() 
        {
            if (!dealers.IsDealer(User.Id()))
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            return View(new CarFormModel
            {
                Categories = this.cars.AllCategories()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(CarFormModel car)
        {
            var dealerId = this.dealers.IdByUser(User.Id());

            if (dealerId == 0)
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            if (!dealers.IsDealer(User.Id()))
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            if (!cars.CategoryExists(car.CategoryId))
            {
                this.ModelState.AddModelError(nameof(car.CategoryId), "Category doesn't exist.");
            }

            if (!ModelState.IsValid)
            {
                car.Categories = this.cars.AllCategories();

                return View(car);
            }

            this.cars.Create(car.Brand,
                car.Model,
                car.Description,
                car.ImageUrl,
                car.Year,
                car.CategoryId,
                dealerId);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.Id();

            if (!dealers.IsDealer(userId) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            var car = this.cars.Details(id);

            if (car.UserId != userId && !User.IsAdmin())
            {
                return Unauthorized();
            }

            var carForm = this.mapper.Map<CarFormModel>(car);

            carForm.Categories = this.cars.AllCategories();

            return View(carForm);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, CarFormModel car)
        {
            var dealerId = this.dealers.IdByUser(User.Id());

            if (dealerId == 0 && !User.IsAdmin())
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            if (!cars.CategoryExists(car.CategoryId))
            {
                this.ModelState.AddModelError(nameof(car.CategoryId), "Category doesn't exist.");
            }

            if (!ModelState.IsValid)
            {
                car.Categories = this.cars.AllCategories();

                return View(car);
            }

            if (!this.cars.isByDealer(id, dealerId) && !User.IsAdmin())
            {
                return BadRequest();
            }

            this.cars.Edit(
                id,
                car.Brand,
                car.Model,
                car.Description,
                car.ImageUrl,
                car.Year,
                car.CategoryId);

            return RedirectToAction(nameof(All));
        }
    }
}
