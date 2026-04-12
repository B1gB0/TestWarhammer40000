using System;
using System.Collections.Generic;
using Project.Scripts.Database.Data;
using Project.Scripts.Entity;
using R3;

namespace Project.Scripts.UI.ViewModel
{
    public class PartyViewModel : IDisposable
    {
        // Реактивное свойство. При изменении .Value автоматически уведомляет подписчиков.
        public ReactiveProperty<Character> SelectedCharacter { get; }
    
        // Список всех доступных персонажей (для отрисовки портретов в пати)
        public IReadOnlyList<Character> PartyMembers { get; }

        public PartyViewModel(List<CharacterData> dataList)
        {
            // Инициализируем runtime состояния для каждого персонажа
            // ...
            SelectedCharacter = new ReactiveProperty<Character>(PartyMembers[0]);
        }

        public void SelectCharacter(Character state)
        {
            SelectedCharacter.Value = state;
        }
        
        public void Dispose()
        {
        }
    }
}