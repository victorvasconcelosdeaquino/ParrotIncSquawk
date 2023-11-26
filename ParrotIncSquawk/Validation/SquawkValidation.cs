using FluentValidation;
using ParrotIncSquawk.Constants.Errors;
using ParrotIncSquawk.Models;
using ParrotIncSquawk.Persistence;
using System;
using System.Linq;

namespace ParrotIncSquawk.Validation
{
    public sealed class SquawkValidation : AbstractValidator<SquawkRequest>
    {
        private readonly SquawkContext _squawkContext;
        public static readonly string[] blacklistedWords = { "Tweet", "Twitter" };

        /// <summary>
        /// This method validates if the text contains lenght between 1 and 400 or words not allowed
        /// </summary>
        public SquawkValidation()
        {
            RuleFor(p => p.Text)
                .MaximumLength(400)
                .WithMessage(ErrorsConstants.SquawkError.LessThenOrEqual400);
            RuleFor(p => p.Text)
                .MinimumLength(1)
                .WithMessage(ErrorsConstants.SquawkError.AtLeastOne);

            RuleFor(p => p.Text)
                .Must(pass => !blacklistedWords
                .Any(word => pass.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0))
                .WithMessage(ErrorsConstants.SquawkError.BlackList);
        }
    }
}
