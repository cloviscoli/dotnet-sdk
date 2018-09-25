namespace KdtSdk.Models
{
    public class KondutoVoucherPayment : KondutoPayment
    {
        public KondutoVoucherPayment()
            : base(KondutoPaymentType.voucher) { }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is KondutoVoucherPayment)) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}