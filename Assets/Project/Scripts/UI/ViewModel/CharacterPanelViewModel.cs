using System.Collections.Generic;
using Project.Scripts.Entity;
using R3;

namespace Project.Scripts.UI.ViewModel
{
    public class CharacterPanelViewModel
    {
        public IReadOnlyList<Character> Characters { get; }

        public ReactiveProperty<Character> SelectedCharacter { get; }

        public PartyViewModel PartyViewModel { get; }
        public CharacterViewModel CharacterViewModel { get; }
        public AbilitiesGridViewModel AbilitiesViewModel { get; }
        public ModificationsScrollViewModel ModificationsViewModel { get; }
        
        public CharacterPanelViewModel(List<Character> characters)
        {
            Characters = characters;
            SelectedCharacter = new ReactiveProperty<Character>(characters[0]);

            PartyViewModel = new PartyViewModel(characters, SelectedCharacter);
            CharacterViewModel = new CharacterViewModel(SelectedCharacter.Value, SelectedCharacter);
            AbilitiesViewModel = new AbilitiesGridViewModel(SelectedCharacter);
            ModificationsViewModel = new ModificationsScrollViewModel(SelectedCharacter);
        }
    }
}