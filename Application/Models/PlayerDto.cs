﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class PlayerDto
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public int TeamId { get; set; }

        public virtual ICollection<ChartDto> Charts { get; set; }
    }
}
