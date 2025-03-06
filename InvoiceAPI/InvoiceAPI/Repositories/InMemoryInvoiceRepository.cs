using System.Collections.Generic;
using System.Linq;
using InvoiceAPI.Models;

namespace InvoiceAPI.Repositories
{
    public class InMemoryInvoiceRepository : IInvoiceRepository
    {
        private readonly List<Invoice> _invoices = new();

        public Invoice CreateInvoice(Invoice invoice)
        {
            _invoices.Add(invoice);
            return invoice;
        }

        public List<Invoice> GetInvoices() => _invoices;

        public Invoice? GetInvoiceById(string id) =>
            _invoices.FirstOrDefault(inv => inv.Id == id);

        public void UpdateInvoice(Invoice invoice)
        {
            var index = _invoices.FindIndex(inv => inv.Id == invoice.Id);
            if (index != -1)
            {
                _invoices[index] = invoice;
            }
        }
    }
}
