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
    }
  }
}
