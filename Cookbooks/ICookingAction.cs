using System.Collections.Generic;

namespace Cookbooks
{
    internal interface ICookingAction
    {
        void Execute(List<string> data);
    }
}