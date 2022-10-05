namespace Application.Contracts.Persistence.Helpers
{
    public interface IDbHelpers
    {
        Task<ReturnType?> InsertRecordByStoreProcedureAsync<ReturnType, InputParemeters>(string storeProcedureName, InputParemeters? inputParameters = default, string connectionStringName = "DefaultConnection");
        Task<ReturnType> GetRecordByStoreProcedureAsync<ReturnType, InputParemeters>(string storeProcedureName, InputParemeters? inputParameters = default, string connectionStringName = "DefaultConnection");
        Task<List<ReturnType>> GetAllByStoreProcedureAsync<ReturnType>(string storeProcedureName, string connectionStringName = "DefaultConnection");
        Task<int> UpdateRecordByStoreProcedureAsync<InputParemeters>(string storeProcedureName, InputParemeters? inputParameters = default, string connectionStringName = "DefaultConnection");
    }
}