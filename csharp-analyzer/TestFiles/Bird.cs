using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_analyzer.TestFiles
{
    public class Bird : Animal
    {
        public bool _canFly { get; set; } = false;

        public Bird(int age, string type) : base("Bird", age)
        {
            if (!string.IsNullOrEmpty(type))
                _type = type;
        }

        public void Fly()
        {
            if (_canFly && BirdHelperFunctions.CanFly(_type))
            {
                Console.WriteLine("I am flying");
            }
            else
            {
                Console.WriteLine("I am not able to fly");
            }
        }
    }
}
