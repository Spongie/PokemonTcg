namespace DataLayer
{
    public abstract class DBEntity
    {
        public long Id { get; set; }

        public string GetTableName()
        {
            return GetType().FullName.Replace('.', '_');
        }
    }
}
