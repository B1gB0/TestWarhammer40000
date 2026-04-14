using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.Entity;
using Project.Scripts.UI.ViewModel;
using R3;

namespace Project.Scripts.Services
{
    public interface IAbilityService : IService
    {
        public ReactiveProperty<AbilityViewModel> HoveredAbility { get; }
        public UniTask<List<Ability>> CreateAllAbilitiesByCount(int count);
    }
}