using LanguageServer;
using LanguageServer.Client;
using LanguageServer.Json;
using LanguageServer.Parameters;
using LanguageServer.Parameters.General;
using LanguageServer.Parameters.TextDocument;
using LanguageServer.Parameters.Workspace;
using System;
using System.Collections.Generic;
using System.IO;

namespace SampleServer
{
    public class App : Connection
    {
        private Uri _workerSpaceRoot;
        private int _maxNumberOfProblems;
        private Proxy _proxy;
        private TextDocumentManager _documents;

        public App(Stream input, Stream output)
            : base(input, output)
        {
            _proxy = new Proxy(this);
            _documents = new TextDocumentManager();
            _documents.Changed += Documents_Changed;
        }

        private void Documents_Changed(object sender, TextDocumentChangedEventArgs e)
        {
            ValidateTextDocument(e.Document);
        }

        internal Result<InitializeResult, ResponseError<InitializeErrorData>> Initialize(InitializeParams @params)
        {
            _workerSpaceRoot = @params.rootUri;
            var result = new InitializeResult
            {
                capabilities = new ServerCapabilities
                {
                    textDocumentSync = TextDocumentSyncKind.Full,
                    completionProvider = new CompletionOptions
                    {
                        resolveProvider = true
                    }
                }
            };
            return Result<InitializeResult, ResponseError<InitializeErrorData>>.Success(result);
        }

        internal void DidOpenTextDocument(DidOpenTextDocumentParams @params)
        {
            _documents.Add(@params.textDocument);
            Logger.Instance.Log($"{@params.textDocument.uri} opened.");
        }

        internal void DidChangeTextDocument(DidChangeTextDocumentParams @params)
        {
            _documents.Change(@params.textDocument.uri, @params.textDocument.version, @params.contentChanges);
            Logger.Instance.Log($"{@params.textDocument.uri} changed.");
        }

        internal void DidCloseTextDocument(DidCloseTextDocumentParams @params)
        {
            _documents.Remove(@params.textDocument.uri);
            Logger.Instance.Log($"{@params.textDocument.uri} closed.");
        }

        internal void DidChangeConfiguration(DidChangeConfigurationParams @params)
        {
            _maxNumberOfProblems = @params?.settings?.languageServerExample?.maxNumberOfProblems ?? 100;
            foreach (var document in _documents.All)
            {
                ValidateTextDocument(document);
            }
        }

        private void ValidateTextDocument(TextDocumentItem document)
        {
            var diagnostics = new List<Diagnostic>();
            var lines = document.text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var problems = 0;
            for (var i = 0; i < lines.Length && problems < _maxNumberOfProblems; i++)
            {
                var line = lines[i];
                var index = line.IndexOf("typescript");
                if (index >= 0)
                {
                    problems++;
                    diagnostics.Add(new Diagnostic
                    {
                        severity = DiagnosticSeverity.Warning,
                        range = new Range
                        {
                            start = new Position { line = i, character = index },
                            end = new Position { line = i, character = index + 10 }
                        },
                        message = $"{line.Substring(index, 10)} should be spelled TypeScript",
                        source = "ex"
                    });
                }
            }

            _proxy.TextDocument.PublishDiagnostics(new PublishDiagnosticsParams
            {
                uri = document.uri,
                diagnostics = diagnostics.ToArray()
            });
        }

        internal void DidChangeWatchedFiles(DidChangeWatchedFilesParams @params)
        {
            Logger.Instance.Log("We received an file change event");
        }

        internal Result<ArrayOrObject<CompletionItem, CompletionList>, ResponseError> Completion(TextDocumentPositionParams @params)
        {
            var array = new[]
            {
                new CompletionItem
                {
                    label = "TypeScript",
                    kind = CompletionItemKind.Text,
                    data = 1
                },
                new CompletionItem
                {
                    label = "JavaScript",
                    kind = CompletionItemKind.Text,
                    data = 2
                }
            };
            return Result<ArrayOrObject<CompletionItem, CompletionList>, ResponseError>.Success(array);
        }

        internal Result<CompletionItem, ResponseError> ResolveCompletionItem(CompletionItem @params)
        {
            if (@params.data == 1)
            {
                @params.detail = "TypeScript details";
                @params.documentation = "TypeScript documentation";
            }
            else if (@params.data == 2)
            {
                @params.detail = "JavaScript details";
                @params.documentation = "JavaScript documentation";
            }
            return Result<CompletionItem, ResponseError>.Success(@params);
        }
    }
}
