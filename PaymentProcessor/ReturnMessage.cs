namespace PaymentProcessor
{
    public static class ReturnMessage
    {
        public const string NOINVOICE = "There is no invoice matching this payment";
        public const string INVOICEINVALID = "The invoice is in an invalid state, it has an amount of 0 and it has payments.";
        public const string NOPAYMENTNEEDED = "no payment needed";
        public const string ISFULLYPAID = "invoice was already fully paid";
        public const string PAYMENTGREATERTHANPARTIALAMOUNT = "the payment is greater than the partial amount remaining";
        public const string FINALPAYMENTRECEIVED = "final partial payment received, invoice is now fully paid";
        public const string PARTIALPAYMENTRECEIVED = "another partial payment received, still not fully paid";
        public const string PAYMETGREATERTHANINVOICEAMOUNT = "the payment is greater than the invoice amount";
        public const string INVOICEFULLYPAID = "invoice is now fully paid";
        public const string INVOICEPARTIALLYPAID = "invoice is now partially paid";
    }
}
