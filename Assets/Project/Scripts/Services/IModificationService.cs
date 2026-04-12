using System.Collections.Generic;
using Project.Scripts.Entity;

namespace Project.Scripts.Services
{
    public interface IModificationService : IService
    {
        public List<Modification> CreateAllModificationsByCount(int count);
    }
}