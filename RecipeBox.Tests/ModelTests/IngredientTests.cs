using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeBox.Models;
using MySql.Data.MySqlClient;

namespace RecipeBox.Tests
{
    [TestClass]
    public class IngredientTest : IDisposable
    {
        public IngredientTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipe_box_test;";
        }
        public void Dispose()
        {
            Category.DeleteAll();
            Recipe.DeleteAll();
            Ingredient.DeleteAll();

            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"ALTER TABLE tags AUTO_INCREMENT = 1;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"ALTER TABLE recipes_ingredients AUTO_INCREMENT = 1;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"ALTER TABLE categories AUTO_INCREMENT = 1;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"ALTER TABLE recipes AUTO_INCREMENT = 1;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"ALTER TABLE ingredients AUTO_INCREMENT = 1;";
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}