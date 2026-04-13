using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Scripts.Database.Data;
using Project.Scripts.Entity;
using Reflex.Attributes;

namespace Project.Scripts.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly Dictionary<string, CharacterData> _charactersData = new();

        private List<CharacterData> _charactersList;
        private readonly Random _random = new();

        private List<int> _remainingIndices;
        private bool _useUniqueCycle;

        private IDataBaseService _dataBaseService;
        private IAbilityService _abilityService;
        private IModificationService _modificationService;

        public bool IsInitiated { get; private set; }

        [Inject]
        private void Construct(
            IDataBaseService dataBaseService,
            IAbilityService abilityService,
            IModificationService modificationService)
        {
            _dataBaseService = dataBaseService;
            _abilityService = abilityService;
            _modificationService = modificationService;
        }

        public UniTask Init()
        {
            if (IsInitiated)
                return UniTask.CompletedTask;

            foreach (var data in _dataBaseService.Content.CharactersData)
            {
                _charactersData.TryAdd(data.Name, data);
            }

            _charactersList = _charactersData.Values.ToList();
            ResetUniquePool();

            IsInitiated = true;

            return UniTask.CompletedTask;
        }

        public async UniTask<Character> CreateRandomUniqueCharacter(int countAbilities, int countModifications)
        {
            if (_charactersList == null || _charactersList.Count == 0)
                throw new InvalidOperationException("Нет доступных персонажей для создания.");

            if (_remainingIndices.Count == 0)
            {
                ResetUniquePool();
            }

            int lastIdx = _remainingIndices.Count - 1;
            int selectedIndex = _remainingIndices[lastIdx];
            _remainingIndices.RemoveAt(lastIdx);

            return await CreateCharacter(selectedIndex, countAbilities, countModifications);
        }

        private async UniTask<Character> CreateCharacter(int indexCharacter, int countAbilities, int countModifications)
        {
            var data = _charactersData.ElementAt(indexCharacter);
            var abilities = await _abilityService.CreateAllAbilitiesByCount(countAbilities);
            var modifications = _modificationService.CreateAllModificationsByCount(countModifications);

            Character character = new Character(data.Value, abilities, modifications);
            await character.Data.LoadSprites();
            return character;
        }

        private void ResetUniquePool()
        {
            if (_charactersList != null && _charactersList.Count > 0)
            {
                _remainingIndices = Enumerable.Range(0, _charactersList.Count).ToList();
                Shuffle(_remainingIndices);
            }
            else
            {
                _remainingIndices = new List<int>();
            }
        }

        private void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}