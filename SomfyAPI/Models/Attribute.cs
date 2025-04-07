using System.ComponentModel.DataAnnotations.Schema;

namespace SomfyAPI.Models
{
    public class Attribute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public object Value { get; set; }
    }
}
