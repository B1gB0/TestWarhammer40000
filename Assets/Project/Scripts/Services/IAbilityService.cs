using System.Collections.Generic;
using Project.Scripts.Entity;

namespace Project.Scripts.Services
{
    public interface IAbilityService : IService
    {
        public List<Ability> CreateAllAbilitiesByCount(int count);
    }
}