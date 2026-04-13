using Cysharp.Threading.Tasks;
using Project.Scripts.Entity;

namespace Project.Scripts.Services
{
    public interface ICharacterService : IService
    {
        public UniTask<Character> CreateRandomUniqueCharacter(int countAbilities, int countModifications);
    }
}