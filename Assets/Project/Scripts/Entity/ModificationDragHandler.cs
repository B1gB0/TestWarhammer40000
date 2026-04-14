using Project.Scripts.Services;
using Project.Scripts.UI.View;
using Project.Scripts.UI.ViewModel;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts.Entity
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ModificationDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _draggedAlpha = 0.6f;

        private ModificationView _modificationView;
        private ModificationViewModel _viewModel;
        private RectTransform _rectTransform;
        private Vector2 _originalPosition;

        private IModificationService _modificationService;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Init(
            ModificationView parentView,
            ModificationViewModel viewModel,
            IModificationService modificationService)
        {
            _modificationView = parentView;
            _viewModel = viewModel;
            _modificationService = modificationService;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_viewModel.IsEquipped.CurrentValue)
                return; // Нельзя перетаскивать уже прикреплённый модификатор

            _originalPosition = _rectTransform.anchoredPosition;
            _canvasGroup.alpha = _draggedAlpha;
            _canvasGroup.blocksRaycasts = false;

            _modificationService.CurrentDraggedModification.Value = _viewModel;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_viewModel.IsEquipped.CurrentValue) return;

            // Перемещаем объект за мышью (в координатах канваса)
            _rectTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_viewModel.IsEquipped.CurrentValue) return;

            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
            _rectTransform.anchoredPosition = _originalPosition;

            // Сбрасываем перетаскиваемый модификатор
            if (_modificationService.CurrentDraggedModification.Value == _viewModel)
                _modificationService.CurrentDraggedModification.Value = null;
        }
    }
}