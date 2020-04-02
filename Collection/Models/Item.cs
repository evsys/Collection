using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collection.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int IdCollection { get; set; }

        public string Name { get; set; }

        public string Values { get; set; }

        public string Tegs { get; set; }

        public CollectionDb Collection { get; set; }

        [NotMapped]
        public List<string> FormattedValues
        {
            get
            {
                List<string> result = new List<string>();
                string[] values = Values.Split(new char[] { ',' });

                for (int i = 0; i < values.Length; i++)
                {
                    result.Add(values[i]);
                }

                return result;
            }
            set
            {
                string result = "";
                if (value != null)
                {
                    for (int i = 0; i < value.Count; i++)
                    {
                        result += value[i];
                        if (i != value.Count - 1)
                        {
                            result += ",";
                        }
                    }

                }

                Values = result;
            }
        }

        [NotMapped]
        public List<string> FormattedTegs
        {
            get
            {
                List<string> result = new List<string>();

                if (Tegs != null)
                {
                    string[] tegs = Tegs.Split(new char[] { '#' });

                    for (int i = 1; i < tegs.Length; i++)
                    {
                        result.Add(tegs[i]);
                    }
                }
                return result;
            }
        }
    }
}
