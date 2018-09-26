using System;
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
            Assert.AreEqual(0,result);
        }
    }
}