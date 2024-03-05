using Microsoft.Data.SqlClient;

namespace MedicineMarketPlace.BuildingBlocks.Utilities
{
    public static class SqlHelper
    {
        public static List<TData> GetProcedureData<TData>(string connString, string procName,
            params SqlParameter[] parameters)
        {
            using (var sqlConnection = new SqlConnection(connString))
            {
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = procName;
                    if (parameters != null)
                    {
                        sqlCommand.Parameters.AddRange(parameters);
                    }
                    sqlConnection.Open();
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        List<TData> elements;
                        try
                        {
                            elements = ModelBinder<TData>.ReadProcedureData(reader);
                        }
                        finally
                        {
                            while (reader.NextResult())
                            { }
                        }
                        return elements;
                    }
                }
            }
        }
    }
}
