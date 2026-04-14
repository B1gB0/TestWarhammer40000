using System;
using System.Collections.Generic;
using Project.Scripts.Entity;
using Project.Scripts.Services;
using Project.Scripts.UI.ViewModel;
using R3;
using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.View
{
    public class AbilityView : View, IDisposable
    {
        [SerializeField] private TMP_Text _nameText;

        [SerializeField] private Image _icon;
        [SerializeField] private Image _iconOfModification;
        [SerializeField] private Image _backgroundOfModification;
        [SerializeField] private Image _outline;

        [SerializeField] private AbilityDropHandler _abilityDropHandler;
        
        [SerializeField] private List<Sprite> _iconSpritesOfModification;
        [SerializeField] private List<Sprite> _iconSpritesOfUnions;

        private AbilityViewModel _viewModel;
        private CompositeDisposable _disposables = new();
        private SerialDisposable _modTypeSubscription = new SerialDisposable();

        private IModificationService _modificationService;

        [Inject]
        private void Construct(IModificationService modificationService)
        {
            _modificationService = modificationService;
        }

        public void Bind(AbilityViewModel viewModel)
        {
            _viewModel = viewModel;

            _disposables.Clear();

            _viewModel.Name
                .Subscribe(name => _nameText.text = name)
                .AddTo(_disposables);

            _viewModel.Icon
                .Subscribe(sprite => _icon.sprite = sprite)
                .AddTo(_disposables);

            _viewModel.AttachedModification
                .Subscribe(OnAttachedModificationChanged)
                .AddTo(_disposables);

            _viewModel.IsCompatibleHighlighted
                .Subscribe(highlighted => _outline.gameObject.SetActive(highlighted))
                .AddTo(_disposables);

            _viewModel.HasModification
                .Subscribe(hasModification => _iconOfModification.gameObject.SetActive(hasModification))
                .AddTo(_disposables);

            _abilityDropHandler.Init(viewModel, _modificationService);

            _modificationService.HoveredModification
                .Subscribe(hoveredModification =>
                {
                    bool compatible = hoveredModification != null && viewModel.IsCompatible(hoveredModification);
                    viewModel.IsCompatibleHighlighted.Value = compatible;
                })
                .AddTo(_disposables);
        }

        private void OnAttachedModificationChanged(ModificationViewModel modificationViewModel)
        {
            _modTypeSubscription.Disposable = null;

            if (modificationViewModel != null)
            {
                SetIconByType(modificationViewModel.ModificationType.CurrentValue);
                SetUnionByType(modificationViewModel.ModificationType.CurrentValue);

                _modTypeSubscription.Disposable = modificationViewModel.ModificationType.Subscribe(SetIconByType);
                _modTypeSubscription.Disposable = modificationViewModel.ModificationType.Subscribe(SetUnionByType);
            }
            else
            {
                _iconOfModification.sprite = null;
            }
        }

        private void SetIconByType(ModificationType type)
        {
            int index = (int)type;
            if (index >= 0 && index < _iconSpritesOfModification.Count)
                _iconOfModification.sprite = _iconSpritesOfModification[index];
            else
                _iconOfModification.sprite = null;
        }

        private void SetUnionByType(ModificationType type)
        {
            int index = (int)type;
            if (index >= 0 && index < _iconSpritesOfUnions.Count)
                _backgroundOfModification.sprite = _iconSpritesOfUnions[index];
            else
                _backgroundOfModification.sprite = null;
        }

        public void Dispose()
        {
            _disposables.Dispose();
            _modTypeSubscription.Dispose();
        }

        private void OnDestroy() => Dispose();
    }
}