using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collection.Models
{
    public class Field
    {
        public Field(string name, string type)
        {
            Name = name;

            Type = type;
        }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
