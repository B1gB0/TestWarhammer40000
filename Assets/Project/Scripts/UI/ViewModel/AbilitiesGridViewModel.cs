using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Entity;
using R3;

namespace Project.Scripts.UI.ViewModel
{
    public class AbilitiesGridViewModel
    {
        public ReactiveProperty<IReadOnlyList<AbilityViewModel>> Slots { get; }
        private readonly CompositeDisposable _disposables = new();

        public AbilitiesGridViewModel(ReactiveProperty<Character> selectedCharacter)
        {
            Slots = new ReactiveProperty<IReadOnlyList<AbilityViewModel>>();
            selectedCharacter
                .Subscribe(OnCharacterChanged)
                .AddTo(_disposables);
        }

        private void OnCharacterChanged(Character character)
        {
            var newSlots = character.Abilities
                .Select(ability => new AbilityViewModel(ability))
                .ToList();
            Slots.Value = newSlots;
        }

        public void Dispose() => _disposables.Dispose();
    }
}