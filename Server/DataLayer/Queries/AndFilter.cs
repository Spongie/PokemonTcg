namespace Server.DataLayer.Queries
{
    internal class AndFilter : Filter
    {
        public override string GenerateSql() => $"{Column.Name} = {Util.GetValueSqlFormatted(Value, Column.PropertyType)}";
    }
}
