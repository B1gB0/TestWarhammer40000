using Project.Scripts.Services;
using Project.Scripts.UI.ViewModel;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts.Entity
{
    public class AbilityDropHandler : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private AbilityViewModel _viewModel;

        private IModificationService _modificationService;

        public void Init(
            AbilityViewModel viewModel,
            IModificationService modificationService)
        {
            _viewModel = viewModel;
            _modificationService = modificationService;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var draggedMod = _modificationService.CurrentDraggedModification.Value;
            if (draggedMod == null)
                return;
            
            if (_viewModel.TryAttachModification(draggedMod))
            {
                _modificationService.CurrentDraggedModification.Value = null;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
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