using System.Linq.Expressions;

namespace UMS.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyFilter<T, TValue>(
        this IQueryable<T> query,
        TValue? value,
        Expression<Func<T, bool>> predicate)
    {
        return value switch
        {
            null => query,
            string val when string.IsNullOrWhiteSpace(val) => query,
            ICollection<object> collection when !collection.Any() => query,
            _ => query.Where(predicate)
        };
    }
}