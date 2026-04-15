using System.Collections.Generic;
using Project.Scripts.Entity;
using Project.Scripts.Game.Constants;
using Project.Scripts.Services;
using Project.Scripts.UI.ViewModel;
using R3;
using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.View
{
    public class ModificationView : View
    {
        private const float ValueForNonActiveColor = 0.6f;
        private const float ValueForHoverClickColor = 1.6f;

        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _modificationTypeName;

        [SerializeField] private Image _icon;
        [SerializeField] private Image _union;
        [SerializeField] private Image _background;

        [SerializeField] private ModificationDragHandler _modificationDragHandler;
        [SerializeField] private List<Sprite> _iconSprites;

        private ModificationViewModel _viewModel;
        private CompositeDisposable _disposables = new();

        private IModificationService _modificationService;
        private IAbilityService _abilityService;

        [Inject]
        private void Construct(IModificationService modificationService, IAbilityService abilityService)
        {
            _modificationService = modificationService;
            _abilityService = abilityService;
        }

        public void Bind(ModificationViewModel viewModel, ViewFactory viewFactory)
        {
            _viewModel = viewModel;
            _disposables.Clear();

            _viewModel.Name
                .Subscribe(name => _nameText.text = name)
                .AddTo(_disposables);

            viewModel.ModificationType
                .Subscribe(type =>
                {
                    _modificationTypeName.text = type.ToString();

                    int index = (int)type;
                    if (index >= 0 && index < _iconSprites.Count)
                        _icon.sprite = _iconSprites[index];
                    else
                        _icon.sprite = null;

                    SetColorOfUnion(viewModel);
                })
                .AddTo(_disposables);

            _viewModel.IsEquipped
                .Subscribe(isEquipped =>
                {
                    if (isEquipped)
                    {
                        _union.color = new Color(
                            _union.color.r * ValueForNonActiveColor,
                            _union.color.g * ValueForNonActiveColor,
                            _union.color.b * ValueForNonActiveColor,
                            _union.color.a
                        );

                        _icon.color = Colors.GetColor(ColorName.ModificationNonActiveColor);
                        _background.color = Colors.GetColor(ColorName.ModificationBackgroundNonActiveColor);
                        _nameText.color = Colors.GetColor(ColorName.ModificationNonActiveColor);
                        _modificationTypeName.color = Colors.GetColor(ColorName.ModificationNonActiveColor);
                    }
                    else
                    {
                        SetColorOfUnion(viewModel);

                        _icon.color = Colors.GetColor(ColorName.DefaultColor);
                        _background.color = Colors.GetColor(ColorName.ModificationBackgroundColor);
                        _nameText.color = Colors.GetColor(ColorName.DefaultColor);
                        _modificationTypeName.color = Colors.GetColor(ColorName.DefaultColor);
                    }
                })
                .AddTo(_disposables);

            _abilityService.HoveredAbility
                .Subscribe(hoveredAbility =>
                {
                    bool compatible = hoveredAbility != null && hoveredAbility.IsCompatible(viewModel);
                    viewModel.IsCompatibleHighlighted.Value = compatible;
                })
                .AddTo(_disposables);

            viewModel.IsCompatibleHighlighted
                .Subscribe(highlighted =>
                {
                    _background.color = Colors
                        .GetColor(
                            highlighted ? ColorName.ModificationHighlightedColor : ColorName.ModificationBackgroundColor);
                })
                .AddTo(_disposables);

            viewModel.IsHoveredClick
                .Subscribe(isHover =>
                {
                    if (viewModel.IsEquipped.Value)
                        return;

                    if (isHover)
                    {
                        _background.color = new Color(
                            _background.color.r * ValueForHoverClickColor,
                            _background.color.g * ValueForHoverClickColor,
                            _background.color.b * ValueForHoverClickColor,
                            _background.color.a
                        );
                    }
                    else
                    {
                        _background.color = Colors.GetColor(ColorName.ModificationBackgroundColor);
                    }
                })
                .AddTo(_disposables);

            _modificationDragHandler.Init(this, viewModel, _modificationService, viewFactory);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void SetColorOfUnion(ModificationViewModel viewModel)
        {
            switch (viewModel.ModificationType.Value)
            {
                case ModificationType.Psyker:
                    _union.color = Colors.GetColor(ColorName.ModificationPsykerColor);
                    break;
                case ModificationType.Dot:
                    _union.color = Colors.GetColor(ColorName.ModificationDotColor);
                    break;
                case ModificationType.Attack:
                    _union.color = Colors.GetColor(ColorName.ModificationAttackColor);
                    break;
                case ModificationType.Buff:
                    _union.color = Colors.GetColor(ColorName.ModificationBuffColor);
                    break;
                case ModificationType.Debuff:
                    _union.color = Colors.GetColor(ColorName.ModificationDebuffColor);
                    break;
            }
        }

        private void OnDestroy() => Dispose();
    }
}