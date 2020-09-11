TO RUN  
-Clone this repository  
-Open with Visual Studio  
-Build and run the project with a single argument that points to a .java file  
    e.g.: C:/path/to/file.java  
-The program will print the structure of the source code contained in the .java file  

TO CREATE AN IMPLEMENTATION FOR A LANGUAGE  
-Install the Antlr4 plugin https://www.antlr.org/tools.html  
-Generate a Lexer and a Parser from a Grammar file (.g4)  
  - Right click a _Lexer.g4 file and go to Tools > Configure ANTLR...    
  - Set the language for the _source files_ that will be generated (in this case "CSharp" - _not the language that is actually getting parsed_)  
  - Right click the _Lexer.g4 again and run Tools > Generate ANTLR Recognizer
  - Repeat for a _Parser.g4  
  
-Manually fix the generated files if necessary    
  - Include a BaseParser.cs file from https://github.com/antlr/grammars-v4 if required. It will be found in a "CSharp" folder inside of the desired language folder.
  - Replace token names in the generated Lexer file with their numeric equivalents (e.g. LPAREN with 73 - found in the .tokens file) if necessary.  
  - Certain function names like "Lt" might use a different runtime version name "LT"
  - The Parser constructor may have too many arguments outside of the InputStream parameter - remove them.    
  - The _input ITokenStream may be protected. The InputStream parameter from the constructor can be saved locally.  
    
-Create Listeners for each type of node in the language's abstract syntax tree  
  - The visitor classes instantiate a Lexer and a Parser to parse a string of code  
  - An example is provided in Java  

REFERENCE  
-https://tomassetti.me/antlr-mega-tutorial  
-https://github.com/antlr/antlr4  
-https://www.antlr.org/tools.html  