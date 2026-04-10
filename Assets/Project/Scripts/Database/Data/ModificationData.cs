using System;
using Project.Scripts.Modifications;
using UnityEngine;

namespace Project.Scripts.Database.Data
{
    [Serializable]
    public class ModificationData
    {
        [SerializeField] private string _name;
        [SerializeField] private ModificationType _type;

        public string Name => _name;
        public ModificationType Type => _type;
    }
}