using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OCTiS.Knx.Model.Filtering
{
    public enum FilterKinds
    {
        Everything,
        Building,
        Topologi,
        GroupAddress,
        GroupAddressDescription,
        ObjectDescription,
        DeviceDescription,
        ProductDescription,
        ReadFlag,
        WriteFlag
    }

    public enum FilterTypes
    {
        HasValue,
        Match,
        EqualTo,
        Contains
    }

    public enum FilterActions
    {
        Include,
        Exclude
    }

    public class FilterDefinition
    {
        public Guid Id { get; set; }
        public FilterKinds Kind
        {
            get 
            {
                if (string.IsNullOrEmpty(KindValue))
                    return default(FilterKinds);
                return (FilterKinds)Enum.Parse(typeof(FilterKinds), KindValue); 
            }
            set { KindValue = value.ToString(); }
        }
        public string KindValue { get; set; }
        public FilterTypes Type 
        {
            get 
            {
                if (string.IsNullOrEmpty(TypeValue))
                    return default(FilterTypes);
                return (FilterTypes)Enum.Parse(typeof(FilterTypes), TypeValue); 
            }
            set { TypeValue = value.ToString(); }
        }
        public string TypeValue { get; set; }
        public FilterActions Action 
        {
            get 
            {
                if (string.IsNullOrEmpty(ActionValue))
                    return default(FilterActions);
                return (FilterActions)Enum.Parse(typeof(FilterActions), ActionValue); 
            }
            set { ActionValue = value.ToString(); }
        }
        public string ActionValue { get; set; }
        public string Parameter { get; set; }

        public FilterDefinition()
        {
            Id = Guid.NewGuid();
        }

        public FilterDefinition(FilterKinds kind, FilterTypes type, FilterActions action, string param)
        {
            Id = Guid.NewGuid();
            Kind = kind;
            Type = type;
            Action = action;
            Parameter = param;
        }

        private bool Match(string value)
        {
            switch (Type)
            {
                case FilterTypes.HasValue:
                    return !string.IsNullOrEmpty(value);
                case FilterTypes.Match:
                    return Regex.IsMatch(value, Parameter);
                case FilterTypes.EqualTo:
                    return value == Parameter;
                case FilterTypes.Contains:
                    return value != null && (Parameter == null || value.Contains(Parameter));
                default:
                    throw new NotImplementedException("Filter Type is unknown");
            }
        }

        private bool Act(bool res)
        {
            switch (Action)
            {
                case FilterActions.Include:
                    return res;
                case FilterActions.Exclude:
                    return !res;
                default:
                    throw new NotImplementedException("Filter Action is unknown");
            }
        }

        private static IEnumerable<string> IterateBuilding(BuildingPart part)
        {
            if (part == null)
                yield break;
            yield return part.Name;
            yield return part.Description;
            foreach (var item in IterateBuilding(part.Parent))
                yield return item;
        }

        private static IEnumerable<string> IterateTopology(DeviceInstance dev)
        {
            if (dev == null || dev.Line == null)
                yield break;
            yield return dev.Line.Name;
            yield return dev.Line.Description;
            if (dev.Line.Area == null)
                yield break;
            yield return dev.Line.Area.Name;
            yield return dev.Line.Area.Description;
            if (dev.Line.Area.Installation == null)
                yield break;
            yield return dev.Line.Area.Installation.Name;
        }

        private static IEnumerable<string> IterateGroupAddresses(GroupAddressInstance ga)
        {
            if (ga.Ref == null)
                yield break;
            yield return ga.Ref.ToString();
        }

        private static IEnumerable<string> IterateGroupAddressDescriptions(GroupAddressInstance ga)
        {
            if (ga.Ref == null)
                yield break;
            yield return ga.Ref.Name;
            if (ga.Ref.ParentRange == null)
                yield break;
            yield return ga.Ref.ParentRange.Name;
            yield return ga.Ref.ParentRange.Description;
            if (ga.Ref.ParentRange.ParentRange == null)
                yield break;
            yield return ga.Ref.ParentRange.ParentRange.Name;
            yield return ga.Ref.ParentRange.ParentRange.Description;
        }

        private static IEnumerable<string> IterateObject(ComObjectInstance obj)
        {
            return new string[] { obj["Name"].Value.ToString(), obj["Text"].Value.ToString() };
        }

        private static IEnumerable<string> IterateDevice(ComObjectInstance obj)
        {
            return new string[] { obj.ParentDevice.Name, obj.ParentDevice.Description };
        }

        private static IEnumerable<string> IterateProduct(ComObjectInstance obj)
        { 
            return new string[] { obj.ParentDevice.Product.Text };
        }

        private static IEnumerable<string> IterateReadFlag(ComObjectInstance obj)
        { 
            return new string[] { obj["ReadFlag"].Value.ToString() };
        }

        private static IEnumerable<string> IterateWriteFlag(ComObjectInstance obj)
        {
            return new string[] { obj["WriteFlag"].Value.ToString() };
        }

        private IEnumerable<string> _Iterate(ComObjectInstance obj, FilterKinds kind)
        {
            switch (kind)
            {
                case FilterKinds.Everything:
                    return _Iterate(obj, FilterKinds.Building)
                        .Union(_Iterate(obj, FilterKinds.DeviceDescription))
                        .Union(_Iterate(obj, FilterKinds.GroupAddress))
                        .Union(_Iterate(obj, FilterKinds.ProductDescription))
                        .Union(_Iterate(obj, FilterKinds.Topologi))
                        .Union(_Iterate(obj, FilterKinds.ObjectDescription))
                        .Distinct();
                case FilterKinds.Building:
                    return IterateBuilding(obj.ParentDevice.Building);
                case FilterKinds.Topologi:
                    return IterateTopology(obj.ParentDevice);
                case FilterKinds.GroupAddress:
                    return obj.GroupAddresses.SelectMany(row => IterateGroupAddresses(row));
                case FilterKinds.GroupAddressDescription:
                    return obj.GroupAddresses.SelectMany(row => IterateGroupAddressDescriptions(row));
                case FilterKinds.ObjectDescription:
                    return IterateObject(obj);
                case FilterKinds.DeviceDescription:
                    return IterateDevice(obj);
                case FilterKinds.ProductDescription:
                    return IterateProduct(obj);
                case FilterKinds.ReadFlag:
                    return IterateReadFlag(obj);
                case FilterKinds.WriteFlag:
                    return IterateWriteFlag(obj);
                default:
                    throw new NotImplementedException("Filter Kind is unknown");
            }
        }

        public IEnumerable<string> Iterate(ComObjectInstance obj)
        {
            return _Iterate(obj, Kind).Where(row => !string.IsNullOrEmpty(row));
        }

        private IEnumerable<string> _Iterate(GroupAddressInstance ga)
        {
            switch (Kind)
            {
                case FilterKinds.GroupAddress:
                    return IterateGroupAddresses(ga);
                case FilterKinds.GroupAddressDescription:
                    return IterateGroupAddressDescriptions(ga);
                default:
                    throw new NotImplementedException("Filter Kind is not valid");
            }
        }

        public IEnumerable<string> Iterate(GroupAddressInstance obj)
        {
            return _Iterate(obj).Where(row => !string.IsNullOrEmpty(row));
        }

        private bool Match(IEnumerable<string> strings)
        {
            return strings.Any(row => Match(row));
        }

        public IEnumerable<ComObjectInstance> ApplyFilter(IEnumerable<ComObjectInstance> list)
        {
            return list.Where(GetObjectFilter());
        }

        public IEnumerable<GroupAddressInstance> ApplyFilter(IEnumerable<GroupAddressInstance> list)
        {
            return list.Where(GetGroupFilter());
        }

        public Func<ComObjectInstance, bool> GetObjectFilter()
        {
            return (Func<ComObjectInstance, bool>)(row => Act(Match(Iterate(row))));
        }

        public Func<GroupAddressInstance, bool> GetGroupFilter()
        {
            return (Func<GroupAddressInstance, bool>)(row => Act(Match(Iterate(row))));
        }
    }

    public static class FilterExtensions
    {
        public static IEnumerable<string> Split(string data)
        {
            string text = data;
            while (text != null && text.Length > 0)
            {
                if (text.Length >= 2 && text[0] == '"' && text.Substring(1).Contains('"'))
                {
                    yield return text.Substring(1, text.Substring(1).IndexOf('"'));
                    text = text.Substring(text.Substring(1).IndexOf('"') + 2);
                }
                else if (text.Contains(' '))
                {
                    yield return text.Substring(0, text.IndexOf(' '));
                    text = text.Substring(text.IndexOf(' ') + 1);
                }
                else
                {
                    yield return text;
                    break;
                }
            }
        }

        public static IEnumerable<ComObjectInstance> ApplyFilter(this IEnumerable<ComObjectInstance> list, FilterDefinition filter)
        {
            return filter.ApplyFilter(list);
        }

        public static IEnumerable<GroupAddressInstance> ApplyFilter(this IEnumerable<GroupAddressInstance> list, FilterDefinition filter)
        {
            return filter.ApplyFilter(list);
        }

        private static IEnumerable<GroupAddressInstance> _ApplyFilters(this IEnumerable<GroupAddressInstance> list, IEnumerable<FilterDefinition> filters, bool permissive)
        {
            var fs = filters.Select(row => row.GetGroupFilter());
            if (fs.Count() == 0)
                return list;
            if (permissive)
                return list.Where(row => fs.Any(filter => filter(row)));
            else
                return list.Where(row => fs.All(filter => filter(row)));
        }
        
        private static IEnumerable<ComObjectInstance> _ApplyFilters(this IEnumerable<ComObjectInstance> list, IEnumerable<FilterDefinition> filters, bool permissive)
        {
            var fs = filters.Select(row => row.GetObjectFilter());
            if (fs.Count() == 0)
                return list;
            if (permissive)
                return list.Where(row => fs.Any(filter => filter(row)));
            else
                return list.Where(row => fs.All(filter => filter(row)));
        }

        public static List<FilteredResult> ApplyFilters(this IEnumerable<ComObjectInstance> list, IEnumerable<FilterDefinition> filters, bool permissive)
        {
            var firstPass = filters.Where(row => row.Kind == FilterKinds.GroupAddress || row.Kind == FilterKinds.GroupAddressDescription);
            var secondPass = filters.Where(row => row.Kind != FilterKinds.GroupAddress && row.Kind != FilterKinds.GroupAddressDescription);

            return list
                .SelectMany(row => row.GroupAddresses)
                .OrderBy(row => row.Ref.Value)
                .Distinct(GroupAddressInstance.EqualityComparer)
                ._ApplyFilters(firstPass, permissive)
                .Select(row => new FilteredResult() { GroupAddress = row,
                    ComObjects = row.Ref.ComObjectInstances
                        ._ApplyFilters(secondPass, permissive)
                        .ToList()
                })
                .ToList();
        }
    }

    public struct FilteredResult
    {
        public GroupAddressInstance GroupAddress { get; set; }
        public List<ComObjectInstance> ComObjects { get; set; }
    }
}
