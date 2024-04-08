using System.Text.Json;
using ExpensesTracker.DTOs;
using ExpensesTracker.Helpers;
using Microsoft.AspNetCore.Http.Json;

namespace ExpensesTracker.Extensions;

public static class HttpExtensions
{
    public static void AddPaginationHeader(
        this HttpResponse response,
        PaginationHeader paginationHeader
    )
    {
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, jsonOptions));
        response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
    }
}
