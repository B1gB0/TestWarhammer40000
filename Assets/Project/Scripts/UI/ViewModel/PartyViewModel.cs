using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Entity;
using R3;

namespace Project.Scripts.UI.ViewModel
{
    public class PartyViewModel
    {
        public IReadOnlyList<CharacterViewModel> Slots { get; }
        public ReactiveProperty<Character> SelectedCharacter { get; }

        public PartyViewModel(List<Character> characters, ReactiveProperty<Character> selectedCharacter)
        {
            SelectedCharacter = selectedCharacter;
            Slots = characters.Select(character => new CharacterViewModel(character, selectedCharacter)).ToList();
        }
    }
}