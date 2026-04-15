using Cysharp.Threading.Tasks;
using Project.Scripts.Audio.Sounds;
using Project.Scripts.Entity;
using Project.Scripts.Services;
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

        private AudioSoundsService _audioSoundsService;

        public void Bind(
            CharacterViewModel viewModel,
            ReactiveProperty<Character> selectedCharacter,
            Character character,
            AudioSoundsService audioSoundsService)
        {
            _viewModel = viewModel;
            _audioSoundsService = audioSoundsService;
            _disposables.Clear();
            
            _portrait.sprite = character.Data.SmallPortraitSprite;
            _button.onClick.AddListener(OnButtonClick);
            
            selectedCharacter
                .Subscribe(character => _selector.gameObject.SetActive(character == viewModel.Character))
                .AddTo(_disposables);
        }

        private void OnButtonClick()
        {
            _audioSoundsService.PlaySound(SoundsType.UIButton).Forget();
            _viewModel?.SelectCommand.Execute(Unit.Default);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
            _disposables.Dispose();
        }
    }
}