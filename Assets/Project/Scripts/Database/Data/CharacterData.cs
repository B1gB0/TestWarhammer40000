using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.Scripts.Database.Data
{
    [Serializable]
    public class CharacterData
    {
        [SerializeField] private string _name;
        [SerializeField] private float _health;
        [SerializeField] private float _armor;
        [SerializeField] private string _smallPortraitId;
        
        public string Name => _name;
        public float Health => _health;
        public float Armor => _armor;
        public string SmallPortraitId => _smallPortraitId;
        public Sprite PortraitSprite { get; private set; }
        public Sprite SmallPortraitSprite { get; private set; }

        public async UniTask LoadSprites()
        {
            var portraitOperation = Addressables.LoadAssetAsync<Sprite>(_name);
            var smallPortraitOperation = Addressables.LoadAssetAsync<Sprite>(_smallPortraitId);
            
            PortraitSprite = await portraitOperation;
            SmallPortraitSprite = await smallPortraitOperation;
        }
    }
}