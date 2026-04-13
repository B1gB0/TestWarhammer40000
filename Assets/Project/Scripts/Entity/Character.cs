using System.Collections.Generic;
using Project.Scripts.Database.Data;

namespace Project.Scripts.Entity
{
    public class Character
    {
        public Character(CharacterData data, List<Ability> abilities, List<Modification> modifications)
        {
            Data = data;
            Abilities = abilities;
            Modifications = modifications;
        }
        
        public CharacterData Data { get; private set; }
        public IReadOnlyList<Ability> Abilities { get; private set; }
        public IReadOnlyList<Modification> Modifications { get; private set; }
    }
}