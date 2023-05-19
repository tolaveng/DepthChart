using FluentValidation;

namespace Server.Models
{
    public class PlayerRequest
    {
        public int Number { get; set; }
        public string Name { get; set; }
    }

    public class PlayerValidator : AbstractValidator<PlayerRequest>
    {
        public PlayerValidator()
        {
            RuleFor(x => x.Number).NotNull().WithMessage("Player Number is required")
                .NotEmpty().WithMessage("Player Number is required")
                .GreaterThan(0).WithMessage("Player Number must be greater than 0");
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
