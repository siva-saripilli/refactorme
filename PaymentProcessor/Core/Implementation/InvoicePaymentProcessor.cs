using PaymentProcessor.Strategy;
using System;

namespace PaymentProcessor
{
    public class InvoicePaymentProcessor: IInvoicePaymentProcessor
	{
		private readonly IInvoiceRepository _invoiceRepository;

        public InvoicePaymentProcessor(IInvoiceRepository invoiceRepository)
		{
			_invoiceRepository = invoiceRepository;
		}

        public string ProcessPayment(Payment newPayment)
		{
            var invoice = _invoiceRepository.GetInvoice(newPayment.Reference);

            if (invoice == null)
			{
                throw new InvalidOperationException(ReturnMessage.NOINVOICE);
			}

            if (invoice.Amount == 0)
            {
                return new ZeroInvoice(invoice, newPayment).Handle();
            }
            else
            {
                return new NonZeroInvoice(invoice, newPayment).Handle();
            }
        }
    }
}