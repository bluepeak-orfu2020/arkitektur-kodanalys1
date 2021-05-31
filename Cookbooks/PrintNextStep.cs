using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbooks
{
    class PrintNextStep : ICookingAction
    {
        private RecipeService service;

        public PrintNextStep(RecipeService service)
        {
            this.service = service;
        }

        public void Execute(List<string> data)
        {
            try
            {
                string nextStep = service.NextStep();
                if (nextStep == null)
                {
                    Console.WriteLine("The recipe is finished. There are no more steps.");
                }
                else
                {
                    Console.WriteLine(nextStep);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
