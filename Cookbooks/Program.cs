using System;
using System.Collections.Generic;

namespace Cookbooks
{
    class Program
    {
        RecipeService recipeService;

        static void Main(string[] args)
        {
            RecipeService service = new RecipeService();
            new Program(service).Run();
        }

        public Program(RecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        public void Run()
        {
            Dictionary<string, ICookingAction> actions = new Dictionary<string, ICookingAction>() {
                { "load", new LoadRecipeAction(recipeService) },
                { "next", new PrintNextStep(recipeService) },
                { "list", new PrintIngredients(recipeService) }
            };

            string line;
            while ((line = FetchActionLine()) != "")
            {
                string[] parts = line.Split(" ");
                string actionName = parts[0];
                ICookingAction action = actions.GetValueOrDefault(actionName, null);
                if (action == null)
                {
                    Console.WriteLine($"Unknown action {actionName}");
                }
                else
                {
                    List<string> data = new List<string>(parts);
                    data.RemoveAt(0); // remove action name from list
                    action.Execute(data);
                }
                Console.WriteLine();
            }
        }

        private static string FetchActionLine()
        {
            Console.WriteLine("Enter an action (or blank line to exit)");
            return Console.ReadLine();
        }
    }
}
