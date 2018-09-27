using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;

namespace RecipeBox.Controllers
{
  public class CategoriesController : Controller
  {
    [HttpGet("/categories")]
    public ActionResult Index()
    {
      List <Category> allCategories = Category.GetAll();
      return View(allCategories);
    }
    [HttpPost("/categories/{categoryId}")]
    public ActionResult DetailsAddRecipe(int categoryId, int recipeSelect)
    {
      Dictionary <string, object> dict = new Dictionary<string, object>{};
      Category foundCategory = Category.Find(categoryId);
      foundCategory.AddRecipe(recipeSelect);
      List<Recipe> allRecipes = Recipe.GetAll();
      dict.Add("category",foundCategory);
      dict.Add("recipes",allRecipes);
      return RedirectToAction("Details");
    }
    [HttpPost("/categories/{categoryId}/remove-from-category")]
    public ActionResult RemoveRecipeFromCategory(int categoryId, int recipeId)
    {
      Category foundCategory = Category.Find(categoryId);
      foundCategory.RemoveRecipe(recipeId);
      return RedirectToAction("Details");
    }
    [HttpGet("/categories/{categoryId}")]
    public ActionResult Details(int categoryId)
    {
      Dictionary <string, object> dict = new Dictionary<string, object>{};
      Category foundCategory = Category.Find(categoryId);
      List<Recipe> allRecipes = Recipe.GetAll();
      dict.Add("category",foundCategory);
      dict.Add("recipes",allRecipes);
      return View(dict);
    }
    
    [HttpGet("/categories/new")]
    public ActionResult CreateForm()
    {
      return View();
    }
    [HttpPost("/categories")]
    public ActionResult Create(string newName)
    {
      Category newCategory = new Category(newName);
      newCategory.Save();
      return RedirectToAction("Index");
    }
    [HttpGet("/categories/update/{categoryId}")]
    public ActionResult UpdateForm(int categoryId)
    {
      Category foundCategory = Category.Find(categoryId);
      return View(foundCategory);
    }
    [HttpPost("/categories/update/{categoryId}")]
    public ActionResult Update(int categoryId, string newName)
    {
      Category foundCategory = Category.Find(categoryId);
      foundCategory.Update(newName);
      return RedirectToAction("Details");
    }
    [HttpPost("/categories/delete/{categoryId}")]
    public ActionResult Delete(int categoryId)
    {
      Category.Delete(categoryId);
      return RedirectToAction("Index");
    }

  }
}