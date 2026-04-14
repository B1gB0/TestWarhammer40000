using System;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Entity;
using R3;

namespace Project.Scripts.UI.ViewModel
{
    public class ModificationsScrollViewModel
    {
        private readonly ReactiveProperty<Character> _selectedCharacter;
        private readonly CompositeDisposable _disposables = new();
        
        // public ReactiveProperty<IReadOnlyList<ModificationViewModel>> FreeModifications { get; }
        public ReactiveProperty<IReadOnlyList<ModificationViewModel>> AllModificationViewModels { get; }
        
        private List<ModificationViewModel> _allModificationViews;

        public ModificationsScrollViewModel(ReactiveProperty<Character> selectedCharacter)
        {
            _selectedCharacter = selectedCharacter;
            AllModificationViewModels =
                new ReactiveProperty<IReadOnlyList<ModificationViewModel>>(Array.Empty<ModificationViewModel>());

            selectedCharacter
                .Subscribe(OnCharacterChanged)
                .AddTo(_disposables);
        }

        private void OnCharacterChanged(Character character)
        {
            if (_allModificationViews != null)
            {
                foreach (var slot in _allModificationViews)
                    slot.Dispose();
            }
            
            _allModificationViews = character.Modifications
                .Select(mod => new ModificationViewModel(mod))
                .ToList();
            
            foreach (var slot in _allModificationViews)
            {
                slot.IsEquipped
                    .Subscribe(_ => UpdateFreeList())
                    .AddTo(_disposables);
            }

            UpdateFreeList();
        }

        private void UpdateFreeList()
        {
            var sorted = _allModificationViews
                .OrderBy(slot => slot.IsEquipped.Value)
                .ToList();
            
            AllModificationViewModels.Value = sorted;
        }

        public void Dispose()
        {
            _disposables.Dispose();
            AllModificationViewModels.Dispose();
            if (_allModificationViews != null)
            {
                foreach (var slot in _allModificationViews)
                    slot.Dispose();
            }
        }
    }
}