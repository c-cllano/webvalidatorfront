using Moq;
using DrawFlowProcess.Infrastructure.Repositories;
using DrawFlowProcess.Domain.Interface;
using DrawFlowProcess.Domain.Repositories;
using System.Text.Json;
using DrawFlowProcess.Domain.Domain;

namespace DrawFlowProcess.Test.Unit
{
    [TestClass]
    public class DrawFlowProcessRepositoryTests
    {
        private Mock<IMongoContext> _mockContext = default!;
        private Mock<IClientInfoRepository> _mockClientInfo = default!;
        private DrawFlowProcessRepository _repository = default!;

        [TestInitialize]
        public void Setup()
        {
            _mockContext = new Mock<IMongoContext>();
            _mockClientInfo = new Mock<IClientInfoRepository>();
            _repository = new DrawFlowProcessRepository(_mockContext.Object, _mockClientInfo.Object);
        }

        [TestMethod]
        public void GetJsonConvert_ShouldReturnExportJson_WhenJsonIsValid()
        {
            // Arrange
            var jsonDocument = JsonDocument.Parse("{\"AgreementID\":\"00000000-0000-0000-0000-000000000000\",\"WorkflowID\":1}");

            // Act
            var result = _repository.GetJsonConvert(jsonDocument);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.WorkflowID);
        }

        [TestMethod]
        public async Task SaveJsonConvert_ShouldReturnTrue_WhenExportJsonIsValid()
        {
            // Arrange
            var exportJson = new ExportJson
            {
                AgreementID = Guid.NewGuid(),
                WorkflowID = 1,
                Nodos = new System.Collections.Generic.List<Nodo>()
            };

            // Act
            var result = await _repository.SaveJsonConvert(exportJson);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task GetJsonByIds_ShouldReturnExportJson_WhenIdsAreValid()
        {
            // Arrange
            var agreementId = Guid.NewGuid();
            int workflowId = 1;

            // Act
            var result = await _repository.GetJsonByIds(agreementId, workflowId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(agreementId, result.AgreementID);
            Assert.AreEqual(workflowId, result.WorkflowID);
        }

        [TestMethod]
        public async Task GetProcessFlow_ShouldReturnProcessFlow_WhenParamsAreValid()
        {
            // Arrange
            var agreementId = Guid.NewGuid();
            int workflowId = 1;
            string typeCurrent = "Test";

            // Act
            var result = await _repository.GetProcessFlow(agreementId, workflowId, typeCurrent);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeCurrent, result.TypeFrom);
        }

        [TestMethod]
        public async Task GetJsonPages_ShouldReturnJsonPages_WhenParamsAreValid()
        {
            // Arrange
            var agreementId = Guid.NewGuid();
            int workflowId = 1;

            // Act
            var result = await _repository.GetJsonPages(agreementId, workflowId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.CountPages >= 0);
        }
    }
}
