namespace KdtSdk.Exceptions
{
    /// <summary>
    /// This exception will be thrown whenever Konduto's API responds with an error HTTP status.
    /// </summary>
    public abstract class KondutoHTTPException : KondutoException
    {
        protected KondutoHTTPException(string message)
            : base(message){}

        /// <summary>
        /// </summary>
        /// <param name="message">@param message instance's message</param>
        /// <param name="responseBody">responseBody Konduto's API response</param>
	    protected KondutoHTTPException(string message, string responseBody)
            : base($"{message} Response body: {responseBody}")
        {
            
	    }
    }
}