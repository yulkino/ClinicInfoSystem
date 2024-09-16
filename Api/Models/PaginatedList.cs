namespace Api.Models;

public sealed record PaginatedList<T>(List<T> Items, int CurrentPage, int TotalCount, int TotalPages)
{
    public bool HasPreviousPage => CurrentPage is not 1;
    public bool HasNextPage => CurrentPage != TotalPages;
}
