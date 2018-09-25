using System;

namespace KdtSdk.Models
{
    public class KondutoDebitPayment : KondutoPayment
    {
        public KondutoDebitPayment()
            : base(KondutoPaymentType.debit) { }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is KondutoDebitPayment)) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}