using Project.Scripts.Services;
using Project.Scripts.UI.View;
using Project.Scripts.UI.ViewModel;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts.Entity
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ModificationDragHandler
        : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _draggedAlpha = 0.6f;

        private ModificationView _modificationView;
        private ModificationViewModel _viewModel;
        private RectTransform _rectTransform;
        private Vector2 _originalPosition;

        private GameObject _ghost;
        private RectTransform _ghostRect;

        private IModificationService _modificationService;
        private ViewFactory _viewFactory;
        
        private bool _isDragging;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Init(
            ModificationView view,
            ModificationViewModel viewModel,
            IModificationService modificationService,
            ViewFactory viewFactory)
        {
            _modificationView = view;
            _viewModel = viewModel;
            _modificationService = modificationService;
            _viewFactory = viewFactory;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_viewModel.IsEquipped.CurrentValue)
                return;
            
            _isDragging = true;

            _ghost = Instantiate(_modificationView.gameObject, _viewFactory.UIRoot.UICanvas.transform);
            _ghost.name = $"{gameObject.name}_Ghost";
            _ghostRect = _ghost.GetComponent<RectTransform>();

            Destroy(_ghost.GetComponent<ModificationDragHandler>());
            Destroy(_ghost.GetComponent<ModificationView>());

            var ghostCanvasGroup = _ghost.GetComponent<CanvasGroup>();
            if (ghostCanvasGroup != null)
            {
                ghostCanvasGroup.alpha = _draggedAlpha;
                ghostCanvasGroup.blocksRaycasts = false;
            }

            _modificationService.RegisterDragHandler(this);
            _modificationService.CurrentDraggedModification.Value = _viewModel;

            _originalPosition = _rectTransform.anchoredPosition;
            _canvasGroup.alpha = _draggedAlpha;
            _canvasGroup.blocksRaycasts = false;

            SetGhostPosition(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_viewModel.IsEquipped.CurrentValue)
                return;

            SetGhostPosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_viewModel.IsEquipped.CurrentValue) return;

            CleanupGhost();

            _modificationService.UnregisterDragHandler(this);
            _modificationService.CurrentDraggedModification.Value = null;
            
            _isDragging = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _modificationService.HoveredModification.Value = _viewModel;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isDragging)
                return;
            
            if (_modificationService.HoveredModification.Value == _viewModel)
                _modificationService.HoveredModification.Value = null;
        }

        public void CleanupGhost()
        {
            if (_ghost != null)
            {
                Destroy(_ghost);
                _ghost = null;
            }

            if (this != null && _canvasGroup != null)
            {
                _canvasGroup.alpha = 1f;
                _canvasGroup.blocksRaycasts = true;
                _rectTransform.anchoredPosition = _originalPosition;
            }
        }

        private void SetGhostPosition(PointerEventData eventData)
        {
            if (_ghostRect == null) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _viewFactory.UIRoot.UICanvas.transform as RectTransform,
                eventData.position,
                _viewFactory.UIRoot.UICanvas.worldCamera,
                out Vector2 localPoint);
            _ghostRect.localPosition = localPoint;
        }
    }
}