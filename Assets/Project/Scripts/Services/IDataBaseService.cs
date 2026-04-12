using Project.Scripts.Database;

namespace Project.Scripts.Services
{
    public interface IDataBaseService : IService
    {
        public SpreadsheetContent Content { get; }
    }
}