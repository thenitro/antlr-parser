using System;
using System.Collections.Generic;

namespace antlr_parser
{
    public class SourceCodeSnippet
    {
        readonly string text;
        string branchText;
        string diffedText;
        string colorizedText;
        
        readonly SourceCodeLanguage language;
        
        bool hasLoadedColorizedText;
        bool isDiffedText = false;
        public bool IsDiffedText => isDiffedText && text.Length != branchText.Length;
        public int AbsoluteDiff => Math.Abs(text.Length - branchText.Length);
        
        
        static readonly Dictionary<string, SourceCodeLanguage> ExtensionToLanguage =
            new Dictionary<string, SourceCodeLanguage>
            {
                {".java", SourceCodeLanguage.Java},
                {".cs", SourceCodeLanguage.CSharp},
                {".js", SourceCodeLanguage.JavaScript},
                {".jsx", SourceCodeLanguage.JavaScript},
                {".h", SourceCodeLanguage.Cpp},
                {".cpp", SourceCodeLanguage.Cpp},
                {".kt", SourceCodeLanguage.Kotlin},
                {".xml", SourceCodeLanguage.XML},
                {".sql", SourceCodeLanguage.SQL},
                {".md", SourceCodeLanguage.Markdown},
                {".html", SourceCodeLanguage.HTML},
                {".py", SourceCodeLanguage.Python},
                {".sc", SourceCodeLanguage.Scala},
                {".c", SourceCodeLanguage.C},
                {".cc", SourceCodeLanguage.Cpp},
                {".m", SourceCodeLanguage.Cpp},
                {".rs", SourceCodeLanguage.Rust},

                {".sol", SourceCodeLanguage.Solidity}
            };

        
        

        public SourceCodeSnippet(
            string text,
            SourceCodeLanguage language)
        {
            this.text = text;
            this.language = language;
        }

        public static SourceCodeSnippet Synthesized(string text) =>
            new SourceCodeSnippet(text, SourceCodeLanguage.PlainText);

        public SourceCodeSnippet(
            string text,
            string extension)
        {
            this.text = text;
            bool success = ExtensionToLanguage.TryGetValue(extension, out language);
            if (!success)
            {
                language = SourceCodeLanguage.PlainText;
            }
        }

        public void SetBranchText(string branchText)
        {
            this.branchText = branchText ?? "";

            isDiffedText = true;
            hasLoadedColorizedText = false;
        }

        public void SetDefaultText()
        {
            isDiffedText = false;
            hasLoadedColorizedText = false;
        }

        public string Text => string.IsNullOrEmpty(text)
            ? ""
            : isDiffedText
                ? branchText
                : text;

        public SourceCodeLanguage Language => language;


    }
    public enum SourceCodeLanguage
    {
        // These enum int values must match in:
        // - IntelliJ Plugin -> SQLiteOutput#dbValueForSourceCodeLanguage
        // - C# analyzer -> CsStructure#SourceCodeSnippetLanguage
        // - JS Analyzer -> sqliteOutput#JAVASCRIPT_LANGUAGE_TYPE
        PlainText = -1,

        // Core, widely-used languages
        Java = 0,
        CSharp = 1,
        JavaScript = 2,
        Cpp = 3,
        Kotlin = 4,
        XML = 5,
        SQL = 6,
        Markdown = 7,
        HTML = 8,
        Python = 9,
        Scala = 10,
        C = 11,
        CWithClasses = 12,
        ObjC = 13,
        Rust = 14,

        // Non-core, specialized languages
        Solidity = 100
    }
}