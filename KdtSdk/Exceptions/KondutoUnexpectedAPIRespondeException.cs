namespace KdtSdk.Exceptions
{
    /// <summary>
    /// This exception is thrown whenever Konduto's API responds something we cannot handle.
    /// Please contact our support team if this ever happens.
    /// </summary>
    public class KondutoUnexpectedAPIResponseException : KondutoException 
    {
        public KondutoUnexpectedAPIResponseException(string responseBody)
            : base($"Unexpected API response: {responseBody}") { }
    }
}