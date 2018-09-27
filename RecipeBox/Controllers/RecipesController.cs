using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;

namespace RecipeBox.Controllers
{
    public class RecipesController:Controller{
        [HttpGet("/recipes")]
        public ActionResult Index()
        {
            List<Recipe> allRecipes = Recipe.GetAll();
            
            return View(allRecipes);                            
        }
        [HttpGet("/recipes/new")]
        public ActionResult CreateForm()
        {
            List<Category> allCategories = Category.GetAll();
            return View(allCategories);
        }
        [HttpPost("/recipes")]
        public ActionResult Create(string newName, string newInstructions, int newRating, int categorySelect)
        {
            Recipe newRecipe = new Recipe(newName, newInstructions, newRating);
            Category addCategory = Category.Find(categorySelect);
            newRecipe.Save();
            newRecipe.AddCategory(addCategory);
            return RedirectToAction("Index");
        }
        [HttpGet("/recipes/{recipeId}")]
        public ActionResult Details(int recipeId)
        {
            Recipe foundRecipe = Recipe.Find(recipeId);
            List <string> tagColors = new List <string> {"primary", "secondary", "success", "danger", "warning", "info", "light", "dark"};
            Dictionary <string, object> dict = new Dictionary<string, object>();
            dict.Add("recipe", foundRecipe);
            dict.Add("tagColors", tagColors);
            return View(dict);
        }
        [HttpGet("/recipes/update/{recipeId}")]
        public ActionResult UpdateForm(int recipeId)
        {
            Recipe foundRecipe = Recipe.Find(recipeId);
            List <string> tagColors = new List <string> {"primary", "secondary", "success", "danger", "warning", "info", "light", "dark"};
            Dictionary <string, object> dict = new Dictionary<string, object>();
            dict.Add("recipe", foundRecipe);
            dict.Add("tagColors", tagColors);
            return View(dict);
        }
        [HttpPost("/recipes/update/{recipeId}")]
        public ActionResult Update(int recipeId, string newName, string newInstructions, int newRating)
        {
            Recipe foundRecipe = Recipe.Find(recipeId);
            foundRecipe.Update(newName, newInstructions, newRating);
            return RedirectToAction("Details");
        }
        [HttpPost("/recipes/delete/{recipeId}")]
        public ActionResult Delete(int recipeId)
        {
            Recipe deleteRecipe = Recipe.Find(recipeId);
            deleteRecipe.Delete();

            return RedirectToAction("Index");
        }
    }
}