using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace RecipeBox.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public string Instructions { get; set; }

        public Recipe(string name, string instructions, int rating, int id = 0)
        {
            Name = name;
            Instructions = instructions;
            Rating = rating;
            Id = id;
        }
        public override bool Equals(System.Object otherRecipe)
        {
            if (!(otherRecipe is Recipe))
            {
                return false;
            }
            else
            {
                Recipe newRecipe = (Recipe) otherRecipe;
                bool idEquality = this.Id == newRecipe.Id;
                bool nameEquality = this.Name == newRecipe.Name;
                return (idEquality && nameEquality);
            }
        }
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
        public override string ToString()
        {
            return String.Format("{{ id={0}, name={1}, instr={2}, date={3}}}", Id, Name, Instructions, Rating);
        }
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"INSERT INTO recipes (`name`,`instructions`,`rating`) VALUES (@newName,@newInstruction,@newRating);";
            cmd.Parameters.AddWithValue("@newName", this.Name);
            cmd.Parameters.AddWithValue("@newInstruction", this.Instructions);
            cmd.Parameters.AddWithValue("@newRating", this.Rating);
            cmd.ExecuteNonQuery();
            Id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public static List<Recipe> GetAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT*FROM recipes;";
            MySqlDataReader rdr = cmd.ExecuteReader();
            List<Recipe> allRecipes = new List<Recipe> { };

            while (rdr.Read())
            {
                int Id = rdr.GetInt32(0);
                string Name = rdr.GetString(1);
                string Instructions = rdr.GetString(2);
                int Rating = rdr.GetInt32(3);
                Recipe newRecipe = new Recipe(Name, Instructions, Rating, Id);
                allRecipes.Add(newRecipe);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allRecipes;
        }
        public static Recipe Find(int searchId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT*FROM recipes WHERE id = @searchId;";
            cmd.Parameters.AddWithValue("@searchId", searchId);

            MySqlDataReader rdr = cmd.ExecuteReader();

            rdr.Read();
            int Id = rdr.GetInt32(0);
            string Name = rdr.GetString(1);
            string Instructions = rdr.GetString(2);
            int Rating = rdr.GetInt32(3);
            Recipe foundRecipe = new Recipe(Name, Instructions, Rating, Id);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundRecipe;
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM recipes_ingredients WHERE recipe_id = @deleteId; DELETE FROM tags WHERE recipe_id = @deleteId; DELETE FROM recipes WHERE id = @deleteId;";
            cmd.Parameters.AddWithValue("@deleteId", this.Id);
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
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM recipes_ingredients; DELETE FROM tags; DELETE FROM recipes";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public void Update(string Name, string Instructions, int Rating)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE recipes SET name = @newName,instructions = @newInstructions, rating = @newRating WHERE id = @thisId;";
            cmd.Parameters.AddWithValue("@newName", Name);
            cmd.Parameters.AddWithValue("@newInstructions", Instructions);
            cmd.Parameters.AddWithValue("@newRating", Rating);
            cmd.Parameters.AddWithValue("@thisId", this.Id);

            this.Name = Name;
            this.Instructions = Instructions;
            this.Rating = Rating;
            
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
         public void AddCategory(Category newCategory)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO tags (`category_id`, `recipe_id`) VALUES (@CategoryId, @RecipeId);";
            cmd.Parameters.AddWithValue("@CategoryId", newCategory.Id);
            cmd.Parameters.AddWithValue("@RecipeId", this.Id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public List<Category> GetCategories()
        {
            List<Category> allCategories = new List<Category>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT categories.* FROM recipes
            JOIN tags ON (recipes.id = tags.recipe_id)
            JOIN categories ON (tags.category_id = categories.id)
            WHERE recipes.id = @thisId;";
            cmd.Parameters.AddWithValue("@thisId",this.Id);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int Id = rdr.GetInt32(0);
                string Name = rdr.GetString(1);
                Category newCategory = new Category(Name, Id);
                allCategories.Add(newCategory);
            }
            conn.Close();
            if (conn!=null)
            {
                conn.Dispose();
            }
        return allCategories;
        }
        public void AddIngredient(Ingredient ingredient)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO recipes_ingredients (recipe_id, ingredient_id) VALUES (@recipe_id, @ingredient_id);";
            cmd.Parameters.AddWithValue("@recipe_id", this.Id);
            cmd.Parameters.AddWithValue("@ingredient_id", ingredient.Id);
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        
        public List <Ingredient> GetIngredients()
        {
            List <Ingredient> allIngredients = new List <Ingredient> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT recipes_ingredients.id, recipes_ingredients.recipe_id, recipes_ingredients.ingredient_id, ingredients.name FROM ingredients JOIN recipes_ingredients ON ingredients.id = recipes_ingredients.ingredient_id WHERE recipes_ingredients.recipe_id = @id;";
            cmd.Parameters.AddWithValue("@id", this.Id);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            while (rdr.Read())
            {
                int id = rdr.GetInt32(2);
                string name = Formatting.ToTitleCase(rdr.GetString(3));
                Ingredient foundIngredient = new Ingredient(name, id);
                allIngredients.Add(foundIngredient);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allIngredients;
        }
    }
}