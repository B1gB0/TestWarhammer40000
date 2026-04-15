using Project.Scripts.Entity;
using R3;

namespace Project.Scripts.UI.ViewModel
{
    public class ModificationViewModel
    {
        private readonly Modification _modification;
        private readonly CompositeDisposable _disposables = new();

        public ReactiveProperty<string> Name { get; }
        public ReactiveProperty<ModificationType> ModificationType { get; }
        public ReactiveProperty<bool> IsEquipped { get; }
        public ReactiveProperty<bool> IsHoveredClick { get; }
        public ReactiveProperty<AbilityViewModel> AttachedAbility { get; }
        public ReactiveProperty<bool> IsCompatibleHighlighted { get; }

        public ModificationViewModel(Modification modification)
        {
            _modification = modification;
            
            Name = new ReactiveProperty<string>(modification.Data.Name);
            ModificationType = new ReactiveProperty<ModificationType>(modification.Data.Type);
            IsEquipped = new ReactiveProperty<bool>(false);
            AttachedAbility = new ReactiveProperty<AbilityViewModel>();
            IsCompatibleHighlighted = new ReactiveProperty<bool>(false);
            IsHoveredClick = new ReactiveProperty<bool>(false);
        }
        
        public void MarkAsAttached(AbilityViewModel ability)
        {
            IsEquipped.Value = true;
            AttachedAbility.Value = ability;
        }
        
        public void MarkAsDetached()
        {
            IsEquipped.Value = false;
            AttachedAbility.Value = null;
        }

        public void Dispose()
        {
            _disposables.Dispose();
            Name.Dispose();
            IsEquipped.Dispose();
            AttachedAbility.Dispose();
            IsCompatibleHighlighted.Dispose();
            IsHoveredClick.Dispose();
        }
    }
}