using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbooks
{
    class LoadRecipeAction : ICookingAction
    {
        private RecipeService service;

        public LoadRecipeAction(RecipeService service)
        {
            this.service = service;
        }

        public void Execute(List<string> data)
        {
            if (data.Count != 2)
            {
                Console.WriteLine("Wrong format for loading recipe. Params should be cookBookName recipeName");
                return;
            }
            string cookBookName = data[0];
            string recipeName = data[1];
            service.LoadRecipe(recipeName, cookBookName);
        }
    }
}
