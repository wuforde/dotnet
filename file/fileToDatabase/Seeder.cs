using System.Text;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MySql.Data;
using MySql.Data.MySqlClient;
using ZstdSharp.Unsafe;

namespace fileToDatabase
{

    public class Seeder
    {
        public enum FileTypes {Csv,Json}
        public string? DataFileName {get;set;}
        public string? ColumnFileName {get;set;}
        public string? FileType {get;set;}
        public string? TableName {get;set;}
        public DbTypes.DatabaseType DatabaseType {get;set;}
        private JObject dbConfig {get;set;}

        public Seeder()
        {
            TextReader t = new StreamReader("../../../config/db_config.json");
            JsonReader jsonReader = new JsonTextReader(t);
            this.dbConfig = JObject.Load(jsonReader);
        }

        public void Seed()
        {
            string tableStatement = CreateTableStatement(GetColumnData());
            using(MySqlConnection conn = new MySqlConnection(String.Format("server={0};user={1};database={2};port=3306;password={3}",dbConfig["host"],dbConfig["user"],dbConfig["db"],dbConfig["password"])))
            {
                conn.Open();
                using(MySqlCommand command = conn.CreateCommand())
                {
                    command.CommandText = "drop table if exists " + this.TableName;
                    command.ExecuteNonQuery();

                    command.CommandText = tableStatement;
                    command.ExecuteNonQuery();
                }
            }

            BulkLoadMySql();

        }

        public Stream GetDataFile()
        {


            return null;
        }

        public JArray GetColumnData()
        {
            JArray ret = new JArray();
            StreamReader reader = new StreamReader(this.ColumnFileName);
            JsonReader jsonReader = new JsonTextReader(reader);
            JArray cols = JArray.Load(jsonReader);

            foreach(JObject item in cols)
            {
                JObject temp = new JObject();
                temp.Add("name",item["fieldName"]);
                temp.Add("type",item["dataTypeName"]);
                ret.Add(temp);
            }
            return ret;
        }

        public string CreateTableStatement(JArray cols)
        {
            StringBuilder createStatement = new StringBuilder();

            createStatement.Append("create table " + this.TableName + " (");
            Dictionary<string,string> colTypes = DbTypes.GetTypes(this.DatabaseType);
            foreach(var item in cols)
            {
                string temp = item["type"].Value<string>();

                createStatement.Append(item["name"] + " " + colTypes[temp] + ",");
            }

            createStatement.Remove(createStatement.Length -1,1);
            createStatement.Append(");");

            return createStatement.ToString();
        }

        private void BulkLoad(JArray items)
        {
            JArray cols = this.GetColumnData();


        }

        private void BulkLoadMySql()
        {
            using(MySqlConnection conn = new MySqlConnection(String.Format("server={0};user={1};database={2};port=3306;password={3}",dbConfig["host"],dbConfig["user"],dbConfig["db"],dbConfig["password"])))
            {
                conn.Open();
                MySqlBulkLoader mySqlBulkLoader = new MySqlBulkLoader(conn);
                mySqlBulkLoader.FieldTerminator = ",";
                mySqlBulkLoader.LineTerminator = "\r\n";
                mySqlBulkLoader.Local = true;
                mySqlBulkLoader.FileName = this.DataFileName;
                mySqlBulkLoader.NumberOfLinesToSkip = 1;
                mySqlBulkLoader.FieldQuotationOptional = true;
                mySqlBulkLoader.FieldQuotationCharacter = '"';
                mySqlBulkLoader.TableName = this.TableName;

                mySqlBulkLoader.Load();
            }

        }
    }
}