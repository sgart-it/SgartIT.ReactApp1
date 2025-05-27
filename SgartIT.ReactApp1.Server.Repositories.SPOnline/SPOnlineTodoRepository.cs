using Microsoft.Extensions.Logging;
using PnP.Core.Model.SharePoint;
using PnP.Core.QueryModel;
using PnP.Core.Services;
using SgartIT.ReactApp1.Server.DTO;
using SgartIT.ReactApp1.Server.DTO.Repositories;
using SgartIT.ReactApp1.Server.Repositories.SPOnline.Settings;

namespace SgartIT.ReactApp1.Server.Repositories.SPOnline;

public class SPOnlineTodoRepository(ILogger<SPOnlineTodoRepository> logger, IPnPContextFactory pnpContextFactory, SPOnlineSettings settings) : ITodoRepository
{
    const string FLD_TITLE = "Title";
    const string FLD_COMPLETED = "Completed";
    const string FLD_CATEGORY = "Category";
    const string FLD_CREATED = "Created";
    const string FLD_MODIFIED = "Modified";

    /// <summary>
    /// ATTENZIONE: per il caso reale va gestita la paginazione per supportare più di 5000 items
    /// vedi: https://pnp.github.io/pnpcore/using-the-sdk/working-with-large-lists.html
    /// https://pnp.github.io/pnpcore/using-the-sdk/listitems-intro.html
    /// https://www.sgart.it/IT/informatica/problema-con-pnpcore-e-loaditemsbycamlqueryasync/post
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public async Task<List<Todo>> FindAsync(string? text)
    {
        logger.LogDebug("Getting all todos from list {list} with text: {text}", settings.ListName, text);

        List<Todo> items = [];

        using IPnPContext context = await GetPnpContext();
        var myList = GetList(context);

        // ATTENZIONE: Contains funziona solo su elenchi con meno di 5000 items
        string whereQuery = string.IsNullOrEmpty(text)
            ? string.Empty
            : $@"<Where>
                    <Or>
                        <Contains>
                            <FieldRef Name='{FLD_TITLE}'/>
                            <Value Type='Text'>{text ?? string.Empty}</Value>
                        </Contains>
                        <Contains>
                            <FieldRef Name='{FLD_CATEGORY}'/>
                            <Value Type='Text'>{text ?? string.Empty}</Value>
                        </Contains>
                    </Or>
                </Where>";

        string viewXml = $@"<View>
                    <ViewFields>
                        <FieldRef Name='{FLD_TITLE}'/>
                        <FieldRef Name='{FLD_COMPLETED}'/>
                        <FieldRef Name='{FLD_CATEGORY}'/>
                        <FieldRef Name='{FLD_CREATED}'/>
                        <FieldRef Name='{FLD_MODIFIED}'/>
                    </ViewFields>
                    <Query>
                      {whereQuery}
                      <OrderBy Override='TRUE'><FieldRef Name='{FLD_TITLE}' Ascending='FALSE'/></OrderBy>
                    </Query>
                   </View>";

        // Execute the query
        await myList.LoadItemsByCamlQueryAsync(new CamlQueryOptions()
        {
            ViewXml = viewXml,
            DatesInUtc = true
        }, p => p.RoleAssignments.QueryProperties(p => p.PrincipalId, p => p.RoleDefinitions));

        // Iterate over the retrieved list items
        foreach (IListItem listItem in myList.Items.AsRequested())
        {
            Todo item = MapToTodo(listItem);
            items.Add(item);
        }

        return items;
    }


    public async Task<Todo?> GetAsync(int id)
    {
        logger.LogDebug("Getting todo with id: {id}", id);

        using IPnPContext context = await GetPnpContext();
        var myList = GetList(context);

        IListItem listItem = await myList.Items.GetByIdAsync(id);

        if (listItem != null)
        {
            return MapToTodo(listItem);
        }
        return null;

    }

    public async Task<TodoId> SaveAsync(int id, TodoEdit todo)
    {
        logger.LogDebug("Saving todo: {@todo}", todo);

        using IPnPContext context = await GetPnpContext();
        var myList = GetList(context);

        if (id == 0)
        {
            Dictionary<string, object> item = new()
            {
                { FLD_TITLE, todo.Title },
                { FLD_COMPLETED, todo.IsCompleted },
                { FLD_CATEGORY, todo.Category },
            };

            IListItem addedItem = await myList.Items.AddAsync(item);

            id = addedItem.Id;
        }
        else
        {
            IListItem listItem = await myList.Items.GetByIdAsync(id);

            listItem[FLD_TITLE] = todo.Title;
            listItem[FLD_COMPLETED] = todo.IsCompleted;
            listItem[FLD_CATEGORY] = todo.Category;

            await listItem.UpdateAsync();
        }

        return new TodoId(default)
        {
            Id = id
        };
    }


    public async Task DeleteAsync(int id)
    {
        logger.LogDebug("Deleting todo with id: {id}", id);

        using IPnPContext context = await GetPnpContext();
        var myList = GetList(context);

        IListItem listItem = await myList.Items.GetByIdAsync(id);

        if (listItem != null)
        {
            await listItem.DeleteAsync();
        }
    }


    private static Todo MapToTodo(IListItem listItem) => new(
            listItem.Id,
            (string)(listItem[FLD_TITLE] ?? string.Empty),
            (string)(listItem[FLD_CATEGORY] ?? string.Empty),
            (bool)(listItem[FLD_COMPLETED] ?? false),
            (DateTime)listItem[FLD_CREATED],
            (DateTime)listItem[FLD_MODIFIED]
        );

    private async Task<IPnPContext> GetPnpContext()
    {
        return await pnpContextFactory.CreateAsync(Constants.KEY_SITE);
    }

    private PnP.Core.Model.SharePoint.IList GetList(IPnPContext context)
    {
        return context.Web.Lists.GetByTitle(settings.ListName,
            p => p.Title,
            p => p.Fields.QueryProperties(p => p.InternalName,
                                        p => p.FieldTypeKind,
                                        p => p.TypeAsString,
                                        p => p.Title));
    }

}
