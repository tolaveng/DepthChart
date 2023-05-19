using FluentValidation;

namespace Server.Models
{
    public class ChartRemoveRequest
    {
        public string Position { get; set; }
        public int PlayerNumber { get; set; }
    }

    public class ChartRemoveValidator : AbstractValidator<ChartRemoveRequest>
    {
        public ChartRemoveValidator()
        {
            RuleFor(x => x.Position).NotNull().WithMessage("Position is required")
                .NotEmpty().WithMessage("Position is required");

            RuleFor(x => x.PlayerNumber).NotNull().WithMessage("Player Number is required")
                .GreaterThan(0).WithMessage("Player Number must be greater than 0");
        }
    }
}
