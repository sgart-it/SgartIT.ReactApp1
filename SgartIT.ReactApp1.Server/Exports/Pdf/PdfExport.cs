using iText.Html2pdf;
using SgartIT.ReactApp1.Server.DTO;
using System.Text;


namespace SgartIT.ReactApp1.Server.Exports.Pdf;

/// <summary>
/// itext7.bouncy-castle-adapter
/// itext7.pdfhtml
/// </summary>
/// <param name="logger"></param>
public class PdfExport(ILogger logger)
{
    System.Globalization.CultureInfo ciIT = new(1040);
    public async Task<byte[]> Export(List<Todo> todos, string? text)
    {
        logger.LogDebug("Pdf export");
        try
        {
            // prepare html template
            StringBuilder sb = new(1000);
            foreach (Todo todo in todos)
            {
                sb.Append("<tr>");
                sb.AppendFormat("<td>{0}</td>", todo.Title);
                sb.AppendFormat("<td>{0}</td>", todo.IsCompleted ? "Yes" : "No");
                sb.AppendFormat("<td>{0}</td>", todo.Category);
                sb.AppendFormat("<td>{0}</td>", todo.Modified.ToString(ciIT));
                sb.Append("</tr>");
            }
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "/Exports/Pdf/TodoItems.html";
            string htmlContent = (await File.ReadAllTextAsync(fileName))
                .Replace("@@@text@@@", text ?? string.Empty)
                .Replace("@@@rows@@@", sb.ToString());

            using MemoryStream msOutput = new();
            //ConverterProperties converterProperties = new ConverterProperties();
            HtmlConverter.ConvertToPdf(htmlContent, msOutput);

            // convert html to pdf
            //using Document document = new();
            //using PdfWriter writer = PdfWriter.GetInstance(document, msOutput);
            //document.Open();
            //using (StringReader stringReader = new(htmlContent))
            //{
            //    using HtmlWorker htmlParser = new(document);
            //    htmlParser.Parse(stringReader);
            //}
            //document.Close();
            return msOutput.ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Items count {count}", todos?.Count);
            throw;
        }
    }
}
