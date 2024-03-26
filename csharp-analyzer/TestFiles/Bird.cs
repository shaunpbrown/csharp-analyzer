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

        public Bird(int age) : base("Bird", age)
        {
        }

        public void Fly()
        {
            if (_canFly)
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
