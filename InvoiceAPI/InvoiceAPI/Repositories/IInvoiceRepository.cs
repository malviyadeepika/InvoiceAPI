using System.Collections.Generic;
using InvoiceAPI.Models;

namespace InvoiceAPI.Repositories
{
    public interface IInvoiceRepository
    {
        Invoice CreateInvoice(Invoice invoice);
        List<Invoice> GetInvoices();
        Invoice? GetInvoiceById(string id);
        void UpdateInvoice(Invoice invoice);
    }
}
