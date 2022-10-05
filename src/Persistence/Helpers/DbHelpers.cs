using Application.Contracts.Persistence.Helpers;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Persistence.Helpers
{
    internal class DbHelpers : IDbHelpers
    {
        private readonly IConfiguration _configuration;

        public DbHelpers(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #region Dapper Method to Insert Record via Store Procedure
        /// <summary>
        /// Save single record in database using sql store procedure
        /// </summary>
        /// <typeparam name="InputParemeters"></typeparam>
        /// <param name="storeProcedureName"></param>
        /// <param name="inputParameters"></param>
        /// <param name="connectionStringName"></param>
        /// <returns></returns>
        public async Task<ReturnType?> InsertRecordByStoreProcedureAsync<ReturnType, InputParemeters>(string storeProcedureName, InputParemeters? inputParameters = default, string connectionStringName = "DefaultConnection")
        {
            using IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString(connectionStringName));
            return await dbConnection.QueryFirstOrDefaultAsync<ReturnType>(storeProcedureName, inputParameters, commandType: CommandType.StoredProcedure);
        }
        #endregion

        #region Dapper Method to Get Single Record via Store Procedure
        /// <summary>
        /// Retrieve single record from database using sql store procedure
        /// </summary>
        /// <typeparam name="ReturnType"></typeparam>
        /// <typeparam name="InputParemeters"></typeparam>
        /// <param name="storeProcedureName"></param>
        /// <param name="inputParameters"></param>
        /// <param name="connectionStringName"></param>
        /// <returns></returns>
        public async Task<ReturnType> GetRecordByStoreProcedureAsync<ReturnType, InputParemeters>(string storeProcedureName, InputParemeters? inputParameters = default, string connectionStringName = "DefaultConnection")
        {
            using IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString(connectionStringName));
            return await dbConnection.QueryFirstOrDefaultAsync<ReturnType>(storeProcedureName, inputParameters, commandType: CommandType.StoredProcedure);
        }
        #endregion

        #region Dapper Method to Get Records via Store Procedure
        /// <summary>
        /// Get list of records using sql store procedure
        /// </summary>
        /// <typeparam name="ReturnType"></typeparam>
        /// <param name="storeProcedureName"></param>
        /// <param name="connectionStringName"></param>
        /// <returns>List of objects you specified as a return type</returns>
        public async Task<List<ReturnType>> GetAllByStoreProcedureAsync<ReturnType>(string storeProcedureName, string connectionStringName = "DefaultConnection")
        {
            using IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString(connectionStringName));
            return (await dbConnection.QueryAsync<ReturnType>(storeProcedureName, commandType: CommandType.StoredProcedure)).ToList();
        }
        #endregion

        #region Dapper Method to Update Record via Store Procedure
        /// <summary>
        /// Update single record in database using sql store procedure
        /// </summary>
        /// <typeparam name="InputParemeters"></typeparam>
        /// <param name="storeProcedureName"></param>
        /// <param name="inputParameters"></param>
        /// <param name="connectionStringName"></param>
        /// <returns></returns>
        public async Task<int> UpdateRecordByStoreProcedureAsync<InputParemeters>(string storeProcedureName, InputParemeters? inputParameters = default, string connectionStringName = "DefaultConnection")
        {
            using IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString(connectionStringName));
            return await dbConnection.ExecuteAsync(storeProcedureName, inputParameters, commandType: CommandType.StoredProcedure);
        }
        #endregion
    }
}
