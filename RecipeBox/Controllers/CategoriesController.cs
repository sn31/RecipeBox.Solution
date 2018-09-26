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
    [HttpGet("/categories/{categoryId}")]
    public ActionResult Details(int categoryId)
    {
      Category foundCategory = Category.Find(categoryId);
      return View(foundCategory);
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