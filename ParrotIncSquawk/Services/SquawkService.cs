using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ParrotIncSquawk.Constants.Errors;
using ParrotIncSquawk.Entities;
using ParrotIncSquawk.Models;
using ParrotIncSquawk.Persistence;
using ParrotIncSquawk.Validation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ParrotIncSquawk.Services
{
    public class SquawkService : ISquawkService
    {
        private readonly SquawkContext _squawkContext;

        public SquawkService(SquawkContext squawkContext)
        {
            _squawkContext = squawkContext;
        }

        public async Task<IEnumerable<Squawk>> GetAll(CancellationToken cancellationToken)
        {
            return await _squawkContext.Squawks.
                AsNoTracking().
                ToListAsync(cancellationToken);
        }

        public async Task<Squawk> GetById(Guid userId,
            Guid squawkId,
            CancellationToken cancellationToken)
        {
            return await _squawkContext.Squawks.
                AsNoTracking().
                FirstOrDefaultAsync(u => u.UserId == userId && u.SquawkId == squawkId, cancellationToken);
        }

        public async Task<Result<Squawk>> Create(Guid userId, SquawkRequest model, CancellationToken cancellationToken)
        {
            SquawkValidation validationRules = new();
            ValidationResult validationResult = validationRules.Validate(model);

            if (!validationResult.IsValid)
                return Result<Squawk>.Invalid(validationResult.AsErrors());

            bool hasUserSameText = await _squawkContext.Squawks
            .AsNoTracking()
            .AnyAsync(x => x.Text == model.Text && x.UserId == userId, cancellationToken);

            if (hasUserSameText)
                return Result<Squawk>.Error(ErrorsConstants.SquawkError.DuplicatedError);

            EntityEntry<Squawk> squawk = await _squawkContext.Squawks.AddAsync(new Squawk()
            {
                Text = model.Text,
                UserId = userId,
                SquawkDate = DateTime.UtcNow,
            });

            await _squawkContext.SaveChangesAsync();

            return Result<Squawk>.Success(squawk.Entity);
        }
    }
}
