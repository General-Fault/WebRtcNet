namespace WebRtcNet
{
    /// <summary>
    /// <seealso href="http://w3c.github.io/mediacapture-main/#idl-def-MediaStreamError"/>
    /// </summary>
    public class MediaStreamError
    {
        MediaStreamError(string name, string message, string constraintName)
        {
            Name = name;
            Message = message;
            ConstraintName = constraintName;
        }

        /// The name of the error.
        public string Name { get; }

        /// A string offering extra human-readable information about the error.
        public string Message { get; }


        /// This attribute is only used for some types of errors. For MediaStreamError with a name of 
        /// ConstraintNotSatisfiedError or of OverconstrainedError, this attribute must be set to the name of the constraint that caused 
        public string ConstraintName { get; }
    };
}
