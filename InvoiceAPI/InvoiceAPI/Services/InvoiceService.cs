using System;
using System.Collections.Generic;
using System.Linq;
using InvoiceAPI.Models;
using InvoiceAPI.Repositories;

namespace InvoiceAPI.Services
{
    public class InvoiceService
    {
        private readonly IInvoiceRepository _repository;

        public InvoiceService(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        public Invoice CreateInvoice(decimal amount, DateTime dueDate)
        {
            var invoice = new Invoice { Amount = amount, DueDate = dueDate };
            return _repository.CreateInvoice(invoice);
        }

        public List<Invoice> GetInvoices() => _repository.GetInvoices();

        public bool PayInvoice(string id, decimal amount)
        {
            var invoice = _repository.GetInvoiceById(id);
            if (invoice == null || invoice.Status != "pending") return false;

            invoice.PaidAmount += amount;
            if (invoice.PaidAmount >= invoice.Amount)
            {
                invoice.Status = "paid";
            }
            _repository.UpdateInvoice(invoice);
            return true;
        }

        public void ProcessOverdueInvoices(decimal lateFee, int overdueDays)
        {
            var today = DateTime.UtcNow;
            var invoices = _repository.GetInvoices()
                                      .Where(inv => inv.Status == "pending" && inv.DueDate.AddDays(overdueDays) < today)
                                      .ToList();

            foreach (var invoice in invoices)
            {
                if (invoice.PaidAmount > 0)
                {
                    var newAmount = invoice.Amount - invoice.PaidAmount + lateFee;
                    CreateInvoice(newAmount, today.AddDays(overdueDays));
                    invoice.Status = "paid";
                }
                else
                {
                    CreateInvoice(invoice.Amount + lateFee, today.AddDays(overdueDays));
                    invoice.Status = "void";
                }

                _repository.UpdateInvoice(invoice);
            }
        }
    }
}
