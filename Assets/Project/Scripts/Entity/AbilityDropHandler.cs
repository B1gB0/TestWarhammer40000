using Project.Scripts.Services;
using Project.Scripts.UI.ViewModel;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts.Entity
{
    public class AbilityDropHandler :
        MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private AbilityViewModel _viewModel;

        private IModificationService _modificationService;
        private IAbilityService _abilityService;

        public void Init(
            AbilityViewModel viewModel,
            IModificationService modificationService,
            IAbilityService abilityService)
        {
            _viewModel = viewModel;
            _modificationService = modificationService;
            _abilityService = abilityService;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _viewModel?.DetachModification();
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            var draggedMod = _modificationService.CurrentDraggedModification.Value;
            if (draggedMod == null) return;

            if (_viewModel.TryAttachModification(draggedMod))
            {
                _viewModel.IsCompatibleHighlighted.Value = false;
                _abilityService.HoveredAbility.Value = null;
                _modificationService.HoveredModification.Value = null;
                
                _modificationService.ForceEndDrag();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var draggedMod = _modificationService.CurrentDraggedModification.Value;
            if (draggedMod != null && _viewModel.IsCompatible(draggedMod))
            {
                _viewModel.IsCompatibleHighlighted.Value = true;
            }
            else
            {
                _viewModel.IsHoveredClick.Value = true;
            }

            _abilityService.HoveredAbility.Value = _viewModel;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _viewModel.IsCompatibleHighlighted.Value = false;
            if (_abilityService.HoveredAbility.Value == _viewModel)
                _abilityService.HoveredAbility.Value = null;
            
            _viewModel.IsHoveredClick.Value = false;
        }
    }
}