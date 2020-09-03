using dropped_kerb_service.Controllers;
using dropped_kerb_service.Models;
using dropped_kerb_service.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StockportGovUK.AspNetCore.Availability.Managers;
using Xunit;

namespace dropped_kerb_service.Controllers
{
    public class HomeControllerTests
    {
        private readonly HomeController _homeController;
        private readonly Mock<IDroppedKerbService> _mockDroppedKerbService = new Mock<IDroppedKerbService>();

        public HomeControllerTests()
        {
            _homeController = new HomeController(_mockDroppedKerbService.Object);
        }

        [Fact]
        public async void Get_ShouldReturnOK()
        {
            _mockDroppedKerbService
                .Setup(_ => _.CreateCase(It.IsAny<DroppedKerbRequest>()))
                .ReturnsAsync("test");

            IActionResult result = await _homeController.Post(null);

            _mockDroppedKerbService
                .Verify(_ => _.CreateCase(null), Times.Once);
        }

        [Fact]
        public async void Post_ShouldReturnOK()
        {
            _mockDroppedKerbService
                   .Setup(_ => _.CreateCase(It.IsAny<DroppedKerbRequest>()))
                   .ReturnsAsync("test");

            IActionResult result = await _homeController.Post(null);

            Assert.Equal("OkObjectResult", result.GetType().Name);
        }
    }
}
