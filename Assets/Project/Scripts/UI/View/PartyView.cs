using System.Collections.Generic;
using Project.Scripts.Services;
using Project.Scripts.UI.ViewModel;
using Reflex.Attributes;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class PartyView : View
    {
        [SerializeField] private List<PortraitView> _portraitViews;

        private AudioSoundsService _audioSoundsService;

        [Inject]
        private void Construct(AudioSoundsService audioSoundsService)
        {
            _audioSoundsService = audioSoundsService;
        }

        public void Bind(PartyViewModel partyViewModel)
        {
            for (int i = 0; i < _portraitViews.Count && i < partyViewModel.Slots.Count; i++)
            {
                _portraitViews[i].Bind(
                    partyViewModel.Slots[i],
                    partyViewModel.SelectedCharacter,
                    partyViewModel.Characters[i],
                    _audioSoundsService);
            }
            for (int i = partyViewModel.Slots.Count; i < _portraitViews.Count; i++)
            {
                _portraitViews[i].gameObject.SetActive(false);
            }
        }
    }
}