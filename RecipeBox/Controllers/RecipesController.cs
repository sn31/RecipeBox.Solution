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
        public ActionResult Create(string newName, string ingredients, string newInstructions, int newRating, int categorySelect)
        {
            Recipe newRecipe = new Recipe(newName, newInstructions, newRating);
            newRecipe.Save();
            Category addCategory = Category.Find(categorySelect);
            newRecipe.AddCategory(addCategory);
            ingredients = ingredients.Trim();
            string [] ingredientsArray = ingredients.Split(',');
            foreach (string ingredient in ingredientsArray)
            {
                string trimmedIngredient = ingredient.Trim();
                Ingredient foundIngredient = Ingredient.Find(trimmedIngredient);
                if (foundIngredient == null)
                {
                    foundIngredient = new Ingredient(trimmedIngredient.ToLower());
                    foundIngredient.Save();
                }
                foundIngredient.AddRecipe(newRecipe.Id);
            }
            return RedirectToAction("Index");
        }
        [HttpGet("/recipes/{recipeId}")]
        public ActionResult Details(int recipeId)
        {
            Recipe foundRecipe = Recipe.Find(recipeId);
            List <string> tagColors = new List <string> {"primary", "secondary", "success", "danger", "warning", "info", "light", "dark"};
            List <Ingredient> allIngredients = foundRecipe.GetIngredients();
            Dictionary <string, object> dict = new Dictionary<string, object>();
            dict.Add("recipe", foundRecipe);
            dict.Add("tagColors", tagColors);
            dict.Add("ingredients", allIngredients);
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