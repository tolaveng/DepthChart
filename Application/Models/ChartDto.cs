using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class ChartDto
    {
        public Guid Id { get; set; }
        public int PlayerNumber { get; set; }
        public string Group { get; set; }
        public string PositionId { get; set; }
        public int Depth { get; set; }

        public virtual PlayerDto Player { get; set; }
        public virtual PositionDto Position { get; set; }
    }
}
