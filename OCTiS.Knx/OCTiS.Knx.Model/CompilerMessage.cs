using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCTiS.Knx.Model
{
    public enum WarningSeverity
    {
        Low = 1,
        Significant = 2,
        Breaking = 3
    }

    public class CompilerMessage
    {
        public string Message { get; set; }
        public WarningSeverity Severity { get; set; }
    }

    public class InheritedCompilerMessage : CompilerMessage
    {

        public CompilerFunction ParentFunction { get; set; }

        private IEnumerable<CompilerMessage> _messages;

        public IEnumerable<CompilerMessage> Messages
        {
            get { return _messages; }
            set 
            { 
                _messages = value;
                Severity = _messages.Max(row => row.Severity);
            }
        }
    }

    public static class CompilerExtensions
    {
        public class CompilerMessageComparer : IEqualityComparer<CompilerMessage>
        {

            public bool Equals(CompilerMessage x, CompilerMessage y)
            {
                if (x is InheritedCompilerMessage && y is InheritedCompilerMessage)
                {
                    var ix = x as InheritedCompilerMessage;
                    var iy = y as InheritedCompilerMessage;
                    return ix.ParentFunction == iy.ParentFunction;
                }
                if (x is InheritedCompilerMessage || y is InheritedCompilerMessage)
                    return false;
                return x.Message == y.Message && x.Severity == y.Severity;
            }

            public int GetHashCode(CompilerMessage obj)
            {
                if (obj is InheritedCompilerMessage)
                {
                    var io = obj as InheritedCompilerMessage;
                    return io.ParentFunction.GetHashCode();
                }
                return obj.Message.GetHashCode() + obj.Severity.GetHashCode();
            }
        }

        public static IEnumerable<CompilerMessage> Compile(this CompilerFunction f)
        {
            List<CompilerMessage> result = new List<CompilerMessage>();
            //Match datapointtypes
            DatapointType headType = f.MainType;
            var os = f.ComObjects;
            if (f.ContainsLoop())
            {
                result.Add(new CompilerMessage() { Severity = WarningSeverity.Breaking, Message = "The function contains itself as a sub function. This will result in overflow at execution time." });
                return result;
            }
            if (headType != null)
            {

                var subs = f.GetSubFunctions();
                var subTypes = subs.Where(row => row.MainType != null)
                    .Select(row => row.MainType);
                if (subTypes.Any(row => row.Min > headType.Min)
                    || subTypes.Any(row => row.Max < headType.Max))
                    result.Add(new CompilerMessage() { Severity = WarningSeverity.Significant, Message = "One or more sub functions has an incompatible data range." });
                if (subTypes.Any(row => row.Number != headType.Number))
                    result.Add(new CompilerMessage() { Severity = WarningSeverity.Low, Message = "One or more sub functions has an incompatible datapoint type." });
                if (subs.Any(row => row.MainType == null))
                    result.Add(new CompilerMessage() { Severity = WarningSeverity.Significant, Message = "One or more sub functions has an unset datapoint type." });

                //Match with comObjects
                if (os.Any(row => row.Bits != headType.SizeInBits))
                    result.Add(new CompilerMessage() { Severity = WarningSeverity.Low, Message = "One or more included knx objects might have an incompatible datapoint type" });
            }
            else
                result.Add(new CompilerMessage() { Severity = WarningSeverity.Significant, Message = "The function does not have a datapoint type." });
            if (os.Any(row => row.GroupAddresses == null || row.GroupAddresses.Count == 0))
                result.Add(new CompilerMessage() { Severity = WarningSeverity.Breaking, Message = "One or more of the included knx objects is not addressable" });
            if (f.ComObjects.Count() != f.GetAffectedComObjects().Count())
                result.Add(new CompilerMessage() { Severity = WarningSeverity.Significant, Message = "The function might address knx objects with shared groupaddresses and will therefore cause unforseen consequences." });

            var subResult = f.SubFunctions.Select(row => new { Func= row, Messages = row.Compile() }).Where(row => row.Messages.Count() > 0).Select(row => new InheritedCompilerMessage() { 
                ParentFunction = row.Func, 
                Messages = row.Messages,
                Message = "Inherited"});

            return result.Union(subResult, new CompilerMessageComparer()).Distinct();
        }

        public static IEnumerable<CompilerFunction> GetSubFunctions(this CompilerFunction f)
        {
            return f.SubFunctions.SelectMany(f1 => f1.GetSubFunctions());
        }

        public static IEnumerable<ComObjectInstance> GetAffectedComObjects(this CompilerFunction f)
        {
            return f.ComObjects.Where(row => row.GroupAddresses != null).SelectMany(row => row.GroupAddresses).SelectMany(row => row.Ref.ComObjectInstances).Distinct();
        }

        public static bool ContainsLoop(this CompilerFunction f, CompilerFunction parent = null)
        {
            CompilerFunction toCompare = parent ?? f;
            foreach (var sf in f.SubFunctions)
            {
                if (sf == toCompare)
                    return true;
                else if (sf.ContainsLoop(toCompare))
                    return true;
            }
            return false;
        }
    }
}
