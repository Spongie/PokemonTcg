namespace NetworkingCore
{
    public class LongResult
    {
        public LongResult(long result)
        {
            Result = result;
        }

        public long Result { get; }

        public static implicit operator long(LongResult value)
        {
            return value.Result;
        }

        public static implicit operator LongResult(long value)
        {
            return new LongResult(value);
        }
    }
}
