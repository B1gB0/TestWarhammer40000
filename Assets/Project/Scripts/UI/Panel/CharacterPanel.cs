using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.UI.Panel
{
    public class CharacterPanel : View.View
    {
        [field: SerializeField] public AbilitiesGridView AbilitiesGridView { get; private set; }
        [field: SerializeField] public ModificationsScrollView ModificationsScrollView { get; private set; }
        [field: SerializeField] public PartyView PartyView { get; private set; }
        [field: SerializeField] public CharacterView CharacterView { get; private set; }
    }
}