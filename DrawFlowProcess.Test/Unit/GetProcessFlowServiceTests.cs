using Moq;
using DrawFlowProcess.Application.DrawFlowProcess.GetProcessFlow;
using DrawFlowProcess.Domain.Repositories;
using DrawFlowProcess.Domain.Domain;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace DrawFlowProcess.Test.Unit
{
    [TestClass]
    public class GetProcessFlowHanclerTests
    {
        [TestMethod]
        public async Task Handle_ShouldThrowArgumentNullException_WhenQueryIsNull()
        {
            // Arrange
            var mockRepo = new Mock<IDrwaFlowProcessRepository>();
            var handler = new GetProcessFlowHancler(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await handler.Handle(null!, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handle_ShouldReturnResponse_WhenQueryIsValid()
        {
            // Arrange
            var mockRepo = new Mock<IDrwaFlowProcessRepository>();
            var processFlow = new ProcessFlow
            {
                Conditional = true,
                CountPages = 2,
                TypeFrom = "CurrentStep",
                TypeFront = new List<string> { "NextStep1", "NextStep2" },
                TypeBack = new List<string> { "BackStep1" },
                DataConfiguration = new JsonObject()
            };
            mockRepo.Setup(r => r.GetProcessFlow(
                It.IsAny<Guid>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<JsonDocument>(),
                It.IsAny<int>(),
                It.IsAny<bool>()))
                .ReturnsAsync(processFlow);

            var handler = new GetProcessFlowHancler(mockRepo.Object);
            var query = new GetProcessFlowQuery
            {
                NameType = "TypeA",
                AgreeentId = Guid.NewGuid(),
                WorkFlowId = 1,
                Conditional = JsonDocument.Parse("{}"),
                TypeProcess = 0,
                Back = false
            };

            // Act
            var response = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.CountPages);
            Assert.AreEqual("CurrentStep", response.CurrentStep);
            Assert.AreEqual("NextStep1", response.NextStep?[0]);
            Assert.AreEqual("BackStep1", response.BackStep?[0]);
            Assert.IsNotNull(response.DataConfiguration);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnNull_WhenProcessNotFound()
        {
            // Arrange
            var mockRepo = new Mock<IDrwaFlowProcessRepository>();
            mockRepo.Setup(r => r.GetProcessFlow(
                It.IsAny<Guid>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<JsonDocument>(),
                It.IsAny<int>(),
                It.IsAny<bool>()))
                .ReturnsAsync((ProcessFlow)null!);

            var handler = new GetProcessFlowHancler(mockRepo.Object);
            var query = new GetProcessFlowQuery
            {
                NameType = "TypeA",
                AgreeentId = Guid.NewGuid(),
                WorkFlowId = 99,
                Conditional = JsonDocument.Parse("{}"),
                TypeProcess = 0,
                Back = false
            };

            // Act
            var response = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNull(response);
        }

        [TestMethod]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrows()
        {
            // Arrange
            var mockRepo = new Mock<IDrwaFlowProcessRepository>();
            mockRepo.Setup(r => r.GetProcessFlow(
                It.IsAny<Guid>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<JsonDocument>(),
                It.IsAny<int>(),
                It.IsAny<bool>()))
                .ThrowsAsync(new Exception("DB error"));

            var handler = new GetProcessFlowHancler(mockRepo.Object);
            var query = new GetProcessFlowQuery
            {
                NameType = "TypeA",
                AgreeentId = Guid.NewGuid(),
                WorkFlowId = 1,
                Conditional = JsonDocument.Parse("{}"),
                TypeProcess = 0,
                Back = false
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () =>
                await handler.Handle(query, CancellationToken.None));
        }
    }
}
