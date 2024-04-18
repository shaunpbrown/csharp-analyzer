using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_analyzer.TestFiles
{
    public class Animal
    {
        public string _type { get; set; } = string.Empty;

        public int _age { get; set; }

        public Animal(string type, int age)
        { 
            _type = type;
            _age = age;
        }

        public string GetAnimalType()
        {
            return _type;
        }

        public int GetAge()
        {
            return _age;
        }
    }
}
