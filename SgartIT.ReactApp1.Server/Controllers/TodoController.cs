using Microsoft.AspNetCore.Mvc;
using SgartIT.ReactApp1.Server.DTO;
using SgartIT.ReactApp1.Server.DTO.Settings;
using SgartIT.ReactApp1.Server.Exports.Excel;
using SgartIT.ReactApp1.Server.Exports.Pdf;
using SgartIT.ReactApp1.Server.Services;
using System.IO;
using System.Net.Http.Headers;

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
    public async Task<IActionResult> Index(string? text)
    {
        try
        {
            logger.LogInformation("Find {text}", text);

            return Ok(await main.FindAsync(text));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Find {text}", text);

            return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// GET: /todo/5
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            logger.LogInformation("Get {id}", id);

            var todo = await main.GetAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Get {id}", id);

            return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// POST: /todo
    /// </summary>
    /// <param name="todo"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create(TodoCreate todo)
    {
        try
        {
            logger.LogInformation("Create {title}", todo.Title);

            TodoId todoId = await main.CreateAsync(todo);

            logger.LogInformation("Created {id}", todoId.Id);

            return Created("/api/todo", todoId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Create {title}", todo.Title);

            return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
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
        try
        {
            logger.LogInformation("Edit {id}", id);

            await main.UpdateAsync(id, todo);

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Edit {id}", id);

            return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
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
        try
        {
            logger.LogInformation("Delete {id}", id);

            await main.DeleteAsync(id);

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Delete {id}", id);

            return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
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
            logger.LogError(ex, "Excel find {text}", text);

            return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("pdf")]
    public async Task<IActionResult> PdfExport(string? text)
    {
        try
        {
            List<Todo> items = await main.FindAsync(text);

            PdfExport ex = new(logger);
            byte[] contents = await ex.Export( items, text);

            DateTime dt = DateTime.Now;
            Microsoft.Net.Http.Headers.MediaTypeHeaderValue headers = new("application/pdf");
            //headers.Parameters.Add(new Microsoft.Net.Http.Headers.NameValueHeaderValue("Content-Disposition", $@"attachment;filename=""todo-{dt:yyyyMMddHHmmss}.pdf"""));

            return new FileContentResult(contents, headers);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "PDF find {text}", text);

            return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
