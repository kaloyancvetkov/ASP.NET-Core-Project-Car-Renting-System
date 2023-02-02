using System.ComponentModel.DataAnnotations;

namespace CarRentingSystem.Models.Cars
{
    public class AllCarsQueryModel
    {
        public const int CarsPerPage = 3;

        public string Brand { get; init; }

        public IEnumerable<string> Brands { get; set; }

        [Display(Name = "Search by text:")]
        public string SearchTerm { get; init; }

        public CarSorting Sorting { get; init; }

        public int TotalCars { get; set; }

        public int CurrentPage { get; init; } = 1;

        public IEnumerable<CarListingViewModel> Cars { get; set; }
    }
}
