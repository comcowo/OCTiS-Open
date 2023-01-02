using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace OCTiS.Knx.Model
{
    public enum MeasureTypes
    {
        Entry,
        Converter,
        Aggregator
    }

    public class Measure : CompilerFunctionBase
    {
        private MeasureTypes? _type;
        public MeasureTypes? Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }

    }
}
