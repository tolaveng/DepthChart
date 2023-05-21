using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class Player
    {
        public int Number { get; set; }
        public string Name { get; set; }

        public Player(int number, string name)
        {
            Number = number;
            Name = name;
        }

        public Player(int number)
        {
            Number = number;
            Name = "";
        }
    }

    public class PlayerValidator : AbstractValidator<Player>
    {
        public PlayerValidator()
        {
            RuleFor(x => x.Number).NotNull().WithMessage("Player Number is required")
                .NotEmpty().WithMessage("Player Number is required")
                .GreaterThan(0).WithMessage("Player Number must be greater than 0");

            RuleFor(x => x.Name).NotNull().WithMessage("Player Name is required")
                .NotEmpty().WithMessage("Player Name is required");
        }
    }

    public class PlayerValidatorWithoutName : AbstractValidator<Player>
    {
        public PlayerValidatorWithoutName()
        {
            RuleFor(x => x.Number).NotNull().WithMessage("Player Number is required")
                .NotEmpty().WithMessage("Player Number is required")
                .GreaterThan(0).WithMessage("Player Number must be greater than 0");
        }
    }
}
