using Ardalis.Result;
using ParrotIncSquawk.Entities;
using ParrotIncSquawk.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ParrotIncSquawk.Services
{
    public interface ISquawkService
    {
        Task<IEnumerable<Squawk>> GetAll(CancellationToken cancellationToken);
        Task<Squawk> GetById(Guid userId, Guid squawkId,    CancellationToken cancellationToken);
        Task<Result<Squawk>> Create(Guid userId, SquawkRequest model, CancellationToken cancellationToken);
    }
}
