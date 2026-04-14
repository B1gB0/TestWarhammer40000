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
            ModificationViewModel viewModel,
            IModificationService modificationService)
        {
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
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            // Подсвечиваем, если перетаскивается совместимый мод
            // var draggedMod = _modificationService.CurrentDraggedModification.Value;
            // if (draggedMod != null && _viewModel.IsCompatible(draggedMod))
            // {
            //     _viewModel.IsCompatibleHighlighted.Value = true;
            // }
            _modificationService.HoveredModification.Value = _viewModel;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // _viewModel.IsCompatibleHighlighted.Value = false;
            if (_modificationService.HoveredModification.Value == _viewModel)
                _modificationService.HoveredModification.Value = null;
        }
    }
}