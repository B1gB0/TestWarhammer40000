using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class AbilitiesGridView : View
    {
        [SerializeField] private List<AbilityView> _abilityViews;
        
        [field: SerializeField] public Transform Content { get; private set; }
    }
}