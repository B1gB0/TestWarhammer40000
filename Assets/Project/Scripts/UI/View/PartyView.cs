using System.Collections.Generic;
using Project.Scripts.UI.ViewModel;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class PartyView : View
    {
        [SerializeField] private List<PortraitView> _portraitViews;

        public void Bind(PartyViewModel partyViewModel)
        {
            for (int i = 0; i < _portraitViews.Count && i < partyViewModel.Slots.Count; i++)
            {
                _portraitViews[i].Bind(partyViewModel.Slots[i], partyViewModel.SelectedCharacter);
            }
            for (int i = partyViewModel.Slots.Count; i < _portraitViews.Count; i++)
            {
                _portraitViews[i].gameObject.SetActive(false);
            }
        }
    }
}