using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCTiS.Knx.Model
{
	public class Product
	{
		public string Id { get; set; }
		public string Text { get; set; }
		public string Name { get; set; }
		public string Program { get; set; }
		public string Manufacturer { get; set; }
		public string OrderNumber { get; set; }
		public string HasApplicationProgram { get; set; }
		public string IsCoupler { get; set; }
		public string HasIndividualAddress { get; set; }

		public string ApplicationProgramRefId { get; set; }

		private List<DeviceInstance> _devices;
		public List<DeviceInstance> Devices
		{
			get { return _devices; }
			set
			{
				if (_devices != null)
				{
					foreach (var device in _devices)
						device.Product = null;
				}
				_devices = value;
				if (_devices != null)
				{
					foreach (var device in _devices)
						device.Product = this;
				}
			}
		}
	}
}
