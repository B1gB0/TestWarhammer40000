using Project.Scripts.Entity;
using R3;
using UnityEngine;

namespace Project.Scripts.UI.ViewModel
{
    public class CharacterViewModel
    {
        private readonly ReactiveProperty<Character> _selectedCharacter;
        private readonly CompositeDisposable _disposables = new();

        public ReactiveProperty<Sprite> Portrait { get; }
        public ReactiveProperty<Sprite> SmallPortrait { get; }
        public ReactiveProperty<string> Name { get; }
        public ReactiveProperty<float> Health { get; }
        public ReactiveProperty<float> Armor { get; }
        
        public ReactiveCommand SelectCommand { get; }
        
        public Character Character { get; }

        public CharacterViewModel(Character character, ReactiveProperty<Character> selectedCharacter)
        {
            Character = character;
            
            _selectedCharacter = selectedCharacter;
            
            Portrait = new ReactiveProperty<Sprite>();
            SmallPortrait = new ReactiveProperty<Sprite>();
            Name = new ReactiveProperty<string>();
            Health = new ReactiveProperty<float>();
            Armor = new ReactiveProperty<float>();

            selectedCharacter
                .Subscribe(OnCharacterChanged)
                .AddTo(_disposables);
            
            SelectCommand = new ReactiveCommand();
            SelectCommand.Subscribe(_ => selectedCharacter.Value = character)
                .AddTo(_disposables);
        }

        private void OnCharacterChanged(Character character)
        {
            if (character == null)
            {
                Portrait.Value = null;
                SmallPortrait.Value = null;
                Name.Value = string.Empty;
                Health.Value = 0;
                Armor.Value = 0;
                return;
            }

            Portrait.Value = character.Data.PortraitSprite;
            SmallPortrait.Value = character.Data.SmallPortraitSprite;
            Name.Value = character.Data.Name;
            Health.Value = character.Data.Health;
            Armor.Value = character.Data.Armor;
        }

        public void Dispose() => _disposables.Dispose();
    }
}