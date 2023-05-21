using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class ChartRemoveRequest
    {
        public string Position { get; set; }
        public int PlayerNumber { get; set; }

        public ChartRemoveRequest(string position, int playerNumber)
        {
            Position = position;
            PlayerNumber = playerNumber;
        }
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
