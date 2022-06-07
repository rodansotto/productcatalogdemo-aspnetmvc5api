using System;
using System.Linq;
using System.Linq.Expressions;

namespace MyMvc5App
{
    public static class ExtensionMethods
    {
        // Need to create an extension method for ordering in both directions.
        // Standard Query Operators Overview: http://msdn.microsoft.com/en-us/library/bb397896(v=vs.100).aspx
        // Extension Methods: http://msdn.microsoft.com/en-us/library/bb383977(v=vs.100).aspx
        // How to: Implement and Call a Custom Extension Method: http://msdn.microsoft.com/en-us/library/bb311042(v=vs.100).aspx
        // This new extension method will call existing extension methods OrderBy and OrderByDescending, based on the string parameter sortDir.
        // These existing extension methods sorts elements in data structures that implement IQueryable<T>.
        // Queryable Class: http://msdn.microsoft.com/en-us/library/system.linq.queryable(v=vs.100).aspx
        // Queryable.OrderBy Method: http://msdn.microsoft.com/en-us/library/bb549264(v=vs.100).aspx
        // Queryable.OrderByDescending Method: http://msdn.microsoft.com/en-us/library/bb534316(v=vs.100).aspx
        // These existing extension methods accept an Expression<Func<TSource, TKey>> parameter.
        // Expression<TDelegate> represents TDelegate as a data structure in the form of an expression tree.
        // Expression<TDelegate> Class: http://msdn.microsoft.com/en-us/library/bb335710(v=vs.100).aspx
        // Expression Trees: http://msdn.microsoft.com/en-us/library/bb397951(v=vs.100).aspx
        // How to: Use Expression Trees to Build Dynamic Queries: http://msdn.microsoft.com/en-us/library/bb882637(v=vs.100).aspx
        // Func<TSource, TKey> is the delegate (the TDelegate in Expression<TDelegate> in this case).
        // Func<T, TResult> Delegate: http://msdn.microsoft.com/en-us/library/bb549151(v=vs.100).aspx
        // These existing methods return IOrderedQueryable<TSource> whose elements are sorted according to a key
        // IOrderedQueryable Interface: http://msdn.microsoft.com/en-us/library/System.Linq.IOrderedQueryable(v=vs.100).aspx
        public static IOrderedQueryable<TSource> OrderByWithDirection<TSource, TKey>(
            this IQueryable<TSource> query, Expression<Func<TSource, TKey>> keySelector, string sortDir)
        {
            if (sortDir.ToUpper().Equals("DESC"))
            {
                return query.OrderByDescending(keySelector);
            }
            else
            {
                return query.OrderBy(keySelector);
            }
        }
    }
}