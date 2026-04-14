using System;
using System.Collections.Generic;
using Project.Scripts.UI.ViewModel;
using R3;
using UnityEngine;
using UnityEngine.UI;
using CompositeDisposable = R3.CompositeDisposable;

namespace Project.Scripts.UI.View
{
    public class ModificationsScrollView : View, IDisposable
    {
        private ModificationsScrollViewModel _viewModel;
        private ViewFactory _viewFactory;
        private CompositeDisposable _disposables = new();
        private List<ModificationView> _activeViews = new();

        [field: SerializeField] public Transform Content { get; private set; }

        public void Bind(ModificationsScrollViewModel viewModel, ViewFactory viewFactory)
        {
            _viewModel = viewModel;
            _viewFactory = viewFactory;
            _disposables.Clear();

            _viewModel.FreeModifications
                .Subscribe(OnFreeModificationsChanged)
                .AddTo(_disposables);
        }

        private async void OnFreeModificationsChanged(
            IReadOnlyList<ModificationViewModel> modificationViewModels)
        {
            foreach (var view in _activeViews)
                Destroy(view.gameObject);
            _activeViews.Clear();

            if (modificationViewModels == null || modificationViewModels.Count == 0)
                return;

            foreach (var modificationViewModel in modificationViewModels)
            {
                ModificationView modificationView = await _viewFactory.CreateModificationView(Content);
                modificationView.Bind(modificationViewModel, _viewFactory);
                _activeViews.Add(modificationView);
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