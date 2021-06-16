using System.Collections.Generic;
using System.Linq;

namespace PaymentProcessor
{
	public class Invoice
	{
		public decimal Amount { get; set; }
		public decimal AmountPaid { get; set; }
		public IList<Payment> Payments { get; set; }

		public decimal PaymentsTotalAmount
        {
            get
            {
                return HasPayments ? Payments.Sum(p => p.Amount) : 0;
            }
        }

        public decimal RemainingAmount
        {
            get
            {
                return HasPayments ? Amount - PaymentsTotalAmount : 0;
            }			
        }

        public bool HasPayments
        {
            get
            {
                return Payments != null && Payments.Any();
            }
        }

        public bool IsFullyPaid
        {
            get
            {
                return RemainingAmount is 0;
            }            
        }

        public void AddPayment(Payment newPayment)
        {
            AmountPaid += newPayment.Amount;
            Payments.Add(newPayment);
        }
	}
}