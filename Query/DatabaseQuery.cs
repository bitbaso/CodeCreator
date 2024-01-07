using CodeCreator.DTO;
using Dapper;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace CodeCreator.Query;

public class DatabaseQuery(MySqlConnection connection, ILogger<DatabaseQuery> logger)
{
  /// <summary>
  /// Obtiene las tabals de la base de datos
  /// </summary>
  /// <param name="databaseName"></param>
  /// <returns></returns>
  public async Task<List<string>> GetTables(string databaseName)
  {
    try
    {
      var outcome = new List<string>();

      if (!string.IsNullOrEmpty(databaseName))
      {
        var sql = @$"SELECT TABLE_NAME 
                     FROM INFORMATION_SCHEMA.TABLES 
                     WHERE TABLE_SCHEMA = '{databaseName}'";
        outcome = (await connection.QueryAsync<string>(sql)).ToList();
      }

      return outcome;
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "ERROR GetTables");
      return default;
    }
  }

  /// <summary>
  /// Obtiene las columnas de una tabla
  /// </summary>
  /// <param name="databaseName"></param>
  /// <param name="tableName"></param>
  /// <returns></returns>
  public async Task<List<ColumnData>> GetColumns(string databaseName, string tableName)
  {
    try
    {
      var outcome = new List<ColumnData>();

      if (!string.IsNullOrEmpty(databaseName))
      {
        var sql = @$"SELECT COLUMN_NAME, DATA_TYPE 
                     FROM INFORMATION_SCHEMA.COLUMNS 
                     WHERE TABLE_SCHEMA = '{databaseName}' 
                           AND TABLE_NAME = '{tableName}'";
        outcome = (await connection.QueryAsync<ColumnData>(sql)).ToList();
      }

      return outcome;
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "ERROR GetColumns");
      return default;
    }
  }
}
