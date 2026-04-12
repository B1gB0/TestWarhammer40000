using Project.Scripts.Database.Data;

namespace Project.Scripts.Entity
{
    public class Ability
    {
        public Ability(AbilityData data)
        {
            Data = data;
        }
        
        public AbilityData Data { get; private set; }
    }
}