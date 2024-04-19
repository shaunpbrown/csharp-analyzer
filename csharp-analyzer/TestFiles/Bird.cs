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

        public bool _isInAir { get; set; } = false;

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
                SetIsInAir(true);
            }
            else
            {
                Console.WriteLine("I am not able to fly");
            }
        }

        public void SetIsInAir(bool isInAir)
        {
            _isInAir = isInAir;
        }
    }
}
