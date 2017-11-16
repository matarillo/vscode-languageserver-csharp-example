# README
## This is the README for the "languageprovider-sample" 
-------------------

This folder contains a sample VS code extension that demonstrates an extension that runs a language server

The extension observes all 'plaintext' documents (documents from all editors not associated with a language)
and uses the server to provide validation and completion proposals.

The code for the extension is in the 'client' folder. It uses the 'vscode-languageclient' node module to launch the language server.

The language server is located in the 'server' folder. 

# Modification from the original
Sample language server is implemented in C#.

# How to run locally
* `npm install` to initialize the extension and the server
* `npm run compile` to compile the extension and the server
* open this folder in VS Code. In the Debug viewlet, run 'Launch Client' from drop-down to launch the extension and attach to the extension.
* create a file `test.txt`, and type `typescript`. You should see a validation error.
* to debug the server use external debugger such as Visual Studio.
* set breakpoints in the client or the server.