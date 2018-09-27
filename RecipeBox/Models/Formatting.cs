using System;
using MySql.Data.MySqlClient;
using RecipeBox;
using static RecipeBox.Startup;
using System.Text;

namespace RecipeBox.Models
{
    public class Formatting
    {
        public static string ToTitleCase(string words)
        {
            string [] wordsArray = words.Split(' ');
            StringBuilder builder = new StringBuilder();
            for (int i=0; i<wordsArray.Length; i++)
            {
                builder = new StringBuilder(wordsArray[i].Length);
                for (int ii=0; ii<wordsArray[i].Length; ii++)
                {
                    if (ii == 0)
                    {
                        builder.Append(wordsArray[i][ii].ToString().ToUpper());
                    }
                    else
                    {
                        builder.Append(wordsArray[i][ii]);
                    }
                }
            }
            return builder.ToString();
        }
    }
}