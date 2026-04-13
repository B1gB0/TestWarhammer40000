using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.Entity;
using Project.Scripts.Game.Gameplay.Root.View;
using Project.Scripts.Game.GameRoot;
using Project.Scripts.Services;
using Project.Scripts.UI.Panel;
using Project.Scripts.UI.View;
using Project.Scripts.UI.ViewModel;
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

        private List<Character> _characters = new();

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

            await CreateCharacters();

            CharacterPanel characterPanel = await _viewFactory.CreateCharacterPanel();
            CharacterPanelViewModel characterPanelViewModel = new CharacterPanelViewModel(_characters);
            characterPanel.Bind(characterPanelViewModel);

            var exitSceneSignalSubject = new Subject<Unit>();
            _uiScene.Bind(exitSceneSignalSubject);

            var exitToSceneSignal = exitSceneSignalSubject.Select(_ => _exitParameters);

            return exitToSceneSignal;
        }

        private async UniTask CreateCharacters()
        {
            _characters.Add(await _characterService.CreateRandomUniqueCharacter(4, 7));
            _characters.Add(await _characterService.CreateRandomUniqueCharacter(0, 0));
            _characters.Add(await _characterService.CreateRandomUniqueCharacter(16, 11));
            _characters.Add(await _characterService.CreateRandomUniqueCharacter(5, 3));
            _characters.Add(await _characterService.CreateRandomUniqueCharacter(2, 1));
            _characters.Add(await _characterService.CreateRandomUniqueCharacter(1, 1));
        }
    }
}