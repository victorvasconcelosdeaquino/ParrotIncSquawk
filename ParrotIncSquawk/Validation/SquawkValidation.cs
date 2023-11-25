using FluentValidation;
using ParrotIncSquawk.Entities;
using ParrotIncSquawk.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParrotIncSquawk.Validation
{
    public class SquawkValidation : AbstractValidator<Squawk>
    {
        private readonly SquawkContext _squawkContext;

        public SquawkValidation(SquawkContext squawkContext)
        {
            _squawkContext = squawkContext;

            var blacklistedWords = new List<string> { "Tweet", "Twitter" };

            RuleFor(p => p.Text)
                .MaximumLength(400)
                .WithMessage("'{PropertyName}' should have only 400 max characters.");
            RuleFor(p => p.Text)
                .MinimumLength(1)
                .WithMessage("'{PropertyName}' must be at least 1 character.");

            RuleFor(p => p)
                .Must(p => !IsDuplicate(p))
                .WithMessage("'{PropertyName}' cannot be duplicated.");

            RuleFor(p => p.Text)
                .Must(pass => !blacklistedWords
                .Any(word => pass.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0))
                .WithMessage("'{PropertyName}' contains a word that is not allowed.");
        }

        private bool IsDuplicate(Squawk squawk)
        {
            return _squawkContext.Squawks
                .Any(x => x.Text == squawk.Text &&
                x.UserId == squawk.UserId);
        }
    }
}
