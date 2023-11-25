using ParrotIncSquawk.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParrotIncSquawk.Services
{
    public interface ISquawkService
    {
        Task<IEnumerable<Squawk>> GetAll();
        Task<Squawk> GetById(Guid userId);
        Task<Squawk> Create(Guid userId, Squawk squawk);
    }
}
