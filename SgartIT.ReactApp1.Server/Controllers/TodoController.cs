using Microsoft.AspNetCore.Mvc;
using SgartIT.ReactApp1.Server.DTO;
using SgartIT.ReactApp1.Server.Exports.Excel;
using SgartIT.ReactApp1.Server.Exports.Pdf;
using SgartIT.ReactApp1.Server.Services;

namespace SgartIT.ReactApp1.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController(ILogger<MainService> logger, MainService main) : ControllerBase
{
    // 
    /// <summary>
    /// GET: /todo?text=xxxx 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Index(string? text) => Ok(await main.FindAsync(text));
    

    /// <summary>
    /// GET: /todo/5
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var todo = await main.GetAsync(id);

        if (todo == null)
        {
            return NotFound();
        }

        return Ok(todo);
    }

    /// <summary>
    /// POST: /todo
    /// </summary>
    /// <param name="todo"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create(TodoCreate todo)
    {
        logger.LogInformation("Create {title}", todo.Title);

        TodoId todoId = await main.CreateAsync(todo);

        logger.LogInformation("Created {id}", todoId.Id);

        return Created("/api/todo", todoId);
    }

    /// <summary>
    /// PATCH: /todo/5
    /// </summary>
    /// <param name="id"></param>
    /// <param name="collection"></param>
    /// <returns></returns>
    [HttpPatch("{id}")]
    public async Task<IActionResult> Edit(int id, TodoEdit todo)
    {
        await main.UpdateAsync(id, todo);

        return NoContent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="collection"></param>
    /// <returns></returns>
    // DELETE: /todo/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await main.DeleteAsync(id);

        return NoContent();
    }

    [HttpGet("excel")]
    public async Task<IActionResult> ExcelExport(string? text)
    {
        try
        {
            List<Todo> items = await main.FindAsync(text);

            ExcelExport ex = new(logger);

            MemoryStream ms = new();
            ex.Export(ms, items);

            //reset the position to the start of the stream
            ms.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        catch (Exception ex)
        {
            // esempio di errore custom nel caso il default GlobalExceptionHandler non andasse bene
            logger.LogError(ex, "Excel find {text}", text);

            return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("pdf")]
    public async Task<IActionResult> PdfExport(string? text)
    {
        logger.LogInformation("PDF export text: {text}", text);
        List<Todo> items = await main.FindAsync(text);

        PdfExport ex = new(logger);
        byte[] contents = await ex.Export(items, text);

        DateTime dt = DateTime.Now;
        Microsoft.Net.Http.Headers.MediaTypeHeaderValue headers = new("application/pdf");
        //headers.Parameters.Add(new Microsoft.Net.Http.Headers.NameValueHeaderValue("Content-Disposition", $@"attachment;filename=""todo-{dt:yyyyMMddHHmmss}.pdf"""));

        return new FileContentResult(contents, headers);
    }
}
