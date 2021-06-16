namespace PaymentProcessor
{
    public interface IInvoicePaymentProcessor
    {
        IInvoiceRepository InvoiceRepository { get; set; }
        string ProcessPayment(Payment payment);
    }
}
