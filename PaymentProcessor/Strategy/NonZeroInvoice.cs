namespace PaymentProcessor.Strategy
{
    public class NonZeroInvoice : StrategyBase
    {
        public NonZeroInvoice(Invoice invoice, Payment payment) :
            base(invoice, payment)
        {

        }

        public override string Handle()
        {
            if (Invoice.HasPayments)
            {
                return new InvoiceWithPayments(Invoice, Payment).Handle();
            }

            return new InvoiceWithoutPayments(Invoice, Payment).Handle();
        }
    }
}
