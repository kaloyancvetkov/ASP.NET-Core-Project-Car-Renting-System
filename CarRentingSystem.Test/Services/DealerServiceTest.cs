namespace CarRentingSystem.Test.Services
{
    using CarRentingSystem.Data;
    using CarRentingSystem.Data.Models;
    using CarRentingSystem.Services.Dealers;
    using CarRentingSystem.Test.Mocks;
    using Xunit;

    public class DealerServiceTest
    {
        private const string userId = "TestUserId";

        [Fact]
        public void IsDealerShouldReturnTrueWhenUserIsDealer()
        {
            //Arrange 
            var dealerService = this.GetDealerService();

            //Act
            var result = dealerService.IsDealer(userId);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDealerShouldReturnFalseWhenUserIsDealer()
        {
            //Arrange
            var dealerService = this.GetDealerService();

            //Act
            var result = dealerService.IsDealer("blabla");

            //Assert
            Assert.False(result);
        }

        private IDealerService GetDealerService()
        {
            var data = DatabaseMock.Instance;

            data.Dealers.Add(new Dealer { UserId = userId });
            data.SaveChanges();
            
            return new DealerService(data); 
        }
    }
}
