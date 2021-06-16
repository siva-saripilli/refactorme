namespace PaymentProcessor
{
    public interface IInvoicePaymentProcessor
    {
        string ProcessPayment(Payment payment);
    }
}
