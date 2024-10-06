namespace fileToDatabase
{
    public static class DbTypes
    {
        public enum DatabaseType {MySql, SqlServer}
        public static Dictionary<string,string> GetTypes(DatabaseType databaseType)
        {
            Dictionary<string,string> ret = new Dictionary<string, string>();;

            switch(databaseType)
            {
                case DatabaseType.MySql:
                    ret = GetMySqlTypes();
                    break;
                case DatabaseType.SqlServer:
                    ret = GetSqlServerTypes();
                    break;
                default:
                    break;
            }

            return ret;
        }

        private static Dictionary<string,string> GetMySqlTypes()
        {
            Dictionary<string,string> ret = new Dictionary<string, string>();
            ret.Add("text", "MEDIUMTEXT");
            ret.Add("number", "numeric");
            ret.Add("calendar_date","datetime");

            return ret;
        }

        private static Dictionary<string,string> GetSqlServerTypes()
        {
            Dictionary<string,string> ret = new Dictionary<string, string>();
            ret.Add("text", "text");
            ret.Add("number", "numeric");
            ret.Add("calendar_date","datetime");

            return ret;
        }


    }
}