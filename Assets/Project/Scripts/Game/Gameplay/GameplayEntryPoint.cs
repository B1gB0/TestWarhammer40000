using Cysharp.Threading.Tasks;
using Project.Scripts.Game.Gameplay.Root.View;
using Project.Scripts.Game.GameRoot;
using Project.Scripts.Services;
using Project.Scripts.UI.Panel;
using Project.Scripts.UI.View;
using R3;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Extensions;
using Reflex.Injectors;
using UnityEngine;

namespace Project.Scripts.Game.Gameplay
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        [SerializeField] private UIGameplayRootBinder _sceneUIRootPrefab;
        [SerializeField] private ViewFactory _viewFactory;

        private UIRootView _uiRoot;
        private UIGameplayRootBinder _uiScene;
        private Container _container;
        private GameplayExitParameters _exitParameters;
        
        private IDataBaseService _dataBaseService;
        private ICharacterService _characterService;
        private IAbilityService _abilityService;
        private IModificationService _modificationService;

        [Inject]
        private void Construct(
            IDataBaseService dataBaseService, 
            ICharacterService characterService, 
            IAbilityService abilityService, 
            IModificationService modificationService)
        {
            _dataBaseService = dataBaseService;
            _characterService = characterService;
            _abilityService = abilityService;
            _modificationService = modificationService;
        }

        public async UniTask<Observable<GameplayExitParameters>> Run(
            UIRootView uiRoot,
            GameplayEnterParameters enterParameters = null)
        {
            _container = gameObject.scene.GetSceneContainer();

            _uiRoot = uiRoot;

            await _dataBaseService.Init();
            await _characterService.Init();
            await _abilityService.Init();
            await _modificationService.Init();
            
            // await _particleEffectsService.Init();

            _uiScene = Instantiate(_sceneUIRootPrefab);

            uiRoot.AttachSceneUI(_uiScene.gameObject);

            _viewFactory.GetUIRootAndUIScene(uiRoot, _uiScene, _container);

            _uiScene.GetUIStateMachine(uiRoot.UIStateMachine);

            CharacterPanel characterPanel = await _viewFactory.CreateCharacterPanel();


            var exitSceneSignalSubject = new Subject<Unit>();
            _uiScene.Bind(exitSceneSignalSubject);

            var exitToSceneSignal = exitSceneSignalSubject.Select(_ => _exitParameters);

            return exitToSceneSignal;
        }
    }
}