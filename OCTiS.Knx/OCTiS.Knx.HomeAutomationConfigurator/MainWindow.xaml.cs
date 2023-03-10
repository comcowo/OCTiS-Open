using DevExpress.CodeParser;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.RichEdit;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Text.RegularExpressions;
using DevExpress.Office.Utils;

namespace OCTiS.Knx.HomeAutomationConfigurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void richEditControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Use service substitution to register a custom service that implements highlighting.
            richEditControl.Options.Search.RegExResultMaxGuaranteedLength = 500;
            richEditControl.ReplaceService<ISyntaxHighlightService>(new CustomSyntaxHighlightService(richEditControl.Document));
            richEditControl.ActiveViewType = RichEditViewType.Draft;
            richEditControl.Document.Sections[0].Page.Width = Units.InchesToDocumentsF(80f);
            richEditControl.Document.DefaultCharacterProperties.FontName = "Courier New";
        }
        public class CustomSyntaxHighlightService : ISyntaxHighlightService
        {
            readonly Document document;

            Regex _keywords;
            Regex _quotedString = new Regex(@"""((\\[^\n]|[^""\n])*)""");
            Regex _commentedString = new Regex(@"#(.*?)\r?\n");
            Regex _categoryString = new Regex(@"\w*:\r?\n");

            public CustomSyntaxHighlightService(Document document)
            {
                this.document = document;
                string[] keywords = { "!include", "!include_dir_merge_named" };
                this._keywords = new Regex(@"(" + string.Join("|", keywords.Select(w => Regex.Escape(w))) + @")\b");
            }
            public void ForceExecute()
            {
                Execute();
            }
            public void Execute()
            {
                List<SyntaxHighlightToken> tSqltokens = ParseTokens();
                document.ApplySyntaxHighlight(tSqltokens);
            }

            private List<SyntaxHighlightToken> ParseTokens()
            {
                List<SyntaxHighlightToken> tokens = new List<SyntaxHighlightToken>();

                // search for quoted strings
                DocumentRange[] ranges = document.FindAll(_quotedString).GetAsFrozen() as DocumentRange[];
                for (int i = 0; i < ranges.Length; i++)
                {
                    tokens.Add(CreateToken(ranges[i].Start.ToInt(), ranges[i].End.ToInt(), Color.Red));
                }

                //Extract all keywords
                ranges = document.FindAll(_keywords).GetAsFrozen() as DocumentRange[];
                for (int j = 0; j < ranges.Length; j++)
                {
                    if (!IsRangeInTokens(ranges[j], tokens))
                        tokens.Add(CreateToken(ranges[j].Start.ToInt(), ranges[j].End.ToInt(), Color.BlueViolet));
                }

                //Find all comments
                ranges = document.FindAll(_commentedString).GetAsFrozen() as DocumentRange[];
                for (int j = 0; j < ranges.Length; j++)
                {
                    if (!IsRangeInTokens(ranges[j], tokens))
                        tokens.Add(CreateToken(ranges[j].Start.ToInt(), ranges[j].End.ToInt(), Color.Green));
                }

                //Find all categories
                ranges = document.FindAll(_categoryString).GetAsFrozen() as DocumentRange[];
                for (int j = 0; j < ranges.Length; j++)
                {
                    if (!IsRangeInTokens(ranges[j], tokens))
                        tokens.Add(CreateToken(ranges[j].Start.ToInt(), ranges[j].End.ToInt(), Color.Blue));
                }
                // order tokens by their start position
                tokens.Sort(new SyntaxHighlightTokenComparer());

                // fill in gaps in document coverage
                tokens = CombineWithPlainTextTokens(tokens);
                return tokens;
            }

            //Parse the remaining text into tokens:
            List<SyntaxHighlightToken> CombineWithPlainTextTokens(List<SyntaxHighlightToken> tokens)
            {
                List<SyntaxHighlightToken> result = new List<SyntaxHighlightToken>(tokens.Count * 2 + 1);
                int documentStart = this.document.Range.Start.ToInt();
                int documentEnd = this.document.Range.End.ToInt();
                if (tokens.Count == 0)
                    result.Add(CreateToken(documentStart, documentEnd, Color.Black));
                else
                {
                    SyntaxHighlightToken firstToken = tokens[0];
                    if (documentStart < firstToken.Start)
                        result.Add(CreateToken(documentStart, firstToken.Start, Color.Black));
                    result.Add(firstToken);
                    for (int i = 1; i < tokens.Count; i++)
                    {
                        SyntaxHighlightToken token = tokens[i];
                        SyntaxHighlightToken prevToken = tokens[i - 1];
                        if (prevToken.End != token.Start)
                            result.Add(CreateToken(prevToken.End, token.Start, Color.Black));
                        result.Add(token);
                    }
                    SyntaxHighlightToken lastToken = tokens[tokens.Count - 1];
                    if (documentEnd > lastToken.End)
                        result.Add(CreateToken(lastToken.End, documentEnd, Color.Black));
                }
                return result;
            }

            //Create a token from the retrieved range and specify its forecolor
            SyntaxHighlightToken CreateToken(int start, int end, Color foreColor)
            {
                SyntaxHighlightProperties properties = new SyntaxHighlightProperties();
                properties.ForeColor = foreColor;
                return new SyntaxHighlightToken(start, end - start, properties);
            }

            //Check whether tokens intersect each other
            private bool IsRangeInTokens(DocumentRange range, List<SyntaxHighlightToken> tokens)
            {
                return tokens.Any(t => IsIntersect(range, t));
            }
            bool IsIntersect(DocumentRange range, SyntaxHighlightToken token)
            {
                int start = range.Start.ToInt();
                if (start >= token.Start && start < token.End)
                    return true;
                int end = range.End.ToInt() - 1;
                if (end >= token.Start && end < token.End)
                    return true;
                if (start < token.Start && end >= token.End)
                    return true;
                return false;
            }
        }

        //Compare token's initial positions to sort them
        public class SyntaxHighlightTokenComparer : IComparer<SyntaxHighlightToken>
        {
            public int Compare(SyntaxHighlightToken x, SyntaxHighlightToken y)
            {
                return x.Start - y.Start;
            }
        }
    }
}
