namespace KdtSdk.Models
{
    public class KondutoTransferPayment : KondutoPayment
    {
        public KondutoTransferPayment()
            : base(KondutoPaymentType.transfer) { }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is KondutoTransferPayment)) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}