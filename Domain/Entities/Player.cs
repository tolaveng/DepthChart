using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Player
    {
        [Key]
        public int Number { get; set; }
        public string Name { get; set; }
        public string PositionId { get; set; }

        public virtual ICollection<Chart> Charts { get; set; }
    }
}
