namespace ServiceManagerRestAPI {
    public interface IAutoMigrationService 
    {
        public Task EnsureDatabaseInitializedAsync();
    }
}
