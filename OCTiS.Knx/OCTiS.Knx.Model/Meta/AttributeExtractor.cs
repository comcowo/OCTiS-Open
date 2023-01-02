using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCTiS.Knx.Model.Meta
{
    public enum AttributePath
    {
        GroupAddressName,
        ObjectName,
        DeviceName,
        ProductName,
        Building,
        Room,
        Installation
    }

    public class AttributeExtractor : MetaExtractor
    {
        public AttributePath Path 
        {
            get 
            {
                if (string.IsNullOrEmpty(PathValue))
                    return default(AttributePath);
                return (AttributePath)Enum.Parse(typeof(AttributePath), PathValue); 
            }
            set { PathValue = value.ToString(); }
        }
        public string PathValue { get; set; }
        public override void Extract(Filtering.FilteredResult fr, ref KnxPort meta)
        {
            switch (Path)
            {
                case AttributePath.Installation:
                    meta.MetaData[AttributeName] = fr.ComObjects.Select(row => row.ParentDevice.Building.Parent.Parent.Name).Where(row => !string.IsNullOrWhiteSpace(row)).FirstOrDefault();
                    break;
                case AttributePath.Building:
                    meta.MetaData[AttributeName] = fr.ComObjects.Select(row => row.ParentDevice.Building.Parent.Name).Where(row => !string.IsNullOrWhiteSpace(row)).FirstOrDefault();
                    break;
                case AttributePath.Room:
                    meta.MetaData[AttributeName] = fr.ComObjects.Select(row => row.ParentDevice.Building.Name).Where(row => !string.IsNullOrWhiteSpace(row)).FirstOrDefault();
                    break;
                case AttributePath.GroupAddressName:
                    meta.MetaData[AttributeName] = fr.GroupAddress.Ref.Name;
                    break;
                case AttributePath.ObjectName:
                    meta.MetaData[AttributeName] = fr.ComObjects.Select(row => (row["Name"].Value ?? row["Text"].Value).ToString()).FirstOrDefault();
                    break;
                case AttributePath.DeviceName:
                    meta.MetaData[AttributeName] = fr.ComObjects.Select(row => row.ParentDevice.Name).Where(row => !string.IsNullOrWhiteSpace(row)).FirstOrDefault();
                    break;
                case AttributePath.ProductName:
                    meta.MetaData[AttributeName] = fr.ComObjects.Select(row => row.ParentDevice.Product.Text).Where(row => !string.IsNullOrWhiteSpace(row)).FirstOrDefault();
                    break;
                default:
                    break;
            }
        }
    }
}
