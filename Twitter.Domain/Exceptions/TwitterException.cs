using System;
using System.Runtime.Serialization;

namespace Twitter.Domain.Exceptions
{
    [Serializable]
    public class TwitterException : Exception
    {
        public TwitterException() { }
        
        public TwitterException(string message) : base(message) { }
        
        public TwitterException(string message, Exception innerException) : base(message, innerException) { }
        
        protected TwitterException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
