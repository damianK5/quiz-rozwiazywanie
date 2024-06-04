using MySql.Data.MySqlClient;

namespace quiz_rozwiazywanie.DAL
{
    class DBConnection
    {
        private MySqlConnectionStringBuilder stringBuilder = new MySqlConnectionStringBuilder();
        private static DBConnection instance = null;

        public static DBConnection Instance
        {
            get
            {
                if (instance == null)
                    instance = new DBConnection();
                return instance;
            }
        }

        private MySqlConnection connection;
        public MySqlConnection Connection 
        {
            get {return new MySqlConnection(stringBuilder.ToString()); }
        }

        private DBConnection() 
        {
            stringBuilder.UserID = "quiz";
            stringBuilder.Server = "localhost";
            stringBuilder.Database = "quiz";
            stringBuilder.Port = 3306;
            stringBuilder.Password = "soybean";

            connection = new MySqlConnection(stringBuilder.ToString());
        }
    }

}
