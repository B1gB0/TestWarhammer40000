using System.Collections.Generic;
using Project.Scripts.Entity;
using Project.Scripts.UI.ViewModel;
using R3;

namespace Project.Scripts.Services
{
    public interface IModificationService : IService
    {
        public ReactiveProperty<ModificationViewModel> CurrentDraggedModification { get; }
        public ReactiveProperty<ModificationViewModel> HoveredModification { get; }
        public ReactiveProperty<bool> IsDragging { get; }
        public List<Modification> CreateAllModificationsByCount(int count);
        public void RegisterDragHandler(ModificationDragHandler handler);
        public void UnregisterDragHandler(ModificationDragHandler handler);
        public void ForceEndDrag();
    }
}