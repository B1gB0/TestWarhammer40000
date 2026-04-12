using Project.Scripts.Database.Data;

namespace Project.Scripts.Entity
{
    public class Modification
    {
        public Modification(ModificationData data)
        {
            Data = data;
        }
        
        public ModificationData Data { get; private set; }
    }
}