using HandlebarsDotNet;
using Microsoft.Extensions.Logging;

namespace CodeCreator.Manager;

public class CodeTemplateManager(ILogger<CodeTemplateManager> logger)
{
    /// <summary>
    /// Obtiene la plantilla de un tipo
    /// </summary>
    /// <param name="templateName"></param>
    /// <returns></returns>
    public async Task<string> GetTemplate(string templateName, string templatesPath)
    {
        try
        {
            var outcome = default(string);

            if (!string.IsNullOrEmpty(templateName) && !string.IsNullOrEmpty(templatesPath))
            {
                var templatePath = Path.Combine(templatesPath, templateName);
                outcome = await File.ReadAllTextAsync(templatePath);
            }

            return outcome;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR  GetTemplate");
            return default;
        }
    }

    /// <summary>
    /// Obtiene la plantilla compilada con datos
    /// </summary>
    /// <param name="template"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public async Task<string> FillTemplate(string template, object data)
    {
        try
        {
            var outcome = default(string);

            if (!string.IsNullOrEmpty(template) && data != null)
            {
                var compiledTemplate = Handlebars.Compile(template);
                if (compiledTemplate != null)
                {
                    outcome = compiledTemplate(data);
                }
            }

            return outcome;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "ERROR  CompileTemplate");
            return default;
        }
    }
}
