using System.Collections.Generic;
using System.IO;
using System.Linq;
using antlr_parser.Antlr4Impl.Java;
using antlr_parser.Antlr4Impl.JavaScript;
using PrimitiveCodebaseElements.Primitive;

namespace antlr_parser
{
    public static class ParserHandler
    {
        public static readonly HashSet<string> SupportedParsableFiles =
            new HashSet<string>
            {
                ".java", ".cs", ".h", ".hxx", ".hpp", ".cpp", ".c", ".cc", ".m", ".py", ".py3", ".js", ".jsx"
            };

        public static readonly HashSet<string> ReadableFiles =
            new HashSet<string>
            {
                // parsable (see above)
                ".java", ".cs", ".h", ".hxx", ".hpp", ".cpp", ".c", ".cc", ".m", ".py", ".py3",".js", ".jsx",
                // languages to be parsed in the future
                ".kt", ".sc", ".sol", ".rs",
                // data formats
                ".txt", ".md", ".html", ".json", ".xml", ".sql", ".yaml"
            };

        public static IEnumerable<ClassInfo> ClassInfoFromSourceText(
            string filePath,
            string sourceExtension,
            string sourceText)
        {
            if (!SupportedParsableFiles.Contains(sourceExtension)) return null;

            switch (sourceExtension)
            {
                case ".java":
                    return AntlrParseJava.OuterClassInfosFromJavaSource(
                        sourceText,
                        filePath);
                case ".js":
                case ".jsx":
                    return AntlrParseJavaScript.OuterClassInfosFromJavaScriptSource(
                        sourceText,
                        filePath);
                case ".cs":
                // cs
                case ".h":
                case ".hxx":
                case ".hpp":
                case ".cpp":
                case ".c":
                case ".m":
                case ".cc":
                    //cpp
                case ".py":
                case ".py3":
                    // python
                    return new List<ClassInfo>();
            }

            return null;
        }

        public static string GetTextFromFilePath(string filePath)
        {
            string ext = Path.GetExtension(filePath);
            try
            {
                return ReadableFiles.Contains(ext)
                    ? File.ReadAllText(filePath)
                    : "binary data";
            }
            catch (IOException e)
            {
                return e.Message;
            }
        }
    }
}