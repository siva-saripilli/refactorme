using System;
using System.Linq;

namespace PaymentProcessor
{
	public class InvoicePaymentProcessor
	{
		private readonly InvoiceRepository _invoiceRepository;

		public InvoicePaymentProcessor( InvoiceRepository invoiceRepository )
		{
			_invoiceRepository = invoiceRepository;
		}

		public string ProcessPayment( Payment payment )
		{
			var inv = _invoiceRepository.GetInvoice( payment.Reference );

			if ( inv == null )
			{
				throw new InvalidOperationException( "There is no invoice matching this payment" );
			}
			else
			{
				if ( inv.Amount == 0 )
				{
					if ( inv.Payments == null || !inv.Payments.Any( ) )
					{
						return "no payment needed";
					}
					else
					{
						throw new InvalidOperationException( "The invoice is in an invalid state, it has an amount of 0 and it has payments." );
					}
				}
				else
				{
					if ( inv.Payments != null && inv.Payments.Any( ) )
					{
						if ( inv.Payments.Sum( x => x.Amount ) != 0 && inv.Amount == inv.Payments.Sum( x => x.Amount ) )
						{
							return "invoice was already fully paid";
						}
						else if ( inv.Payments.Sum( x => x.Amount ) != 0 && payment.Amount > ( inv.Amount - inv.AmountPaid ) )
						{
							return "the payment is greater than the partial amount remaining";
						}
						else
						{
							if ( ( inv.Amount - inv.AmountPaid ) == payment.Amount )
							{
								inv.AmountPaid += payment.Amount;
								inv.Payments.Add( payment );
								return "final partial payment received, invoice is now fully paid";
							}
							else
							{
								inv.AmountPaid += payment.Amount;
								inv.Payments.Add( payment );
								return "another partial payment received, still not fully paid";
							}
						}
					}
					else
					{
						if ( payment.Amount > inv.Amount )
						{
							return "the payment is greater than the invoice amount";
						}
						else if ( inv.Amount == payment.Amount )
						{
							inv.AmountPaid = payment.Amount;
							inv.Payments.Add( payment );
							return "invoice is now fully paid";
						}
						else
						{
							inv.AmountPaid = payment.Amount;
							inv.Payments.Add( payment );
							return "invoice is now partially paid";
						}
					}
				}
			}
		}
	}
}