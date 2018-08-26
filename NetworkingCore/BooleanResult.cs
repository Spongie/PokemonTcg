namespace NetworkingCore
{
    public struct BooleanResult
    {
        public BooleanResult(bool result)
        {
            Result = result;
        }

        public bool Result { get; }

        public static implicit operator bool(BooleanResult value)
        {
            return value.Result;
        }

        public static implicit operator BooleanResult(bool value)
        {
            return new BooleanResult(value);
        }
    }
}
