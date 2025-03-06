using System;
using Microsoft.AspNetCore.Mvc;
using InvoiceAPI.Models;
using InvoiceAPI.Services;

namespace InvoiceAPI.Controllers
{
    [ApiController]
    [Route("invoices")]
    public class InvoicesController : ControllerBase
    {
        private readonly InvoiceService _service;

        public InvoicesController(InvoiceService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult CreateInvoice([FromBody] Invoice invoice)
        {
            var newInvoice = _service.CreateInvoice(invoice.Amount, invoice.DueDate);
            return CreatedAtAction(nameof(CreateInvoice), new { id = newInvoice.Id }, newInvoice);
        }

        [HttpGet]
        public IActionResult GetInvoices()
        {
            return Ok(_service.GetInvoices());
        }

        [HttpPost("{id}/payments")]
        public IActionResult PayInvoice(string id, [FromBody] PaymentRequest payment)
        {
            var success = _service.PayInvoice(id, payment.Amount);
            return success ? Ok() : NotFound();
        }

        [HttpPost("process-overdue")]
        public IActionResult ProcessOverdueInvoices([FromBody] ProcessOverdueRequest request)
        {
            _service.ProcessOverdueInvoices(request.LateFee, request.OverdueDays);
            return Ok();
        }
    }
}
