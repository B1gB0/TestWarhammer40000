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
    }
}