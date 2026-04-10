using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Scripts.Modifications;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.Scripts.Database.Data
{
    [Serializable]
    public class AbilityData
    {
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private string _compatibleTypes;

        public string Id => _id;
        public string Name => _name;
        
        public List<ModificationType> CompatibleTypes
        {
            get
            {
                return _compatibleTypes?.Split(';')
                    .Select(s => Enum.Parse<ModificationType>(s.Trim()))
                    .ToList() ?? new List<ModificationType>();
            }
        }
        
        public Sprite IconSprite { get; private set; }
        
        public async UniTask LoadIconAsync()
        {
            if (string.IsNullOrEmpty(_id))
            {
                Debug.LogWarning($"У способности {Name} не указан адрес иконки");
                return;
            }
            var handle = Addressables.LoadAssetAsync<Sprite>(_id);
            IconSprite = await handle;
        }
        
        public void ReleaseIcon()
        {
            if (IconSprite != null)
                Addressables.Release(IconSprite);
        }
    }
}