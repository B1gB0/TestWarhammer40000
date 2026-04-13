using System;
using System.Collections.Generic;
using Project.Scripts.UI.ViewModel;
using R3;
using UnityEngine;
using CompositeDisposable = R3.CompositeDisposable;

namespace Project.Scripts.UI.View
{
    public class ModificationsScrollView : View, IDisposable
    {
        [SerializeField] private ModificationView _modificationViewPrefab;

        private ModificationsScrollViewModel _viewModel;
        private CompositeDisposable _disposables = new();
        private List<ModificationView> _activeViews = new();
        
        [field: SerializeField] public Transform Content { get; private set; }

        public void Bind(ModificationsScrollViewModel viewModel)
        {
            _viewModel = viewModel;
            _disposables.Clear();

            _viewModel.FreeModifications
                .Subscribe(OnFreeModificationsChanged)
                .AddTo(_disposables);
        }

        private void OnFreeModificationsChanged(IReadOnlyList<ModificationViewModel> modificationViewModels)
        {
            foreach (var view in _activeViews)
                Destroy(view.gameObject);
            _activeViews.Clear();

            if (modificationViewModels == null || modificationViewModels.Count == 0)
                return;

            foreach (var modificationViewModel in modificationViewModels)
            {
                var view = Instantiate(_modificationViewPrefab, Content);
                view.Bind(modificationViewModel);
                _activeViews.Add(view);
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();
            foreach (var view in _activeViews)
                Destroy(view.gameObject);
            _activeViews.Clear();
        }

        private void OnDestroy() => Dispose();
    }
}