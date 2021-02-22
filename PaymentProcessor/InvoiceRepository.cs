namespace PaymentProcessor {
	public class InvoiceRepository
	{
		private Invoice _invoice;

		public InvoiceRepository( Invoice invoice )
		{
			_invoice = invoice;
		}

		public Invoice GetInvoice( string reference )
		{
			return _invoice;
		}
	}
}