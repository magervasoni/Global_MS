using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using web_app_domain;
using web_app_performance.Controllers;
using web_app_repository;

namespace Test
{
    public class ConsumptionControllerTest
    {
        private readonly Mock<IConsumoRepository> _consumoRepositoryMock;
        private readonly ConsumptionController _controller;

        public ConsumptionControllerTest()
        {
            _consumoRepositoryMock = new Mock<IConsumoRepository>();
            _controller = new ConsumptionController(_consumoRepositoryMock.Object);
        }

        [Fact]
        public async Task Get_ListarConsumptionsOk()
        {
            // Arrange
            var consumptions = new List<Consumo>()
            {
                new Consumo()
                {
                    Id = 1,
                    RegistroData = "Consumo A",
                    ValorConsumo = "100.0"
                }
            };
            _consumoRepositoryMock.Setup(r => r.ListConsumptions()).ReturnsAsync(consumptions);

            // Act
            var result = await _controller.GetConsumption();

            // Asserts
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(JsonConvert.SerializeObject(consumptions), JsonConvert.SerializeObject(okResult.Value));
        }

        [Fact]
        public async Task Get_ListarConsumptionsRetornaNotFound()
        {
            // Arrange
            _consumoRepositoryMock.Setup(u => u.ListConsumptions()).ReturnsAsync((IEnumerable<Consumo>)null);

            // Act
            var result = await _controller.GetConsumption();

            // Asserts
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Post_SalvarConsumption()
        {
            // Arrange
            var consumption = new Consumo()
            {
                Id = 1,
                RegistroData = "Consumo  BB",
                ValorConsumo = "2200.0"
            };
            _consumoRepositoryMock.Setup(u => u.SaveConsumption(It.IsAny<Consumo>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(consumption);

            // Asserts
            _consumoRepositoryMock
                .Verify(u => u.SaveConsumption(It.IsAny<Consumo>()), Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
