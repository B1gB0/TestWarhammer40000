using System;
using UnityEngine;

namespace Project.Scripts.Database.Data
{
    [Serializable]
    public class CharacterData
    {
        [SerializeField] private string _name;
        [SerializeField] private float _health;
        [SerializeField] private float _armor;
        
        public string Name => _name;
        public float Health => _health;
        public float Armor => _armor;
    }
}