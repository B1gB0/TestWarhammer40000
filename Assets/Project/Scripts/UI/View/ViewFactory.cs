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
        
        private UIRootView _uiRoot;
        private UIGameplayRootBinder _uiScene;
        private Container _container;

        [Inject]
        private void Construct(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }
        
        public void GetUIRootAndUIScene(UIRootView uiRoot, UIGameplayRootBinder uiScene, Container container)
        {
            _uiRoot = uiRoot;
            _uiScene = uiScene;
            _container = container;

            GameObjectInjector.InjectRecursive(_uiScene.gameObject, _container);
        }

        public async UniTask<CharacterPanel> CreateCharacterPanel()
        {
            var characterPanelTemplate = await _resourceService.Load<GameObject>(CharacterPanelPath);
            characterPanelTemplate = Instantiate(characterPanelTemplate);
            
            CharacterPanel characterPanel = characterPanelTemplate.GetComponent<CharacterPanel>();
            characterPanel.transform.SetParent(_uiScene.transform, false);
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