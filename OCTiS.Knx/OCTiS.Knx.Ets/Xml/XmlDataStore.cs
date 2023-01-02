using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace OCTiS.Knx.Ets.Xml
{

    public class XmlDataStore
    {
        protected XNamespace ns;
        private XDocument _document;

        public XDocument Document
        {
            get { return _document; }
        }

        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
        }

        public XmlDataStore(Stream s, string fileName)
        {
            _document = XDocument.Load(s);
            _fileName = fileName;
            ns = _document.Root.Name.Namespace;
        }
    }
}
