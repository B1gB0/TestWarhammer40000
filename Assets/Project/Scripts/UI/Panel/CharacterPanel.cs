using Project.Scripts.UI.View;
using Project.Scripts.UI.ViewModel;
using UnityEngine;

namespace Project.Scripts.UI.Panel
{
    public class CharacterPanel : View.View
    {
        [SerializeField] private PartyView _partyView;
        [SerializeField] private CharacterView _characterView;
        [SerializeField] private AbilitiesGridView _abilitiesGridView;
        [SerializeField] private ModificationsScrollView _modificationsScrollView;

        private CharacterPanelViewModel _viewModel;

        public void Bind(CharacterPanelViewModel viewModel)
        {
            _viewModel = viewModel;
            _partyView.Bind(viewModel.PartyViewModel);
            _characterView.Bind(viewModel.CharacterViewModel);
            _abilitiesGridView.Bind(viewModel.AbilitiesViewModel);
            _modificationsScrollView.Bind(viewModel.ModificationsViewModel);
        }
    }
}