using System.Collections.Generic;

namespace Cookbooks
{
    public class Recipe
    {
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<RecipeStep> Steps { get; set; }

        private int currentStepIndex;

        public Recipe()
        {
            currentStepIndex = 0;
        }

        public string NextStep()
        {
            if (currentStepIndex < Steps.Count)
            {
                string stepDescription = Steps[currentStepIndex].Description;
                currentStepIndex++;
                return stepDescription;
            }
            else
            {
                return null;
            }
        }
    }

    public class Ingredient
    {
        public string Name { get; set; }
        public float Amount { get; set; }
    }

    public class RecipeStep
    {
        public string Description { get; set; }
    }
}