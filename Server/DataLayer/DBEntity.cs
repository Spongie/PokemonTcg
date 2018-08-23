namespace Server.DataLayer
{
    public class DBEntity
    {
        public long Id { get; set; }

        public string GetTableName()
        {
            return GetType().FullName.Replace('.', '_');
        }
    }
}
