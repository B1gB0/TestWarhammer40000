using DG.Tweening;
using Project.Scripts.Services;
using Project.Scripts.UI;
using Reflex.Attributes;
using UnityEngine;

namespace Project.Scripts.Game.MainMenu.Root.View
{
    public class MainMenuElements : UI.View.View
    {
        private ITweenAnimationService _tweenAnimationService;

        [Inject]
        public void Construct(ITweenAnimationService tweenAnimationService)
        {
            _tweenAnimationService = tweenAnimationService;
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}