using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace RecipeBox.Models {
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Ingredient(string name, int id = 0)
        {
            Id = id;
            Name = name;
        }

        public override bool Equals(System.Object otherIngredient){
            if (!(otherIngredient is Ingredient))
            {
                return false;
            }
            else
            {
                Ingredient newIngredient = (Ingredient) otherIngredient;
                bool idEquality = (this.Id == newIngredient.Id);
                bool nameEquality = (this.Name == newIngredient.Name);
                return (idEquality && nameEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO ingredients (name) VALUES (@name);";
            cmd.Parameters.AddWithValue("@name", this.Name);
            cmd.ExecuteNonQuery();
            this.Id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Ingredient> GetAll()
        {
            List <Ingredient> allIngredients = new List <Ingredient>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM ingredients;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            
            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                Ingredient newIngredient = new Ingredient(name, id);
                allIngredients.Add(newIngredient);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allIngredients;
        }

        public static Ingredient Find(int searchId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM ingredients WHERE id=@id;";
            cmd.Parameters.AddWithValue("@id", searchId);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            Ingredient foundIngredient = new Ingredient("");

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                foundIngredient.Id = id;
                foundIngredient.Name = name;
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundIngredient;
        }

        public void Update(string newName)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE ingredients SET name = @newName WHERE id=@id;";
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

        public static void Delete(int searchId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM ingredients WHERE id=@id;";
            cmd.Parameters.AddWithValue("@id", searchId);
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"DELETE FROM recipes_ingredients WHERE ingredient_id=@id;";
            cmd.Parameters.AddWithValue("@id", searchId);
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM recipes_ingredients;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"DELETE FROM ingredients;";
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
            cmd.CommandText = @"INSERT INTO recipes_ingredients (recipe_id, ingredient_id) VALUES (@recipe_id, @ingredient_id);";
            cmd.Parameters.AddWithValue("@recipe_id", recipeId);
            cmd.Parameters.AddWithValue("@ingredient_id", this.Id);
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Recipe> GetRecipes()
        {
            List <Recipe> allRecipes = new List <Recipe> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT recipes_ingredients.id, recipes_ingredients.recipe_id, recipes.name, recipes.instructions, recipes.rating, recipes_ingredients.ingredient_id, ingredients.name FROM recipes JOIN recipes_ingredients ON recipes.id = recipes_ingredients.id JOIN ingredients ON recipes_ingredients.ingredient_id = ingredients.id WHERE recipes_ingredients.id = @id";
            cmd.Parameters.AddWithValue("@id", this.Id);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            Ingredient foundIngredient = new Ingredient("");

            while (rdr.Read())
            {
                int id = rdr.GetInt32(1);
                string name = rdr.GetString(2);
                string instructions = rdr.GetString(3);
                int rating = rdr.GetInt32(4);
                Recipe foundRecipe = new Recipe(name, instructions, rating, id);
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