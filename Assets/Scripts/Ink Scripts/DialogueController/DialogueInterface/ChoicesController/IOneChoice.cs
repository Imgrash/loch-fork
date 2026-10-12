using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorySpace
{
    public interface IOneChoice
    {
        public void Set(Choices root, KeyValuePair<int, string> choiceVal, ITextField tf);
        public void makeChoice();
    }
}
