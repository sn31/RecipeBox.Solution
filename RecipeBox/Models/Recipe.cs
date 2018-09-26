namespace RecipeBox.Models
{
    public class Recipe
    {
        public int Id {get;set;}
        public string Name { get; set; }
        public int Rating {get;set;}

        public Recipe(string name, int rating, int id = 0)
        {
            Name = name;
            Rating= rating;
            Id = id;
        }
    }
}