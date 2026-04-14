using Cysharp.Threading.Tasks;
using Project.Scripts.Game.Gameplay.Root.View;
using Project.Scripts.Game.GameRoot;
using Project.Scripts.Services;
using Project.Scripts.UI.Panel;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class ViewFactory : MonoBehaviour
    {
        private const string CharacterPanelPath = "CharacterPanel";
        private const string ModificationViewPath = "ModificationView";
        private const string AbilityViewPath = "AbilityView";

        private IResourceService _resourceService;
        
        private Container _container;

        public UIRootView UIRoot { get; private set; }
        public UIGameplayRootBinder UIScene { get; private set; }

        [Inject]
        private void Construct(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }
        
        public void GetUIRootAndUIScene(UIRootView uiRoot, UIGameplayRootBinder uiScene, Container container)
        {
            UIRoot = uiRoot;
            UIScene = uiScene;
            _container = container;

            GameObjectInjector.InjectRecursive(UIScene.gameObject, _container);
        }

        public async UniTask<CharacterPanel> CreateCharacterPanel()
        {
            var characterPanelTemplate = await _resourceService.Load<GameObject>(CharacterPanelPath);
            characterPanelTemplate = Instantiate(characterPanelTemplate);
            
            CharacterPanel characterPanel = characterPanelTemplate.GetComponent<CharacterPanel>();
            characterPanel.transform.SetParent(UIScene.transform, false);
            GameObjectInjector.InjectRecursive(characterPanel.gameObject, _container);
            return characterPanel;
        }
        
        public async UniTask<ModificationView> CreateModificationView(Transform content)
        {
            var modificationViewTemplate = await _resourceService.Load<GameObject>(ModificationViewPath);
            modificationViewTemplate = Instantiate(modificationViewTemplate);
            
            ModificationView modificationView = modificationViewTemplate.GetComponent<ModificationView>();
            modificationView.transform.SetParent(content, false);
            GameObjectInjector.InjectSingle(modificationView.gameObject, _container);
            return modificationView;
        }
        
        public async UniTask<AbilityView> CreateAbilityView(Transform content)
        {
            var abilityViewTemplate = await _resourceService.Load<GameObject>(AbilityViewPath);
            abilityViewTemplate = Instantiate(abilityViewTemplate);
            
            AbilityView abilityView = abilityViewTemplate.GetComponent<AbilityView>();
            abilityView.transform.SetParent(content, false);
            GameObjectInjector.InjectSingle(abilityView.gameObject, _container);
            return abilityView;
        }
    }
}