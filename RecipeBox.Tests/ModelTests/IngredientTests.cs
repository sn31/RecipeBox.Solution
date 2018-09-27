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
        [TestMethod]
        public void GetAll_DBStartFromEmpty()
        {
            //Arrange
            int expectedCount = 0;

            //Act
            List <Ingredient> allIngredients = Ingredient.GetAll();
            int count = allIngredients.Count;

            //Assert
            Assert.AreEqual(expectedCount, count);
        }
        [TestMethod]
        public void Save_AddNewIngredient()
        {
            //Arrange
            Ingredient newIngredient = new Ingredient("Egg");
            newIngredient.Save();
            List <Ingredient> expectedIngredients = new List <Ingredient> {newIngredient};

            //Act
            List <Ingredient> ingredients = Ingredient.GetAll();

            //Assert
            CollectionAssert.AreEqual(expectedIngredients, ingredients);
        }
        [TestMethod]
        public void GetAll_ReturnEveryIngredients_List()
        {
            //Arrange
            Ingredient newIngredient1 = new Ingredient("Egg");
            newIngredient1.Save();
            Ingredient newIngredient2 = new Ingredient("Milk");
            newIngredient2.Save();
            List <Ingredient> expectedIngredients = new List <Ingredient> {newIngredient1, newIngredient2};
            
            //Act
            List <Ingredient> ingredients = Ingredient.GetAll();

            //Assert
            CollectionAssert.AreEqual(expectedIngredients, ingredients);
        }
        [TestMethod]
        public void Find_FindExactIngredient_Ingredient()
        {
            //Arrange
            Ingredient newIngredient1 = new Ingredient("Egg");
            newIngredient1.Save();
            Ingredient newIngredient2 = new Ingredient("Milk");
            newIngredient2.Save();

            //Act
            Ingredient foundIngredient = Ingredient.Find(newIngredient1.Id);

            //Assert
            Assert.AreEqual(newIngredient1, foundIngredient);
        }
        [TestMethod]
        public void Update_UpdateNewName()
        {
            //Arrange
            Ingredient newIngredient1 = new Ingredient("Egg");
            newIngredient1.Save();
            string newName = "Milk";

            //Act
            newIngredient1.Update(newName);

            //Assert
            Assert.AreEqual(newName, newIngredient1.Name);
        }
        [TestMethod]
        public void Delete_DeleteIngredientProperly()
        {
            //Arrange
            Ingredient newIngredient1 = new Ingredient("Egg");
            newIngredient1.Save();
            Ingredient newIngredient2 = new Ingredient("Milk");
            newIngredient2.Save();
            List <Ingredient> expectedIngredients = new List <Ingredient> {newIngredient2};

            //Act
            Ingredient.Delete(newIngredient1.Id);
            List <Ingredient> ingredients = Ingredient.GetAll();

            //Assert
            CollectionAssert.AreEqual(expectedIngredients, ingredients);
        }
        [TestMethod]
        public void DeleteAll_DeleteAllIngredients()
        {
            //Arrange
            Ingredient newIngredient1 = new Ingredient("Egg");
            newIngredient1.Save();
            Ingredient newIngredient2 = new Ingredient("Milk");
            newIngredient2.Save();
            List <Ingredient> expectedIngredients = new List <Ingredient> {};

            //Act
            Ingredient.DeleteAll();
            List <Ingredient> ingredients = Ingredient.GetAll();

            //Assert
            CollectionAssert.AreEqual(expectedIngredients, ingredients);
        }
        [TestMethod]
        public void AddRecipe_AddRecipeExactly()
        {   
            //Arrange
            Ingredient newIngredient1 = new Ingredient("Egg");
            newIngredient1.Save();
            Recipe newRecipe = new Recipe("Scrambled Eggs", "1. Foo \n2. Bar", 8);
            newRecipe.Save();
            List <Recipe> expectedRecipes = new List <Recipe> {newRecipe};

            //Act
            newIngredient1.AddRecipe(newRecipe.Id);
            List <Recipe> recipes = newIngredient1.GetRecipes();

            //Assert
            CollectionAssert.AreEqual(expectedRecipes, recipes);
        }
        [TestMethod]
        public void GetRecipes_GetAllRecipes_List()
        {
            //Arrange
            Ingredient newIngredient1 = new Ingredient("Egg");
            newIngredient1.Save();
            Recipe newRecipe = new Recipe("Scrambled Eggs", "1. Foo \n2. Bar", 8);
            newRecipe.Save();
            List <Recipe> expectedRecipes = new List <Recipe> {newRecipe};
            newIngredient1.AddRecipe(newRecipe.Id);

            //Act
            List <Recipe> recipes = newIngredient1.GetRecipes();

            //Assert
            CollectionAssert.AreEqual(expectedRecipes, recipes);
        }
    }
}