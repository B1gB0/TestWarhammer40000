using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.View
{
    public class AbilityView : View
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _iconOfModification;
        [SerializeField] private Image _backgroundOfModification;
        [SerializeField] private List<Sprite> _iconSpritesOfModification;

        public void Set()
        {
            
        }
    }
}