namespace ServiceUI.Interfaces
{
    public interface IAutoMigrationService
    {
        public Task EnsureDatabaseInitializedAsync();
    }
}
