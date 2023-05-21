using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class ChartRequest
    {
        public string Position { get; set; }
        public Player Player { get; set; }
        public int? Depth { get; set; }

        public ChartRequest(string position, Player player, int? depth)
        {
            Position = position;
            Player = player;
            Depth = depth;
        }

        public class ChartRequestValidator : AbstractValidator<ChartRequest>
        {
            public ChartRequestValidator()
            {
                RuleFor(x => x.Position).NotNull().NotEmpty().WithMessage("Position is required");
                RuleFor(x => x.Depth).GreaterThanOrEqualTo(0).When(x => x.Depth != null)
                    .WithMessage("Depth must be greater than 0"); ;
                RuleFor(x => x.Player).SetValidator(new PlayerValidator()).When(x => x.Player != null);
            }
        }
    }
}
