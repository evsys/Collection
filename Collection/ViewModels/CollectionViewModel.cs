using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collection.Models
{
    public class CollectionViewModel
    {
        public int IdCollection { get; set; }

        public string NameCollection { get; set; }

        public string Description { get; set; }

        public string Theme { get; set; }

        public List<string> NameField { get; set; }

        public List<string> TypeField { get; set; }

        public string NameUser { get; set; }
    }
}
