using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collection.Models
{
    public class CollectionDb
    {
        public CollectionDb(string name, string description, string theme)
        {
            Name = name;
            Description = description; 
            Theme = theme;
            Image = "path";
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Theme { get; set; }

        public string Image { get; set; }

        public string Fields { get; set; }

        [NotMapped]
        public int CountItems { get; set; }

        [NotMapped]

        public List<Field> FormattedFields
        {
            get
            {
                List<Field> result = new List<Field>();

                if (!string.IsNullOrEmpty(Fields))
                {
                    string[] field = Fields.Split(new char[] { ';' });

                    for (int i = 0; i < field.Length; i++)
                    {
                        string[] partsOfField = field[i].Split(new char[] { ',' });

                        result.Add(new Field(partsOfField[0], partsOfField[1]));
                    }
                }

                return result;
            }
            set
            {
                string fields = "";
                for (int i = 0; i < value.Count; i++)
                {
                    fields += value[i].Name + "," + value[i].Type;
                    if (i != value.Count - 1)
                    {
                        fields += ";";
                    }
                }
                Fields = fields;
            }
        }

        public string IdUser { get; set; }

        public User User { get; set; }
    }
}