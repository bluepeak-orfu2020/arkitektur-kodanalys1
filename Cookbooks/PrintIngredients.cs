using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbooks
{
    class PrintIngredients : ICookingAction
    {
        private RecipeService service;

        public PrintIngredients(RecipeService service)
        {
            this.service = service;
        }

        public void Execute(List<string> data)
        {
            try
            {
                List<string> ingredients = service.ListIngredients();
                string result = "Ingredients:";
                foreach (string ingredient in ingredients)
                {
                    result += $"\n  {ingredient}";
                }
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
