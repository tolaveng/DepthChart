using FluentValidation;

namespace Server.Models
{
    public class ChartRequest
    {
        public string Position { get; set; }
        public PlayerRequest Player { get; set; }
        public int? Depth { get; set; }
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
