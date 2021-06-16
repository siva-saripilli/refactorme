namespace PaymentProcessor.Strategy
{
    public class InvoiceWithPayments : StrategyBase
    {
        public InvoiceWithPayments(Invoice invoice, Payment payment): 
            base(invoice, payment)
        {

        }

        public override string Handle()
        {
            if (Invoice.IsFullyPaid)
            {
                return ReturnMessage.ISFULLYPAID;
            }
            else if (Invoice.RemainingAmount == Payment.Amount)
            {
                Invoice.AddPayment(Payment);
                return ReturnMessage.FINALPAYMENTRECEIVED;
            }
            else if (Payment.Amount > Invoice.RemainingAmount)
            {
                return ReturnMessage.PAYMENTGREATERTHANPARTIALAMOUNT;
            }

            // Partial payment against invoice
            Invoice.AddPayment(Payment);
            return ReturnMessage.PARTIALPAYMENTRECEIVED;
        }
    }
}
