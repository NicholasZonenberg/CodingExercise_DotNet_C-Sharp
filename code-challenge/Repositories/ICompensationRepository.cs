using challenge.Models;
using System;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    // Interface for the Compensation Repository
    public interface ICompensationRepository
    {
        Compensation GetById(String id);
        Compensation Add(Compensation compensation);
        Task SaveAsync();
    }
}
