using PaymentProcessor.Strategy;
using System;

namespace PaymentProcessor
{
    public class InvoicePaymentProcessor: IInvoicePaymentProcessor
	{
		public IInvoiceRepository InvoiceRepository { get; set; }

        public InvoicePaymentProcessor() { }

        public InvoicePaymentProcessor(IInvoiceRepository invoiceRepository)
        {
            InvoiceRepository = invoiceRepository;
        }

        public string ProcessPayment(Payment newPayment)
        {
            var invoice = InvoiceRepository.GetInvoice(newPayment.Reference);

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