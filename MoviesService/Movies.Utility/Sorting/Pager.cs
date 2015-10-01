using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HVManager.Util.Sorting;

namespace Movies.Utility.Sorting
{
    public class Pager
    {
        public static PagedList<T> Paginate<T>(List<T> items, PagerSettings pagerSettings)
        {
            if (pagerSettings != null)
            {
                var pagedItems = items;

                // Sort the items
                if (!String.IsNullOrEmpty(pagerSettings.SortBy))
                {
                    pagedItems = new Sorter<T>().Sort(items.AsQueryable(), pagerSettings.SortBy, pagerSettings.SortDirection).ToList();
                }
                else if (pagerSettings.SortExpressions.Any())
                {
                    pagedItems = new Sorter<T>().Sort(items.AsQueryable(), pagerSettings.SortExpressions).ToList();
                }

                // Take the specified items page
                if (pagerSettings.Paginate)
                {
                    pagedItems = pagedItems.Skip((pagerSettings.PageNumber - 1)*pagerSettings.PageSize).Take(pagerSettings.PageSize).ToList();
                }

                // Calculate totals
                var totalValue = Total(items, pagerSettings.TotalBy);
                var pageTotalValue = Total(pagedItems, pagerSettings.TotalBy);

                return new PagedList<T>(
                    pagedItems,
                    items.Count,
                    pagerSettings.PageNumber,
                    pagerSettings.PageSize);
            }

            return new PagedList<T>(items, items.Count, 1, items.Count);
        }

        public static List<T> Paginate<T>(List<T> items, int pageNumber, int pageSize, bool paginate)
        {
            var pageItems = items;

            if (paginate)
            {
                pageItems = items.Skip((pageNumber - 1)*pageSize).Take(pageSize).ToList();
            }

            return pageItems;
        }

        public static List<T> Sort<T>(List<T> items, string sortBy, SortDirection sortDirection)
        {
            var sortedItems = items;

            if (!String.IsNullOrEmpty(sortBy))
            {
                var sorter = new Sorter<T>();
                sortedItems = sorter.Sort(items.AsQueryable(), sortBy, sortDirection).ToList();
            }

            return sortedItems;
        }

        public static decimal Total<T>(List<T> items, string totalBy)
        {
            var total = 0M;

            if (!String.IsNullOrEmpty(totalBy))
            {
                var parameterExpression = Expression.Parameter(typeof(T), "item");
                var totalByExpression = Expression.Property(parameterExpression, totalBy);

                var lambdaExpression = Expression.Lambda<Func<T, decimal>>
                    (Expression.Convert(totalByExpression, typeof(decimal)), parameterExpression);

                total = items.AsQueryable<T>().Sum<T>(lambdaExpression);
            }

            return total;
        }

    }
}