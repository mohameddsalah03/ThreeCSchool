namespace ThreeCSchool.Core.Domain.Contracts.Persistence
{
    public interface IDataSeeding
    {
        Task InitializeAsync();
        Task DataSeedAsync();
    }
}