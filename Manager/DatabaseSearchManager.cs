using System.Text.Json;
using CodeCreator.DTO;
using CodeCreator.Entities;
using CodeCreator.Query;
using Microsoft.Extensions.Logging;

namespace CodeCreator.Manager;

public class DatabaseSearchManager(DatabaseQuery databaseQuery, ILogger<DatabaseSearchManager> logger)
{
    /// <summary>
    /// Obtiene la información de la base de datos
    /// </summary>
    /// <returns></returns>
    public async Task<DatabaseDataEntity> GetDatabaseInfo(string databaseName)
    {
        try
        {
            var outcome = default(DatabaseDataEntity);
            if (!string.IsNullOrEmpty(databaseName))
            {
                var databaseData = new DatabaseDataEntity();
                var tablesOfDatabase = await databaseQuery.GetTables(databaseName);
                if (tablesOfDatabase != null && tablesOfDatabase.Any())
                {
                    databaseData.Tables = new List<TableDataEntity>();
                    foreach (var tableName in tablesOfDatabase)
                    {
                        if (!string.IsNullOrEmpty(tableName))
                        {
                            var tableDataToAdd = new TableDataEntity()
                            {
                                Name = tableName
                            };

                            var columnsOfTheTable = await databaseQuery.GetColumns(databaseName, tableName);
                            if (columnsOfTheTable != null)
                            {
                                var columnsData = new List<ColumnDataEntity>();
                                foreach (var column in columnsOfTheTable)
                                {
                                    if (column != null)
                                    {
                                        var columnDataToAdd = new ColumnDataEntity()
                                        {
                                            Name = column.COLUMN_NAME,
                                            MysqlColumnType = column.DATA_TYPE,
                                        };
                                        columnsData.Add(columnDataToAdd);
                                    }
                                }
                                tableDataToAdd.Columns = columnsData;
                            }

                            databaseData.Tables.Add(tableDataToAdd);
                        }
                    }

                    outcome = databaseData;
                }
            }
            return outcome;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error GetDatabaseInfo");
            return default;
        }
    }

}
