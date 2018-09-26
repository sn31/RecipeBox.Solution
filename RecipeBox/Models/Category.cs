using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace RecipeBox.Models {
    public class Category {
        public int Id { get; set; }
        public string Name { get; set; }

        public Category (string name, int id = 0) {
            Name = name;
            Id = id;
        }

        public override bool Equals (System.Object otherCategory) {
            if (!(otherCategory is Category)) {
                return false;
            } else {
                Category newCategory = (Category) otherCategory;
                bool idEquality = (this.Id == newCategory.Id);
                bool nameEquality = (this.Name == newCategory.Name);
                return (idEquality && nameEquality);
            }
        }

        public override int GetHashCode () {
            return this.Name.GetHashCode ();
        }

        public void Save () {
            MySqlConnection conn = DB.Connection ();
            conn.Open ();

            MySqlCommand cmd = conn.CreateCommand () as MySqlCommand;
            cmd.CommandText = @"INSERT INTO categories (name) VALUES (@name);";
            cmd.Parameters.AddWithValue ("@name", this.Name);
            cmd.ExecuteNonQuery ();
            this.Id = (int) cmd.LastInsertedId;

            conn.Close ();
            if (conn != null) {
                conn.Dispose ();
            }
        }

        public static Category Find (int id) {
            MySqlConnection conn = DB.Connection ();
            conn.Open ();

            MySqlCommand cmd = conn.CreateCommand () as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM categories WHERE id=@id;";
            cmd.Parameters.AddWithValue ("@id", id);
            MySqlDataReader rdr = cmd.ExecuteReader () as MySqlDataReader;

            Category foundCategory = new Category ("");
            while (rdr.Read ()) {
                int foundId = rdr.GetInt32 (0);
                string foundName = rdr.GetString (1);
                foundCategory.Id = foundId;
                foundCategory.Name = foundName;
            }

            conn.Close ();
            if (conn != null) {
                conn.Dispose ();
            }
            return foundCategory;
        }

        public static List<Category> GetAll () {
            List <Category> allCategories = new List <Category> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM categories;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            while (rdr.Read())
            {
                int foundId = rdr.GetInt32(0);
                string foundName = rdr.GetString(1);
                Category foundCategory = new Category(foundName, foundId);
                allCategories.Add(foundCategory);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return allCategories;
        }

        public void Update (string newName) {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE categories SET name = @newName WHERE id = @id;";
            cmd.Parameters.AddWithValue("@newName", newName);
            cmd.Parameters.AddWithValue("@id", this.Id);
            cmd.ExecuteNonQuery();

            this.Name = newName;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static void Delete (int id) {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM tags WHERE category_id = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.CommandText = @"DELETE FROM categories WHERE id = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static void DeleteAll () {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM tags;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"DELETE FROM categories;";
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void AddRecipe(int recipeId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO tags (category_id, recipe_id) VALUES (@categoryId, @recipeId);";
            cmd.Parameters.AddWithValue("@categoryId", this.Id);
            cmd.Parameters.AddWithValue("@recipeId", recipeId);
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List <Recipe> GetRecipes()
        {
            List <Recipe> allRecipes = new List <Recipe> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT tags.id, tags.category_id, categories.name, tags.recipe_id, recipes.name, recipes.instructions, recipes.rating FROM recipes JOIN tags ON recipes.id = tags.recipe_id JOIN categories ON tags.category_id = categories.id WHERE category_id = @categoryId;";
            cmd.Parameters.AddWithValue("@categoryId", this.Id);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            while (rdr.Read())
            {
                int foundId = rdr.GetInt32(3);
                string foundName = rdr.GetString(4);
                string foundInstructions = rdr.GetString(5);
                int foundRating = rdr.GetInt32(6);

                Recipe foundRecipe = new Recipe(foundName, foundInstructions, foundRating, foundId);
                allRecipes.Add(foundRecipe);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return allRecipes;
        }
    }
}