using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Entity;
using R3;
using UnityEngine;

namespace Project.Scripts.UI.ViewModel
{
    public class AbilityViewModel
    {
        private readonly Ability _ability;
        private readonly CompositeDisposable _disposables = new();

        public ReactiveProperty<string> Name { get; }
        public ReactiveProperty<Sprite> Icon { get; }

        public ReactiveProperty<ModificationViewModel> AttachedModification { get; }

        public ReactiveProperty<bool> HasModification { get; }

        public ReactiveProperty<bool> IsCompatibleHighlighted { get; }

        public IReadOnlyList<ModificationType> CompatibleTypes => _ability.Data.CompatibleTypes;

        public AbilityViewModel(Ability ability)
        {
            _ability = ability;
            Name = new ReactiveProperty<string>(ability.Data.Name);
            Icon = new ReactiveProperty<Sprite>(ability.Data.IconSprite);
            AttachedModification = new ReactiveProperty<ModificationViewModel>();
            HasModification = AttachedModification.Select(mod => mod != null).ToBindableReactiveProperty();
            IsCompatibleHighlighted = new ReactiveProperty<bool>(false);
        }

        public bool TryAttachModification(ModificationViewModel modification)
        {
            if (!IsCompatible(modification))
                return false;

            if (AttachedModification.Value != null)
                return false;

            AttachedModification.Value = modification;
            modification.MarkAsAttached(this);
            return true;
        }

        public void DetachModification()
        {
            if (AttachedModification.Value == null) return;

            var mod = AttachedModification.Value;
            AttachedModification.Value = null;
            mod.MarkAsDetached();
        }

        public void Dispose()
        {
            _disposables.Dispose();
            Name.Dispose();
            Icon.Dispose();
            AttachedModification.Dispose();
            HasModification.Dispose();
            IsCompatibleHighlighted.Dispose();
        }
        
        private bool IsCompatible(ModificationViewModel modification)
        {
            if (modification == null) return false;
            return CompatibleTypes.Contains(modification.ModificationType.Value);
        }
    }
}