namespace Kaboom.Interfaces
{
    public interface IDataBaseService
    {
        void SetDatabaseProvider(string provider);
        string GetCurrentDatabaseProvider();

    }
}
