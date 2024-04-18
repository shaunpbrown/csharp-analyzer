using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_analyzer.TestFiles
{
    public static class BirdHelperFunctions
    {
        public static bool CanFly(string birdName)
        {
            birdName = birdName.Trim().ToLower();  
            if(birdName == "penguin" || birdName == "ostrich")
            {
                return false;
            }
            
            return true;
        }
    }
}
