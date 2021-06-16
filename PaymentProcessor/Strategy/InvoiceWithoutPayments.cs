namespace PaymentProcessor.Strategy
{
    public class InvoiceWithoutPayments: StrategyBase
    {
        public InvoiceWithoutPayments(Invoice invoice, Payment payment):
            base(invoice, payment)
        {

        }

        public override string Handle()
        {
            if (Payment.Amount > Invoice.Amount)
            {
                return ReturnMessage.PAYMETGREATERTHANINVOICEAMOUNT;
            }
            else if (Invoice.Amount == Payment.Amount)
            {
                Invoice.AddPayment(Payment);
                return ReturnMessage.INVOICEFULLYPAID;
            }

            // Partial payment against invoice
            Invoice.AddPayment(Payment);
            return ReturnMessage.INVOICEPARTIALLYPAID;
        }
    }
}
