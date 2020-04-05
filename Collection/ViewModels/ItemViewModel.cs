using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collection.ViewModels
{
    public class ItemModel
    {
        public int IdItem { get; set; }
        public int IdCollection { get; set; }
        public string ItemName { get; set; }
        public string Tegs { get; set; }
        public List<string> ItemValue { get; set; }
    }
}
