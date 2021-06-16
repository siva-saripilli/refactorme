namespace PaymentProcessor 
{
	public class InvoiceRepository : IInvoiceRepository
	{
		public Invoice Invoice { get; set; }

		public InvoiceRepository() { }

		public InvoiceRepository( Invoice invoice )
		{
			this.Invoice = invoice;
		}

		public Invoice GetInvoice( string reference )
		{
			return this.Invoice;
		}
	}
}