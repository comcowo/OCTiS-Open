using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace OCTiS.Knx.Model
{
	public sealed class DeviceInstance
	{
		public Line Line { get; set; }
		public BuildingPart Building { get; set; }

		public string Name { get; set; }

		public string Id { get; set; }

		public string Description { get; set; }

		public string Comment { get; set; }

		public short Address { get; set; }

		public string Hardware2ProgramRefId { get; set; }

		public string ProductRefId { get; set; }
		public Product Product { get; set; }

		public string IsComVis { get; set; }
		public string CommunicationPartLoaded { get; set; }
		public string MediumConfigLoaded { get; set; }
		public string ParametersLoaded { get; set; }
		public string ApplicationProgramLoaded { get; set; }
		public string IndividualAddressLoaded { get; set; }

		public string FullAddress
		{
			get
			{
				return string.Format("{0}.{1}.{2}", Line.Area.Address, Line.Address, Address);
			}
		}

		private List<ComObjectInstance> _comObjectInstances;
		public List<ComObjectInstance> ComObjectInstances
		{
			get { return _comObjectInstances; }
			set
			{
				if (_comObjectInstances != null)
					foreach (var coi in _comObjectInstances)
						coi.ParentDevice = null;
				_comObjectInstances = value;
				if (_comObjectInstances != null)
					foreach (var coi in _comObjectInstances)
						coi.ParentDevice = this;
			}
		}
	}
}
