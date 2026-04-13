using Project.Scripts.Entity;
using Project.Scripts.UI.ViewModel;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.View
{
    public class PortraitView : View
    {
        [SerializeField] private Image _portrait;
        [SerializeField] private Button _button;
        [SerializeField] private Image _selector;
        
        private CharacterViewModel _viewModel;
        private CompositeDisposable _disposables = new();

        public void Bind(CharacterViewModel viewModel, ReactiveProperty<Character> selectedCharacter)
        {
            _viewModel = viewModel;
            _disposables.Clear();
            
            _portrait.sprite = viewModel.SmallPortrait.CurrentValue;
            _button.onClick.AddListener(OnButtonClick);
            
            selectedCharacter
                .Subscribe(character => _selector.gameObject.SetActive(character == viewModel.Character))
                .AddTo(_disposables);
        }

        private void OnButtonClick()
        {
            _viewModel?.SelectCommand.Execute(Unit.Default);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
            _disposables.Dispose();
        }
    }
}