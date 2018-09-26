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
            return View();
        }
        [HttpPost("/recipes")]
        public ActionResult Create(string newName, string newInstructions, int newRating)
        {
            Recipe newRecipe = new Recipe(newName, newInstructions, newRating);
            newRecipe.Save();
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
            return View(foundRecipe);
        }
        [HttpPost("/recipes/update/{recipeId}")]
        public ActionResult Update(int recipeId, string newName, string newInstructions, int newRating)
        {
            Recipe foundRecipe = Recipe.Find(recipeId);
            foundRecipe.Update(newName, newInstructions, newRating);
            return RedirectToAction("Details");
        }
    }
}