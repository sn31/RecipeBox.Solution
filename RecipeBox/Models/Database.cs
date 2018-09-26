using System;
using MySql.Data.MySqlClient;
using RecipeBox;
using static RecipeBox.Startup;

namespace RecipeBox.Models
{
    public class DB
    {
        public static MySqlConnection Connection()
        {
            MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);
            return conn;
        }
    }
}
