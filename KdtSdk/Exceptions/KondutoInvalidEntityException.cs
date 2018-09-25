using KdtSdk.Models;

namespace KdtSdk.Exceptions
{
    /// <summary>
    /// This exception is thrown when a {@link KondutoModel} instance is invalid.
    /// </summary>
    public class KondutoInvalidEntityException : KondutoException 
    {
	    public KondutoInvalidEntityException(KondutoModel entity)
            : base($"{entity} is invalid: {entity.GetError()}") { }
    }
}
