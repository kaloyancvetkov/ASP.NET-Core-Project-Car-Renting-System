namespace CarRentingSystem.Test.Mocks
{
    using CarRentingSystem.Data;
    using Microsoft.EntityFrameworkCore;
    using System;

    public static class DatabaseMock
    {
        public static CarRentingDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<CarRentingDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString(), b => b.EnableNullChecks(false))
                    .Options;

                
                

                return new CarRentingDbContext(dbContextOptions);
            }
        }
    }
}
