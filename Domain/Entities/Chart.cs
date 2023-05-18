using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Chart
    {
        [Key]
        public Guid Id { get; set; }
        public int PlayerNumber { get; set; }
        public int TeamId { get; set; }
        public string PositionId { get; set; }
        public int Depth { get; set; }

        public virtual Player Player { get; set; }
        public virtual Team Team { get; set; }
        public virtual Position Position { get; set; }
    }
}
