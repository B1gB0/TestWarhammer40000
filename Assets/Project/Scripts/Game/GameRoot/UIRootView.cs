using Project.Scripts.Services;
using Project.Scripts.UI.Panel;
using Project.Scripts.UI.StateMachine;
using Project.Scripts.UI.StateMachine.States;
using Reflex.Attributes;
using UnityEngine;

namespace Project.Scripts.Game.GameRoot
{
    public class UIRootView : MonoBehaviour
    {
        [SerializeField] private UISceneContainer _uiSceneContainer;

        [SerializeField] private LoadingPanel _loadingPanel;

        private AudioSoundsService _audioSoundsService;

        public UIStateMachine UIStateMachine { get; private set; }

        [Inject]
        private void Construct(AudioSoundsService audioSoundsService)
        {
            _audioSoundsService = audioSoundsService;
        }

        private void Awake()
        {
            UIStateMachine = new UIStateMachine();
            UIStateMachine.AddState(new LoadingPanelState(_loadingPanel));
        }

        public void ShowLoadingProgress(float progress)
        {
            _loadingPanel.SetProgressText(progress);
        }

        public void AttachSceneUI(GameObject sceneUI)
        {
            ClearSceneUI();

            sceneUI.transform.SetParent(_uiSceneContainer.transform, false);
        }

        private void ClearSceneUI()
        {
            var childCount = _uiSceneContainer.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Destroy(_uiSceneContainer.transform.GetChild(i).gameObject);
            }
        }
    }
}