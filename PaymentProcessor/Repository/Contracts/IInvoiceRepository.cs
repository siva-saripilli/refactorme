namespace PaymentProcessor
{
    public interface IInvoiceRepository
    {
        Invoice GetInvoice(string reference);
    }
}
