using fileToDatabase;

internal class Program
{
    private static void Main(string[] args)
    {
        Seeder seeder = new Seeder();
        seeder.ColumnFileName = "../../../data/crimedataColumns.json";
        seeder.DataFileName = "../../../data/header.csv ";
        seeder.DatabaseType = DbTypes.DatabaseType.MySql;
        seeder.TableName = "crime_data";
        seeder.Seed();

       

    }
}