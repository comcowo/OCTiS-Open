using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace OCTiS.Knx.Model.Filtering
{
    public class Filter
    {
        public Filter()
        {
            Filters = new List<FilterDefinition>();
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public List<FilterDefinition> Filters { get; set; }

        public List<FilteredResult> GetFilterResult(Project project, bool permissive = true)
        {
            if (project == null)
                return new List<FilteredResult>();
            var list = project.Devices.SelectMany(row => row.ComObjectInstances);
            return list.ApplyFilters(Filters, permissive);
        }

        public static IEnumerable<string> GetSuggestions(Project project, FilterDefinition filter)
        {
            return project.Devices.SelectMany(row => row.ComObjectInstances)
                .SelectMany(row => 
                    filter.Iterate(row)
                        .Where(r1 => !string.IsNullOrEmpty(r1)))
                    .OrderBy(row => row)
                    .Distinct();
        }

    }
}