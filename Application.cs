using CodeCreator.Entities;
using CodeCreator.Manager;
using Microsoft.Extensions.Logging;

namespace CodeCreator;

public class Application(DatabaseSearchManager databaseSearchManager,
                         CodeTemplateManager codeTemplateManager,
                         DataTransformerManager dataTransformerManager,
                         FileCreatorManager fileCreatorManager,
                         ILogger<Application> logger)
{
    public async Task Run()
    {
        try
        {
            Console.WriteLine("CodeCreator");
            Console.WriteLine("Database name:");
            var databaseName = Console.ReadLine();
            Console.WriteLine("Namespace name:");
            var namespaceName = Console.ReadLine();
            //var databaseName = "localDB";
            var templatesPath = "Data/Templates";
            var outpuPath = "Output";

            if (!string.IsNullOrEmpty(databaseName))
            {
                Directory.CreateDirectory(outpuPath);

                var databaseInfo = await databaseSearchManager.GetDatabaseInfo(databaseName);
                if (databaseInfo != null
                    && databaseInfo.Tables != null
                    && databaseInfo.Tables.Any())
                {
                    foreach (var table in databaseInfo.Tables)
                    {
                        if (table != null)
                        {
                            var transformedTableInfo = await dataTransformerManager.GetTransformTable(table, namespaceName);

                            await CreateDTOClass(templatesPath,
                                                 outpuPath,
                                                 transformedTableInfo);
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Run");
        }
    }

    public async Task<bool> CreateDTOClass(string templatesPath,
                                           string outputPath,
                                           TransformTableDataEntity transformTable)
    {
        try
        {
            var outcome = default(bool);

            var template = await codeTemplateManager.GetTemplate("dtoTemplate.hbs", templatesPath);
            if (!string.IsNullOrEmpty(template))
            {
                var filledTemplate = await codeTemplateManager.FillTemplate(template, transformTable);

                var outputFilePath = Path.Combine(outputPath, transformTable.Name + ".cs");
                await fileCreatorManager.CreateFile(filledTemplate, outputFilePath);
                outcome = true;
            }

            return outcome;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR  FunctionName");
            return default;
        }
    }
}
