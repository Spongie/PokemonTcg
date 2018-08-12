namespace Server.DataLayer.Queries
{
    internal struct UpdateField
    {
        public UpdateField(string column, string value)
        {
            Column = column;
            Value = value;
        }

        public string Column { get; set; }
        public string Value { get; set; }

        public string GenerateQuery()
        {
            return $"{Column} = {Value}";
        }
    }
}
