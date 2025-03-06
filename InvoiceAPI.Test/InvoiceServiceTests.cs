using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using InvoiceAPI.Models;
using InvoiceAPI.Repositories;
using InvoiceAPI.Services;

namespace InvoiceAPI.Tests.Services
{
    public class InvoiceServiceTests
    {
        private readonly Mock<IInvoiceRepository> _mockRepo;
        private readonly InvoiceService _service;

        public InvoiceServiceTests()
        {
            _mockRepo = new Mock<IInvoiceRepository>(); // ✅ Mocking the repository
            _service = new InvoiceService(_mockRepo.Object); // ✅ Injecting mock repo
        }

        // ✅ Test CreateInvoice
        [Fact]
        public void CreateInvoice_ShouldReturnInvoice_WhenValidDataProvided()
        {
            // Arrange
            var dueDate = new DateTime(2024, 04, 10);
            var invoice = new Invoice { Amount = 500, DueDate = dueDate };

            _mockRepo.Setup(repo => repo.CreateInvoice(It.IsAny<Invoice>())).Returns(invoice);

            // Act
            var result = _service.CreateInvoice(500, dueDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.Amount);
            Assert.Equal(dueDate, result.DueDate);
        }

        // ✅ Test GetInvoices
        [Fact]
        public void GetInvoices_ShouldReturnListOfInvoices()
        {
            // Arrange
            var invoices = new List<Invoice>
            {
                new Invoice { Amount = 100, DueDate = DateTime.UtcNow },
                new Invoice { Amount = 200, DueDate = DateTime.UtcNow.AddDays(10) }
            };

            _mockRepo.Setup(repo => repo.GetInvoices()).Returns(invoices);

            // Act
            var result = _service.GetInvoices();

            // Assert
            Assert.Equal(2, result.Count);
        }

        // ✅ Test PayInvoice - Successful Payment
        [Fact]
        public void PayInvoice_ShouldReturnTrue_WhenInvoiceIsPaid()
        {
            // Arrange
            var invoice = new Invoice { Id = "123", Amount = 500, PaidAmount = 100, Status = "pending" };

            _mockRepo.Setup(repo => repo.GetInvoiceById("123")).Returns(invoice);
            _mockRepo.Setup(repo => repo.UpdateInvoice(It.IsAny<Invoice>()));

            // Act
            var result = _service.PayInvoice("123", 400);

            // Assert
            Assert.True(result);
            Assert.Equal(500, invoice.PaidAmount);
            Assert.Equal("paid", invoice.Status);
        }

        // ✅ Test PayInvoice - Fail When Status is Not Pending
        [Fact]
        public void PayInvoice_ShouldReturnFalse_WhenInvoiceIsNotPending()
        {
            // Arrange
            var invoice = new Invoice { Id = "456", Amount = 300, PaidAmount = 0, Status = "paid" };

            _mockRepo.Setup(repo => repo.GetInvoiceById("456")).Returns(invoice);

            // Act
            var result = _service.PayInvoice("456", 100);

            // Assert
            Assert.False(result);
        }

        // ✅ Test ProcessOverdueInvoices
        [Fact]
        public void ProcessOverdueInvoices_ShouldCreateNewInvoices()
        {
            // Arrange
            var today = DateTime.UtcNow;
            var overdueInvoices = new List<Invoice>
            {
                new Invoice { Id = "1", Amount = 200, DueDate = today.AddDays(-40), PaidAmount = 50, Status = "pending" },
                new Invoice { Id = "2", Amount = 150, DueDate = today.AddDays(-50), PaidAmount = 0, Status = "pending" }
            };

            _mockRepo.Setup(repo => repo.GetInvoices()).Returns(overdueInvoices);
            _mockRepo.Setup(repo => repo.CreateInvoice(It.IsAny<Invoice>())).Returns(new Invoice());
            _mockRepo.Setup(repo => repo.UpdateInvoice(It.IsAny<Invoice>()));

            // Act
            _service.ProcessOverdueInvoices(10, 30); // Late fee = 10, overdueDays = 30

            // Assert
            Assert.Equal("paid", overdueInvoices[0].Status); // ✅ Partially paid invoice marked as paid
            Assert.Equal("void", overdueInvoices[1].Status); // ✅ Unpaid invoice marked as void
            _mockRepo.Verify(repo => repo.CreateInvoice(It.IsAny<Invoice>()), Times.Exactly(2)); // ✅ Ensures 2 new invoices are created
            _mockRepo.Verify(repo => repo.UpdateInvoice(It.IsAny<Invoice>()), Times.Exactly(2)); // ✅ Ensures 2 invoices were updated
        }
    }
}
