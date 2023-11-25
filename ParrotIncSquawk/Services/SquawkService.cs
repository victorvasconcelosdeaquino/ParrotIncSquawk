using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using ParrotIncSquawk.Entities;
using ParrotIncSquawk.Persistence;
using ParrotIncSquawk.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<Squawk>> GetAll()
        {
            return await _squawkContext.Squawks.ToListAsync();
        }

        public async Task<Squawk> GetById(Guid userId)
        {
            return await _squawkContext.Squawks.FindAsync(userId);
        }

        public async Task<Squawk> Create(Guid userId, Squawk squawk)
        {
            var errors = new List<string>();
            SquawkValidation validationRules = new SquawkValidation(_squawkContext);
            ValidationResult validationResult = validationRules.Validate(squawk);

            if (validationResult.IsValid == false)
            {
                foreach (ValidationFailure failure in validationResult.Errors)
                {
                    errors.Add(failure.ErrorMessage);
                }
            }

            if (errors.Count > 0)
                throw new System.Exception(String.Join(", ", errors));

            _squawkContext.Add(squawk);
            await _squawkContext.SaveChangesAsync();
            return squawk;
        }
    }
}
