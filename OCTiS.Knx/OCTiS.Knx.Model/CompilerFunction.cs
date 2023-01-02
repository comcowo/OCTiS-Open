using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace OCTiS.Knx.Model
{
    public class CompilerFunction : CompilerFunctionBase
    {
        public CompilerFunction()
        {
            _comObjects = new System.Collections.ObjectModel.ObservableCollection<ComObjectInstance>();
            _subFunctions = new System.Collections.ObjectModel.ObservableCollection<CompilerFunction>();
        }

        private System.Collections.ObjectModel.ObservableCollection<ComObjectInstance> _comObjects;
        private System.Collections.ObjectModel.ObservableCollection<CompilerFunction> _subFunctions;
        public IEnumerable<ComObjectInstance> ComObjects
        {
            get { return _comObjects; }
        }

        public IEnumerable<CompilerFunction> SubFunctions
        {
            get { return _subFunctions; }
        }

        public void AddSubFunction(CompilerFunction f)
        {
            if (_subFunctions.Contains(f) || f == this)
                return;
            _subFunctions.Add(f);
            OnPropertyChanged("SubFunctions");
        }

        public void RemoveSubFunction(CompilerFunction f)
        {
            if (!_subFunctions.Contains(f))
                return;
            _subFunctions.Remove(f);
            OnPropertyChanged("SubFunctions");
        }

        public void AddComObject(ComObjectInstance i)
        {
            if (_comObjects.Contains(i))
                return;
            _comObjects.Add(i);
            OnPropertyChanged("ComObjects");
        }

        public void RemoveComObject(ComObjectInstance i)
        {
            if (!_comObjects.Contains(i))
                return;
            _comObjects.Remove(i);
            OnPropertyChanged("ComObjects");
        }
    }

    public static class FunctionExtensions
    {
        public static IEnumerable<ComObjectInstance> GetAllComObjects(this CompilerFunction f)
        {
            return f.ComObjects.Union(f.GetSubFunctions().SelectMany(row => row.ComObjects));
        }
    }
}
