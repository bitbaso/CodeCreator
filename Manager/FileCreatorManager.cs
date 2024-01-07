using Microsoft.Extensions.Logging;

namespace CodeCreator.Manager;

public class FileCreatorManager(ILogger<FileCreatorManager> logger)
{

    /// <summary>
    /// Crear un archivo
    /// </summary>
    /// <param name="content"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<bool> CreateFile(string content, string path)
    {
        try
        {
            var outcome = default(bool);

            await File.WriteAllTextAsync(path, content);

            return outcome;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR  FunctionName");
            return default;
        }
    }
}
