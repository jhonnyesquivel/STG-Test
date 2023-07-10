namespace STG.Infrastructure.DbInit
{
    using System.Data.SqlClient;

    public static class DatabaseInitializer
    {
        public static void Initialize(string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            var scriptFilePath = Path.Combine(AppContext.BaseDirectory, "DbInit", "dbscript.sql");
            var script = File.ReadAllText(scriptFilePath);
            var commandStrings = script.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var commandString in commandStrings)
            {
                using (var command = new SqlCommand(commandString, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            connection.Close();
        }
    }
}