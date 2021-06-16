using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using PaymentProcessor;

namespace PaymentProcessorTests
{
	[TestFixture]
	public class InvoicePaymentProcessorTests
	{
		private IInvoiceRepository _invoiceRepository;
		private IInvoicePaymentProcessor _paymentProcessor;

		private void Initialize(Invoice invoice)
        {
			var host = Host.CreateDefaultBuilder()
				.ConfigureServices((hostContext, services) => {
					services.AddTransient<IInvoicePaymentProcessor, InvoicePaymentProcessor>();
					services.AddTransient<IInvoiceRepository, InvoiceRepository>();
				}).Build();

			var scope = host.Services.CreateScope();

			_invoiceRepository = scope.ServiceProvider.GetService<IInvoiceRepository>();
			_invoiceRepository.Invoice = invoice;
			_paymentProcessor = scope.ServiceProvider.GetService<IInvoicePaymentProcessor>();
			_paymentProcessor.InvoiceRepository = _invoiceRepository;			
        }

		[Test]
        public void ProcessPayment_Should_ThrowException_When_NoInoiceFoundForPaymentReference()
		{
			Initialize(null);

            var payment = new Payment();
			var failureMessage = "";

			try
			{
                var result = _paymentProcessor.ProcessPayment(payment);
			}
            catch (InvalidOperationException e)
			{
				failureMessage = e.Message;
			}

			Assert.AreEqual("There is no invoice matching this payment", failureMessage);
		}

		[Test]
        public void ProcessPayment_Should_ReturnFailureMessage_When_NoPaymentNeeded()
		{
			Initialize(new Invoice
			{
				Amount = 0,
				AmountPaid = 0,
				Payments = null
			});

            var payment = new Payment();
            var result = _paymentProcessor.ProcessPayment(payment);

			Assert.AreEqual("no payment needed", result);
		}

		[Test]
        public void ProcessPayment_Should_ReturnFailureMessage_When_InvoiceAlreadyFullyPaid()
		{
            Initialize(new Invoice
			{
				Amount = 10,
				AmountPaid = 10,
				Payments = new List<Payment>
				{
					new Payment
					{
						Amount = 10
					}
				}
			});

            var payment = new Payment();
            var result = _paymentProcessor.ProcessPayment(payment);

			Assert.AreEqual("invoice was already fully paid", result);
		}

		[Test]
        public void ProcessPayment_Should_ReturnFailureMessage_When_PartialPaymentExistsAndAmountPaidExceedsAmountDue()
		{
			Initialize(new Invoice
			{
				Amount = 10,
				AmountPaid = 5,
				Payments = new List<Payment>
				{
					new Payment
					{
						Amount = 5
					}
				}
			});

			var payment = new Payment
			{
				Amount = 6
			};
            var result = _paymentProcessor.ProcessPayment(payment);

			Assert.AreEqual("the payment is greater than the partial amount remaining", result);
		}

		[Test]
        public void ProcessPayment_Should_ReturnFailureMessage_When_NoPartialPaymentExistsAndAmountPaidExceedsInvoiceAmount()
		{
			Initialize(new Invoice
			{
				Amount = 5,
				AmountPaid = 0,
				Payments = new List<Payment>()
			});

			var payment = new Payment
			{
				Amount = 6
			};
            var result = _paymentProcessor.ProcessPayment(payment);

			Assert.AreEqual("the payment is greater than the invoice amount", result);
		}

		[Test]
        public void ProcessPayment_Should_ReturnFullyPaidMessage_When_PartialPaymentExistsAndAmountPaidEqualsAmountDue()
		{
			Initialize(new Invoice
			{
				Amount = 10,
				AmountPaid = 5,
				Payments = new List<Payment>
				{
					new Payment
					{
						Amount = 5
					}
				}
			});

			var payment = new Payment
			{
				Amount = 5
			};
            var result = _paymentProcessor.ProcessPayment(payment);

			Assert.AreEqual("final partial payment received, invoice is now fully paid", result);
		}

		[Test]
        public void ProcessPayment_Should_ReturnFullyPaidMessage_When_NoPartialPaymentExistsAndAmountPaidEqualsInvoiceAmount()
		{
			Initialize(new Invoice
			{
				Amount = 10,
				AmountPaid = 0,
				Payments = new List<Payment> { new Payment { Amount = 10 } }
			});

			var payment = new Payment
			{
				Amount = 10
			};
			var result = _paymentProcessor.ProcessPayment(payment);

			Assert.AreEqual("invoice was already fully paid", result);
		}

		[Test]
        public void ProcessPayment_Should_ReturnPartiallyPaidMessage_When_PartialPaymentExistsAndAmountPaidIsLessThanAmountDue()
		{
			Initialize(new Invoice
			{
				Amount = 10,
				AmountPaid = 5,
				Payments = new List<Payment>
				{
					new Payment
					{
						Amount = 5
					}
				}
			});

			var payment = new Payment
			{
				Amount = 1
			};
            var result = _paymentProcessor.ProcessPayment(payment);

			Assert.AreEqual("another partial payment received, still not fully paid", result);
		}

		[Test]
        public void ProcessPayment_Should_ReturnPartiallyPaidMessage_When_NoPartialPaymentExistsAndAmountPaidIsLessThanInvoiceAmount()
		{
			Initialize(new Invoice
			{
				Amount = 10,
				AmountPaid = 0,
				Payments = new List<Payment>()
			});

			var payment = new Payment
			{
				Amount = 1
			};
            var result = _paymentProcessor.ProcessPayment(payment);

            Assert.AreEqual("invoice is now partially paid", result);
		}
	}
}