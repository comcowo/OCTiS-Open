using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace OCTiS.Knx.Model
{
    public class CompilerFunctionBase : INotifyPropertyChanged
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        private int _address;

        public int Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }

        private DatapointType _mainType = null;
        public DatapointType MainType
        {
            get { return _mainType; }
            set
            {
                if (_mainType != value && value != null)
                    SubType = value.SubTypes.FirstOrDefault(row => row.Default);
                _mainType = value;
                OnPropertyChanged("MainType");
            }
        }

        private DatapointSubType _subType;
        public DatapointSubType SubType
        {
            get { return _subType; }
            set
            {
                _subType = value;
                OnPropertyChanged("SubType");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
