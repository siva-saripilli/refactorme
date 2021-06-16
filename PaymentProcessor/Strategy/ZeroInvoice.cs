using System;

namespace PaymentProcessor.Strategy
{
    public class ZeroInvoice : StrategyBase
    {
        public ZeroInvoice(Invoice invoice, Payment payment): 
            base(invoice, payment)
        {

        }

        public override string Handle()
        {
            if (Invoice.HasPayments)
            {
                throw new InvalidOperationException(ReturnMessage.INVOICEINVALID);
            }

            return ReturnMessage.NOPAYMENTNEEDED;
        }
    }
}
