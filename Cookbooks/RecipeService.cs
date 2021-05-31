using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cookbooks
{
    class RecipeService
    {
        private Recipe currentRecipe = null;

        public void LoadRecipe(string name, string cookBookName)
        {
            string cookBookPath = Path.Combine(Environment.CurrentDirectory, @"CookBooks", $"{cookBookName}.txt");
            if (!File.Exists(cookBookPath))
            {
                throw new Exception($"Unknown cook book {cookBookName}");
            }
            string cookBookContent = File.ReadAllText(cookBookPath);
            List<string> recipes;
            if (cookBookName == "cookbook1")
            {
                recipes = cookBookContent.Split(Environment.NewLine + Environment.NewLine).ToList();
            }
            else if (cookBookName == "cookbook2")
            {
                recipes = cookBookContent.Split(@"$$$").ToList();
            }
            else
            {
                throw new Exception($"Unknown cook book {cookBookName}");
            }

            foreach (string recipeData in recipes)
            {
                Recipe recipe = ParseRecipe(recipeData, cookBookName);
                if (recipe.Name == name)
                {
                    currentRecipe = recipe;
                    return;
                }
            }
            throw new Exception($"Could not find recipe: {name}");
        }

        private Recipe ParseRecipe(string recipeData, string cookBookName)
        {
            string recipeName = "N/A";
            List<Ingredient> ingredients = new List<Ingredient>();
            List<RecipeStep> steps = new List<RecipeStep>();
            if (cookBookName == "cookbook1")
            {
                Regex recipeRegex = new Regex(@"^([^:]+): (.+)$", RegexOptions.Multiline);
                MatchCollection matches = recipeRegex.Matches(recipeData);
                foreach (Match match in matches)
                {
                    string key = match.Groups[1].Value;
                    if (key == "name")
                    {
                        recipeName = match.Groups[2].Value.Trim();
                    }
                    else if (key == "ingredients")
                    {
                        string ingredientsData = match.Groups[2].Value;
                        string[] ingredientList = ingredientsData.Split(", ");
                        Regex ingredientRegex = new Regex(@"(.*?)\((\d+)\)");
                        foreach (string ingredient in ingredientList)
                        {
                            Match ingridiantMatch = ingredientRegex.Match(ingredient);
                            if (!ingridiantMatch.Success)
                            {
                                Console.WriteLine($"Failed to load ingredient: {ingredient}");
                            }
                            else
                            {
                                ingredients.Add(new Ingredient()
                                {
                                    Amount = float.Parse(ingridiantMatch.Groups[2].Value, CultureInfo.InvariantCulture.NumberFormat),
                                    Name = ingridiantMatch.Groups[1].Value
                                });
                            }
                        }
                    }
                    else if (key == "steps")
                    {
                        string ingredientsData = match.Groups[2].Value;
                        steps.AddRange(
                            ingredientsData
                                .Split(", ")
                                .ToList()
                                .Select(stepDescription => new RecipeStep() { Description = stepDescription })
                        );

                    }
                }
            }
            else if (cookBookName == "cookbook2")
            {
                string[] parts = recipeData.Split("&&&");
                if (parts.Length != 3)
                {
                    Console.WriteLine($"Failed to load recipe: {recipeData}");
                }
                else
                {
                    recipeName = parts[0];
                    ingredients = parts[1]
                        .Split("%%%")
                        .Select(data => {
                            string[] ingredientParts = data.Split("###");
                            return new Ingredient() { Name = ingredientParts[0], Amount = float.Parse(ingredientParts[1]) };
                        })
                        .ToList();
                    steps = parts[2]
                        .Split("%%%")
                        .Select(data => new RecipeStep() { Description = data })
                        .ToList();
                }
            }

            return new Recipe() { Name = recipeName, Ingredients = ingredients, Steps = steps };
        }

        public string NextStep()
        {
            if (currentRecipe == null)
            {
                throw new Exception("No recipe is loaded");
            }
            else
            {
                return currentRecipe.NextStep();
            }
        }

        public List<string> ListIngredients()
        {
            if (currentRecipe == null)
            {
                throw new Exception("No recipe is loaded");
            }
            else
            {
                return currentRecipe.Ingredients.Select(i => $"{i.Name} ({i.Amount})").ToList();
            }
        }
    }
}
