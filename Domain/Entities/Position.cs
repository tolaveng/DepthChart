using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Position
    {
        [Key]
        public string Id {  get; set; }
        public string Name { get; set; }
    }
}
