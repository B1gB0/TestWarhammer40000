using System.Collections.Generic;
using Project.Scripts.UI.ViewModel;
using R3;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class AbilitiesGridView : View
    {
        private AbilitiesGridViewModel _viewModel;
        private CompositeDisposable _disposables = new();
        private List<AbilityView> _activeViews = new();

        private ViewFactory _viewFactory;
        
        [field: SerializeField] public Transform Content { get; private set; }

        public void Bind(AbilitiesGridViewModel viewModel, ViewFactory viewFactory)
        {
            _viewModel = viewModel;
            _viewFactory = viewFactory;
            _disposables.Clear();
            
            _viewModel.Slots
                .Subscribe(OnSlotsChanged)
                .AddTo(_disposables);
        }

        private async void OnSlotsChanged(IReadOnlyList<AbilityViewModel> abilityViewModels)
        {
            foreach (var view in _activeViews)
                Destroy(view.gameObject);
            _activeViews.Clear();

            if (abilityViewModels == null || abilityViewModels.Count == 0)
                return;
            
            foreach (var abilityViewModel in abilityViewModels)
            {
                AbilityView abilityView = await _viewFactory.CreateAbilityView(Content);
                abilityView.Bind(abilityViewModel);
                _activeViews.Add(abilityView);
            }
        }

        private void Dispose()
        {
            _disposables.Dispose();
            foreach (var view in _activeViews)
                Destroy(view.gameObject);
            _activeViews.Clear();
        }
        
        private void OnDestroy() => Dispose();
    }
}