using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class ModificationsScrollView : View
    {
        [SerializeField] private List<ModificationView> _modificationViews;
        
        [field: SerializeField] public Transform Content { get; private set; }
    }
}