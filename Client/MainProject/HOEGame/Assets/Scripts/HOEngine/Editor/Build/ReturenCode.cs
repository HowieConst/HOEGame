namespace HOEngine.Editor
{
    public enum ReturnCode
    {
        // Success Codes are Positive!
        /// <summary>
        /// Use to indicate that the operation suceeded.
        /// </summary>
        Success = 0,
        
        /// <summary>
        ///Use to indicate that the operation suceeded but did not actually execute.
        /// </summary>
        SuccessNotReturn = 1,
        /// <summary>
        /// Use to indicate that the operation encountered an error.
        /// </summary>
        Error = 2,
        /// <summary>
        /// Use to indicate that the operation encountered an exception.
        /// </summary>
        Exception = 3,
    }
}