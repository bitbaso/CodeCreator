using System.Globalization;
using CodeCreator.Entities;
using Microsoft.Extensions.Logging;

namespace CodeCreator.Manager;

public class DataTransformerManager(ILogger<DataTransformerManager> logger)
{
    private Dictionary<string, string> _mysqlToCSharpMapping = new Dictionary<string, string>
        {
            {"bit", "bool"},
            {"tinyint", "sbyte"},
            {"smallint", "short"},
            {"mediumint", "int"},
            {"int", "int"},
            {"bigint", "long"},
            {"float", "float"},
            {"double", "double"},
            {"decimal", "decimal"},
            {"char", "char"},
            {"varchar", "string"},
            {"binary", "byte[]"},
            {"varbinary", "byte[]"},
            {"blob", "byte[]"},
            {"text", "string"},
            {"date", "DateTime"},
            {"time", "TimeSpan"},
            {"datetime", "DateTime"},
            {"timestamp", "DateTime"},
            {"year", "short"},
            {"enum", "string"},
            {"set", "string[]"},
            {"boolean", "bool"},
            {"json", "string"},
            // Añadir más mapeos según sea necesario
        };



    public async Task<TransformTableDataEntity> GetTransformTable(TableDataEntity tableDataEntity, string namespaceName)
    {
        try
        {
            var outcome = default(TransformTableDataEntity);


            if (tableDataEntity != null
                && tableDataEntity.Columns != null
                && tableDataEntity.Columns.Any())
            {
                var cleanClassName = await CleanName(tableDataEntity.Name);
                var cleanNamespaceName = await CleanName(namespaceName);
                outcome = new TransformTableDataEntity()
                {
                    Name = cleanClassName,
                    NamespaceName = cleanNamespaceName
                };

                outcome.Columns = tableDataEntity.Columns;
                outcome.TransformColumns = new List<TransformColumnDataEntity>();
                foreach (var column in outcome.Columns)
                {
                    if (column != null)
                    {
                        var cleanColumnName = await CleanName(column.Name);
                        var trasnsformColumnToAdd = new TransformColumnDataEntity()
                        {
                            Name = cleanColumnName,
                            MysqlColumnType = column.MysqlColumnType,
                            NetColumnType = _mysqlToCSharpMapping[column.MysqlColumnType]
                        };
                        outcome.TransformColumns.Add(trasnsformColumnToAdd);
                    }
                }
            }

            return outcome;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR  GetTransformTable");
            return default;
        }
    }

    /// <summary>
    /// Limpia el nombre para una propiedad
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private async Task<string> CleanName(string text)
    {
        try
        {
            var outcome = default(string);

            if (!string.IsNullOrEmpty(text))
            {
                var trimText = text.Trim();
                // Convertir la primera letra a mayúsculas y mantener el resto del texto
                outcome = char.ToUpper(trimText[0], CultureInfo.InvariantCulture) + trimText.Substring(1);
            }

            return outcome;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR  CleanName");
            return default;
        }
    }
}
