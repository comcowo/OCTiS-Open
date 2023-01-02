using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Pdf.Native.BouncyCastle.Utilities.Encoders;
using DevExpress.Xpf.Dialogs;
using DevExpress.Xpf.RichEdit;
using DevExpress.XtraRichEdit.API.Native;
//using DevExpress.XtraRichEdit.Model;
using OCTiS.Knx.Ets;
using OCTiS.Knx.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCTiS.Knx.HomeAutomationConfigurator
{
    [POCOViewModel]
    public class MainWindowModel
    {
        private DevExpress.XtraRichEdit.API.Native.Document _Document;
        private string _TemplateText = "# Loads default set of integrations. Do not remove.\r\ndefault_config:\r\n\r\n# Load frontend themes from the themes folder\r\nfrontend:\r\n  themes: !include_dir_merge_named themes\r\n\r\n# Text to speech\r\ntts:\r\n  - platform: google_translate\r\n\r\nautomation: !include automations.yaml\r\nscript: !include scripts.yaml\r\nscene: !include scenes.yaml\r\n# Move cursor to line under category and push button\r\nknx:\r\n  cover:\r\n\r\n  light:\r\n\r\n  switch:\r\n\r\n  sensor:";

        public List<string> ValueTypes { get; } = new List<string> { "temperature", "percent", "wind_speed_kmh", "color_temperature" };
        public virtual string ValueType { get; set; }
        public virtual ObservableCollection<GroupAddressInfo> GroupAddressInfos { get; set; }
        public ObservableCollection<GroupAddressInfo> SelectedGroupAddressInfos { get; } = new ObservableCollection<GroupAddressInfo>();

        public MainWindowModel()
        {
            SelectedGroupAddressInfos.CollectionChanged += SelectedGroupAddressInfos_CollectionChanged;
        }
        private void SelectedGroupAddressInfos_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }


        public void OpenFile()
        {
            var fileDialog = new DXOpenFileDialog();
            fileDialog.InitialDirectory = Properties.Settings.Default.OpenFilePath;
            fileDialog.Filter = "ETS Project Files|*.knxproj";
            if(fileDialog.ShowDialog() != true) return;
            Properties.Settings.Default.OpenFilePath = fileDialog.InitialDirectory;
            using (Stream s = File.Open(fileDialog.FileName, FileMode.Open))
            {
                var store = ProjectStore.Load(s);
                var model = ModelFactory.BuildModel(store);
                var groupAddressInfos = model.Project.GroupRanges
                                            .SelectMany(gr => gr.Ranges)
                                            .SelectMany(r => r.Addresses)
                                            .Select(a => new GroupAddressInfo(a)).ToList();
                var connectedGroupAddressInfos = groupAddressInfos.Where(ga => ga.HasConnections).ToList();
                GroupAddressInfos = new ObservableCollection<GroupAddressInfo>(connectedGroupAddressInfos);
            }
        }

        public bool CanInsertName() => SelectedGroupAddressInfos.Count > 0;
        public void InsertName()
        {
            foreach (var groupAddressInfo in SelectedGroupAddressInfos)
                InsertLine($"    - name: \"{groupAddressInfo.SpacePath + groupAddressInfo.AddressPath}\"");
        }
        public bool CanInsertAny(string tag) => SelectedGroupAddressInfos.Count > 0;
        public void InsertAny(string tag)
        {
            foreach (var groupAddressInfo in SelectedGroupAddressInfos)
                InsertLine($"      {tag}: \"{groupAddressInfo.GroupAddress}\"");
            SelectedGroupAddressInfos.Clear();
        }
        public bool CanInsertType(string tag) => true;
        public void InsertType(string type)
        {
            foreach (var groupAddressInfo in SelectedGroupAddressInfos)
                InsertLine($"      type: {type}");
        }
        public void DocumentLoaded(object parameter)
        {
            _Document = parameter as DevExpress.XtraRichEdit.API.Native.Document;
            InsertLine(_TemplateText);
            //string plainText = _Document.GetText(_Document.Range);
        }
        private void InsertLine(string text)
        {
            DocumentPosition pos = _Document.CaretPosition;
            SubDocument doc = pos.BeginUpdateDocument();
            doc.InsertText(pos, text + "\r\n");
            pos.EndUpdateDocument(doc);
        }
    }
}
