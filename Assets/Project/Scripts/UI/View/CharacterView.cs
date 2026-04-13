using Project.Scripts.UI.ViewModel;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.View
{
    public class CharacterView : View
    {
        [SerializeField] private Image _portrait;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _health;
        [SerializeField] private TMP_Text _armor;

        private CharacterViewModel _viewModel;
        private CompositeDisposable _disposables = new();

        public void Bind(CharacterViewModel characterViewModel)
        {
            _viewModel = characterViewModel;
            _disposables.Clear();

            characterViewModel.Portrait
                .Subscribe(sprite => _portrait.sprite = sprite)
                .AddTo(_disposables);
            characterViewModel.Name
                .Subscribe(name => _name.text = name)
                .AddTo(_disposables);
            characterViewModel.Health
                .Subscribe(hp => _health.text = hp.ToString())
                .AddTo(_disposables);
            characterViewModel.Armor
                .Subscribe(armor => _armor.text = armor.ToString())
                .AddTo(_disposables);
        }

        private void Dispose() => _disposables.Dispose();
        private void OnDestroy() => Dispose();
    }
}