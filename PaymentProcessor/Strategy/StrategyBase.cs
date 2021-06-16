namespace PaymentProcessor.Strategy
{
    public abstract class StrategyBase
    {
        protected StrategyBase(Invoice invoice, Payment payment)
        {
            Invoice = invoice;
            Payment = payment;
        }

        public virtual string Handle()
        {
            return string.Empty;
        }

        public Invoice Invoice { get; }

        public Payment Payment { get; }
    }
}
