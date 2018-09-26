using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeBox.Models;

namespace RecipeBox.Tests
{
    [TestClass]
    public class RecipeTest : IDisposable
    {
        public RecipeTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipe_box_test;";
        }
        public void Dispose()
        {
            Recipe.DeleteAll();
            Category.DeleteAll();
        }

        [TestMethod]
        public void GetAll_DatabaseEmptyAtFirst_0()
        {
            int result = Recipe.GetAll().Count;
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Equals_TrueForSameRecipeName_Recipe()
        {
            Recipe firstRecipe = new Recipe("Cheesecake", "Do something here", 5);
            Recipe secondRecipe = new Recipe("Cheesecake", "Do something here", 5);

            Assert.AreEqual(firstRecipe, secondRecipe);
        }

        [TestMethod]
        public void Save_RecipeSavesToDatabase_RecipeList()
        {
            //Arrange
            Recipe testRecipe = new Recipe("Cheesecake", "Do something here", 5);
            testRecipe.Save();

            //Act
            List<Recipe> result = Recipe.GetAll();
            List<Recipe> testList = new List<Recipe> { testRecipe };

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Save_AssignsIdToObject_id()
        {
            //Arrange
            Recipe testRecipe = new Recipe("Steak", "Do something here", 3);
            testRecipe.Save();

            //Act
            Recipe savedRecipe = Recipe.GetAll() [0];

            int result = savedRecipe.Id;
            int testId = testRecipe.Id;

            //Assert
            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Find_FindsRecipeInDatabase_Recipe()
        {
            Recipe testRecipe = new Recipe("Jello", "Do something here", 4);
            testRecipe.Save();

            Recipe result = Recipe.Find(testRecipe.Id);

            Assert.AreEqual(testRecipe, result);
        }

        [TestMethod]
        public void AddCategory_AddsCategoryToRecipe_CategoryList()
        {
            Recipe testRecipe = new Recipe("Cow Cake", "Some steps",4);
            testRecipe.Save();

            Category testCategory = new Category("Junk Food");
            testCategory.Save();

            testRecipe.AddCategory(testCategory);
            List<Category> result = testRecipe.GetCategories();
            List<Category> testList = new List<Category> { testCategory };

            CollectionAssert.AreEqual(testList, result);
        }
        [TestMethod]
        public void GetCategories_ReturnAllRecipeCategories_CategoriesList()
        {
            Recipe testRecipe = new Recipe("Cow Cake", "Some steps",4);
            testRecipe.Save();

            Category testCategory1 = new Category("Junk Food");
            testCategory1.Save();
            Category testCategory2 = new Category("Health Food");
            testCategory2.Save();

            testRecipe.AddCategory(testCategory1);
            testRecipe.AddCategory(testCategory2);
            List<Category> result = testRecipe.GetCategories();
            List<Category> testList = new List<Category> { testCategory1,testCategory2 };

            CollectionAssert.AreEqual(testList, result);
        }
        [TestMethod]
        public void Delete_DeletesRecipeAssociationsFromDatabase_RecipeList()
        {
            Category testCategory = new Category("Breakfast");
            testCategory.Save();

            Recipe testRecipe = new Recipe("cookies","foo bar", 1);
            testRecipe.Save();
            testRecipe.AddCategory(testCategory);
            testRecipe.Delete();

            List<Recipe> resultCategoryRecipes = testCategory.GetRecipes();
            List<Recipe> testCategoryRecipes = new List<Recipe> { };

            //Assert
            CollectionAssert.AreEqual(testCategoryRecipes, resultCategoryRecipes);
        }
        [TestMethod]
        public void Update_UpdateRecipeWithNewInfo_Recipe()
        {
             Recipe testRecipe = new Recipe("cookies","foo bar", 1);
             testRecipe.Save();
             testRecipe.Update("Milk","foo bar",2);

             string result = testRecipe.Name;

             Assert.AreEqual("Milk",result);

        }

    }
}