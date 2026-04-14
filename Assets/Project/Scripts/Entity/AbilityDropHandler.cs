using Project.Scripts.Services;
using Project.Scripts.UI.View;
using Project.Scripts.UI.ViewModel;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts.Entity
{
    public class AbilityDropHandler : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private AbilityView _abilityView;
        private AbilityViewModel _viewModel;

        private IModificationService _modificationService;

        public void Init(
            AbilityView parentView,
            AbilityViewModel viewModel,
            IModificationService modificationService)
        {
            _abilityView = parentView;
            _viewModel = viewModel;
            _modificationService = modificationService;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var draggedMod = _modificationService.CurrentDraggedModification.Value;
            if (draggedMod == null)
                return;

            // Пытаемся прикрепить
            if (_viewModel.TryAttachModification(draggedMod))
            {
                // Успешно прикреплено — сбрасываем перетаскиваемый модификатор
                _modificationService.CurrentDraggedModification.Value = null;
                // ViewModel модификатора уже обновила своё состояние (IsEquipped = true)
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Подсвечиваем, если перетаскивается совместимый мод
            var draggedMod = _modificationService.CurrentDraggedModification.Value;
            if (draggedMod != null && _viewModel.IsCompatible(draggedMod))
            {
                _viewModel.IsCompatibleHighlighted.Value = true;
            }
            _modificationService.HoveredAbility.Value = _viewModel;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _viewModel.IsCompatibleHighlighted.Value = false;
            if (_modificationService.HoveredAbility.Value == _viewModel)
                _modificationService.HoveredAbility.Value = null;
        }
    }
}