using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.Entity;

namespace Project.Scripts.Services
{
    public interface IAbilityService : IService
    {
        public UniTask<List<Ability>> CreateAllAbilitiesByCount(int count);
    }
}