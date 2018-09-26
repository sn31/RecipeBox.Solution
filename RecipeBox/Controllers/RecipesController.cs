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
            Dictionary<string, object> model = new Dictionary<string, object> { };
            List<Category> allCategories = Category.GetAll();
            List<Recipe> allRecipes = Recipe.GetAll();
            model.Add("categories", allCategories);
            model.Add("recipes", allRecipes);
            return View(model);
        }
        [HttpGet("/recipes/new")]
        public ActionResult CreateForm()
        {
            return View();
        }
    }
}