using System.Collections.Generic;
using Project.Scripts.Entity;
using Project.Scripts.Game.Constants;
using Project.Scripts.UI.ViewModel;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.View
{
    public class ModificationView : View
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _modificationTypeName;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _union;
        [SerializeField] private List<Sprite> _iconSprites;

        private ModificationViewModel _viewModel;
        private CompositeDisposable _disposables = new();

        public void Bind(ModificationViewModel viewModel)
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
                })
                .AddTo(_disposables);
            
            _viewModel.IsCompatibleHighlighted
                .Subscribe(highlighted => GetComponent<Image>().color = highlighted ? Color.green : Color.white)
                .AddTo(_disposables);
            
            _viewModel.IsEquipped
                .Subscribe(isEquipped => gameObject.SetActive(!isEquipped))
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void OnDestroy() => Dispose();
    }
}