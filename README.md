TO RUN  
-Clone this repository  
-Open with Visual Studio  
-Build and run the project with a single argument that points to a .java file  
    e.g.: C:/path/to/file.java  
-The program will print the structure of the source code contained in the .java file  

TO CREATE AN IMPLEMENTATION FOR A LANGUAGE  
-Install the Antlr4 plugin https://www.antlr.org/tools.html  
-Generate a Lexer and a Parser from a Grammar file (.g4)  
  - right click a _Lexer.g4 file and go to Tools > Configure ANTLR...    
  - set the language (in this case "CSharp")  
  - right click the _Lexer.g4 again and run Tools > Generate ANTLR Recognizer
  - repeat for a _Parser.g4  
    
-Create Listeners for each type of node in the language's abstract syntax tree  
  - the visitor classes instantiate a Lexer and a Parser to parse a string of code  
  - an example is provided in Java  

REFERENCE  
-https://tomassetti.me/antlr-mega-tutorial  
-https://github.com/antlr/antlr4  
-https://www.antlr.org/tools.html  