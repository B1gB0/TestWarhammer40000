using System.Collections.Generic;
using Project.Scripts.UI.ViewModel;
using R3;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class AbilitiesGridView : View
    {
        [SerializeField] private AbilityView _abilityViewPrefab;

        private AbilitiesGridViewModel _viewModel;
        private CompositeDisposable _disposables = new();
        private List<AbilityView> _activeViews = new();
        
        [field: SerializeField] public Transform Content { get; private set; }

        public void Bind(AbilitiesGridViewModel viewModel)
        {
            _viewModel = viewModel;
            _disposables.Clear();
            
            _viewModel.Slots
                .Subscribe(OnSlotsChanged)
                .AddTo(_disposables);
        }

        private void OnSlotsChanged(IReadOnlyList<AbilityViewModel> abilityViewModels)
        {
            foreach (var view in _activeViews)
                Destroy(view.gameObject);
            _activeViews.Clear();

            if (abilityViewModels == null || abilityViewModels.Count == 0)
                return;
            
            foreach (var abilityViewModel in abilityViewModels)
            {
                var view = Instantiate(_abilityViewPrefab, Content);
                view.Bind(abilityViewModel);
                _activeViews.Add(view);
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