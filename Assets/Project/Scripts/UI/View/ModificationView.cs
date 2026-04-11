using System.Collections.Generic;
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
        [SerializeField] private List<Sprite> _iconSprites;

        public void SetData()
        {
            
        }
    }
}