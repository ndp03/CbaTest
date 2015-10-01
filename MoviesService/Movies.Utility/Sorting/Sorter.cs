using System;
using System.Collections.Generic;
using System.Linq;
using Movies.Utility.Sorting;

namespace HVManager.Util.Sorting
{
    public class Sorter<T>
    {
        public IEnumerable<T> Sort(IEnumerable<T> items, string sortBy, SortDirection sortDirection)
        {
            return Sort(items, new List<SortExpression> { new SortExpression { SortBy = sortBy, SortDirection = sortDirection } });
        }

        public IEnumerable<T> Sort(IEnumerable<T> items, List<SortExpression> sortExpressions)
        {
            if (items == null || !sortExpressions.Any())
            {
                return items;
            }

            var unorderedItems = items.ToList();
            IOrderedEnumerable<T> orderedItems = null;

            for (var i = 0; i < sortExpressions.Count; i++)
            {
                var sortExpression = sortExpressions[i];

                if (sortExpression != null)
                {
                    Func<T, object> expression = item => item.GetType().GetProperty(sortExpression.SortBy).GetValue(item, null);

                    if (sortExpression.SortDirection == SortDirection.Ascending)
                    {
                        orderedItems = orderedItems == null
                            ? unorderedItems.OrderBy(expression)
                            : orderedItems.ThenBy(expression);
                    }
                    else if (sortExpression.SortDirection == SortDirection.Descending)
                    {
                        orderedItems = orderedItems == null
                            ? unorderedItems.OrderByDescending(expression)
                            : orderedItems.ThenByDescending(expression);
                    }
                }
            }

            return orderedItems ?? items;
        }
    }
}