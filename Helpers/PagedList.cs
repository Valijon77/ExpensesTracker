using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Helpers;

public class PagedList<T> : List<T>
{
    private PagedList(IEnumerable<T> items, int currentPage, int pageSize, int totalCount)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        AddRange(items);
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public static async Task<PagedList<T>> CreateAsync(
        IQueryable<T> query,
        int currentPage,
        int pageSize
    )
    {
        var totalCount = await query.CountAsync();
        var items = await query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedList<T>(items, currentPage, pageSize, totalCount);
    }
}
