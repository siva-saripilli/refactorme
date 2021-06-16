namespace PaymentProcessor
{
    public interface IInvoiceRepository
    {
        Invoice Invoice { get; set; }
        Invoice GetInvoice(string reference);
    }
}
