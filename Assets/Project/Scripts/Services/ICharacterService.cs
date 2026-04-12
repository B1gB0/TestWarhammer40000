using Project.Scripts.Entity;

namespace Project.Scripts.Services
{
    public interface ICharacterService : IService
    {
        public Character CreateCharacter(int indexCharacter, int countAbilities, int countModifications);
    }
}