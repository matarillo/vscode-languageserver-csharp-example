using LanguageServer;
using LanguageServer.Json;
using LanguageServer.Parameters.TextDocument;
using LanguageServer.Server;

namespace SampleServer
{
    public class TextDocumentService : TextDocumentServiceTemplate
    {
        public App App => (App)Connection;

        protected override void DidChangeTextDocument(DidChangeTextDocumentParams @params)
        {
            App.DidChangeTextDocument(@params);
        }

        protected override void DidOpenTextDocument(DidOpenTextDocumentParams @params)
        {
            App.DidOpenTextDocument(@params);
        }

        protected override void DidCloseTextDocument(DidCloseTextDocumentParams @params)
        {
            App.DidCloseTextDocument(@params);
        }

        protected override Result<ArrayOrObject<CompletionItem, CompletionList>, ResponseError> Completion(TextDocumentPositionParams @params)
        {
            return App.Completion(@params);
        }

        protected override Result<CompletionItem, ResponseError> ResolveCompletionItem(CompletionItem @params)
        {
            return App.ResolveCompletionItem(@params);
        }
    }
}
