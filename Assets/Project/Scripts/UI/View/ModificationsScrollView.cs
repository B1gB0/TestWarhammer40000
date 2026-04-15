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
        [SerializeField] private GameObject _noModificationsTitle;

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

            _viewModel.AllModificationViewModels
                .Subscribe(OnAllModificationsChanged)
                .AddTo(_disposables);
        }

        private async void OnAllModificationsChanged(
            IReadOnlyList<ModificationViewModel> modificationViewModels)
        {
            foreach (var view in _activeViews)
                Destroy(view.gameObject);
            _activeViews.Clear();

            if (modificationViewModels == null || modificationViewModels.Count == 0)
            {
                _noModificationsTitle.gameObject.SetActive(true);
                return;
            }

            _noModificationsTitle.gameObject.SetActive(false);

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