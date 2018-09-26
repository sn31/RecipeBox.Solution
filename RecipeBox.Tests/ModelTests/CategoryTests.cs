using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeBox.Models;
using MySql.Data.MySqlClient;

namespace RecipeBox.Tests
{
  [TestClass]
  public class CategoryTest : IDisposable
  {
    public CategoryTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipe_box_test;";
    }
    public void Dispose()
    {
      Category.DeleteAll();
      Recipe.DeleteAll();

      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"ALTER TABLE tags AUTO_INCREMENT = 1;";
      cmd.ExecuteNonQuery();
      cmd.CommandText = @"ALTER TABLE categories AUTO_INCREMENT = 1;";
      cmd.ExecuteNonQuery();
      cmd.CommandText = @"ALTER TABLE recipes AUTO_INCREMENT = 1;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    [TestMethod]
    public void GetAll_DBStartFromEmpty_List()
    {
      //Arrange
      List <Category> expectedCategories = new List <Category> {};

      //Act
      List <Category> allCategories = Category.GetAll();

      //Assert
      CollectionAssert.AreEqual(expectedCategories, allCategories);
    }
    [TestMethod]
    public void Save_AddNewCategory()
    {
      //Arrange
      Category newCategory = new Category("Dessert");

      //Act
      newCategory.Save();
      List <Category> allCategories = Category.GetAll();
      List <Category> expectedCategories = new List <Category> {newCategory};

      //Assert
      CollectionAssert.AreEqual(expectedCategories, allCategories);
    }
    [TestMethod]
    public void Find_FindExactCategory_Category()
    {
      //Arrange
      Category newCategory = new Category("Dessert");
      newCategory.Save();

      //Act
      Category foundCategory = Category.Find(newCategory.Id);

      //Assert
      Assert.AreEqual(newCategory, foundCategory);
    }
    [TestMethod]
    public void GetAll_GetAllCategories_List()
    {
      //Arrange
      Category newCategory1 = new Category("Dessert");
      newCategory1.Save();
      Category newCategory2 = new Category("Main Dish");
      newCategory2.Save();
      List <Category> expectedCategories = new List <Category> {newCategory1, newCategory2};

      //Act
      List <Category> allCategories = Category.GetAll();

      //Assert
      CollectionAssert.AreEqual(expectedCategories, allCategories);
    }
    [TestMethod]
    public void Update_UpdateNameProperly()
    {
      //Arrange
      Category newCategory1 = new Category("Dessert");
      newCategory1.Save();
      string newName = "Main Dish";

      //Act
      newCategory1.Update(newName);

      //Assert
      Assert.AreEqual(newName, newCategory1.Name);
    }
    [TestMethod]
    public void Delete_DeleteCategory()
    {
      //Arrange
      Category newCategory1 = new Category("Dessert");
      newCategory1.Save();
      Category newCategory2 = new Category("Main Dish");
      newCategory2.Save();
      List <Category> expectedCategories = new List <Category> { newCategory2 };

      //Act
      Category.Delete(newCategory1.Id);
      List <Category> allCategories = Category.GetAll();

      //Assert
      CollectionAssert.AreEqual(expectedCategories, allCategories);
    }
    [TestMethod]
    public void DeleteAll_DeleteEverything()
    {
      //Arrange
      Category newCategory1 = new Category("Dessert");
      newCategory1.Save();
      Category newCategory2 = new Category("Main Dish");
      newCategory2.Save();
      List <Category> expectedCategories = new List <Category> {};

      //Act
      Category.DeleteAll();
      List <Category> allCategories = Category.GetAll();

      //Assert
      CollectionAssert.AreEqual(expectedCategories, allCategories);
    }
    [TestMethod]
    public void AddRecipe_AddARecipeProperly()
    {
      //Arrange
      Category newCategory1 = new Category("Dessert");
      newCategory1.Save();
      Recipe newRecipe = new Recipe("Pancake", "1. Foo bar 2. Foo bar", 5);
      newRecipe.Save();
      List <Recipe> expectedRecipes = new List <Recipe> {newRecipe};
      
      //Act
      newCategory1.AddRecipe(newRecipe.Id);
      List <Recipe> allRecipes = newCategory1.GetRecipes();

      //Assert
      CollectionAssert.AreEqual(expectedRecipes, allRecipes);
    }
    [TestMethod]
    public void GetRecipes_ReturnAllRecipes_List()
    {
      //Arrange
      Category newCategory1 = new Category("Dessert");
      newCategory1.Save();
      Recipe newRecipe1 = new Recipe("Pancake", "1. Foo bar 2. Foo bar", 5);
      newRecipe1.Save();
      Recipe newRecipe2 = new Recipe("Scrambled Egg", "1. Foo bar 2. Foo bar", 5);
      newRecipe2.Save();
      newCategory1.AddRecipe(newRecipe1.Id);
      newCategory1.AddRecipe(newRecipe2.Id);
      List <Recipe> expectedRecipes = new List <Recipe> {newRecipe1, newRecipe2};
      
      //Act
      List <Recipe> allRecipes = newCategory1.GetRecipes();

      //Assert
      CollectionAssert.AreEqual(expectedRecipes, allRecipes);
    }
  }
}
