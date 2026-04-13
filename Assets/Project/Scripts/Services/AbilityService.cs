using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Scripts.Database.Data;
using Project.Scripts.Entity;
using Reflex.Attributes;

namespace Project.Scripts.Services
{
    public class AbilityService : IAbilityService
    {
        private readonly Dictionary<string, AbilityData> _abilitiesData = new();
        private readonly Random _random = new();

        private IDataBaseService _dataBaseService;
        public bool IsInitiated { get; private set; }

        [Inject]
        private void Construct(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public UniTask Init()
        {
            if (IsInitiated)
                return UniTask.CompletedTask;

            foreach (var data in _dataBaseService.Content.AbilitiesData)
            {
                _abilitiesData.TryAdd(data.Id, data);
            }

            IsInitiated = true;

            return UniTask.CompletedTask;
        }

        public async UniTask<List<Ability>>  CreateAllAbilitiesByCount(int count)
        {
            if (count <= 0)
                return new List<Ability>();

            if (_abilitiesData.Count == 0)
                throw new InvalidOperationException("Нет доступных способностей для создания.");

            var result = new List<Ability>(count);
            var allDataList = _abilitiesData.Values.ToList();
            int availableCount = allDataList.Count;
            
            var shuffled = allDataList.OrderBy(_ => _random.Next()).ToList();
            
            int uniqueAbilities = Math.Min(count, availableCount);
            for (int i = 0; i < uniqueAbilities; i++)
            {
                Ability ability = new Ability(shuffled[i]);
                await ability.Data.LoadIconAsync();
                result.Add(ability);
            }
            
            int remaining = count - uniqueAbilities;
            for (int i = 0; i < remaining; i++)
            {
                int randomIndex = _random.Next(availableCount);
                Ability ability = new Ability(allDataList[randomIndex]);
                await ability.Data.LoadIconAsync();
                result.Add(ability);
            }

            return result;
        }
    }
}