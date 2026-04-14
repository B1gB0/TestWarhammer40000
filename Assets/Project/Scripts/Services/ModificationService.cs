using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Scripts.Database.Data;
using Project.Scripts.Entity;
using Project.Scripts.UI.ViewModel;
using R3;
using Reflex.Attributes;

namespace Project.Scripts.Services
{
    public class ModificationService : IModificationService
    {
        private readonly List<ModificationData> _modificationsData = new();
        private readonly Random _random = new();

        private IDataBaseService _dataBaseService;
        
        public ReactiveProperty<ModificationViewModel> CurrentDraggedModification { get; } = new();
        public ReactiveProperty<ModificationViewModel> HoveredModification { get; } = new();
        public ReactiveProperty<bool> IsDragging { get; } = new();
        
        public bool IsInitiated { get; private set; }

        [Inject]
        private void Construct(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public UniTask Init()
        {
            if (IsInitiated)
                return UniTask.CompletedTask;
            

            foreach (var data in _dataBaseService.Content.ModificationsData)
            {
                _modificationsData.Add(data);
            }

            IsInitiated = true;

            return UniTask.CompletedTask;
        }

        public List<Modification> CreateAllModificationsByCount(int count)
        {
            if (count <= 0)
                return new List<Modification>();

            if (_modificationsData.Count == 0)
                throw new InvalidOperationException("Нет доступных способностей для создания.");

            var result = new List<Modification>(count);
            var allDataList = _modificationsData;
            int availableCount = allDataList.Count;
            
            var shuffled = allDataList.OrderBy(_ => _random.Next()).ToList();
            
            int uniqueAbilities = Math.Min(count, availableCount);
            for (int i = 0; i < uniqueAbilities; i++)
            {
                result.Add(new Modification(shuffled[i]));
            }
            
            int remaining = count - uniqueAbilities;
            for (int i = 0; i < remaining; i++)
            {
                int randomIndex = _random.Next(availableCount);
                result.Add(new Modification(allDataList[randomIndex]));
            }

            return result;
        }
        
        private ModificationDragHandler _activeHandler;

        public void RegisterDragHandler(ModificationDragHandler handler)
        {
            _activeHandler = handler;
            IsDragging.Value = true;
        }

        public void UnregisterDragHandler(ModificationDragHandler handler)
        {
            if (_activeHandler == handler)
            {
                _activeHandler = null;
                IsDragging.Value = false;
            }
        }

        public void ForceEndDrag()
        {
            _activeHandler.CleanupGhost();
            _activeHandler = null;
            IsDragging.Value = false;
            CurrentDraggedModification.Value = null;
        }
    }
}