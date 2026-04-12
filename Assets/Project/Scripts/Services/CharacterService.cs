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

            IsInitiated = true;

            return UniTask.CompletedTask;
        }

        public Character CreateCharacter(int indexCharacter, int countAbilities, int countModifications)
        {
            var data = _charactersData.ElementAt(indexCharacter);
            var abilities = _abilityService.CreateAllAbilitiesByCount(countAbilities);
            var modifications = _modificationService.CreateAllModificationsByCount(countModifications);

            Character character = new Character(data.Value, abilities, modifications);
            return character;
        }
    }
}