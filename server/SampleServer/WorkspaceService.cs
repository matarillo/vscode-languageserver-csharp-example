using LanguageServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageServer.Parameters.Workspace;

namespace SampleServer
{
    public class WorkspaceService : WorkspaceServiceTemplate
    {
        public App App => (App)Connection;

        protected override void DidChangeConfiguration(DidChangeConfigurationParams @params)
        {
            App.DidChangeConfiguration(@params);
        }

        protected override void DidChangeWatchedFiles(DidChangeWatchedFilesParams @params)
        {
            App.DidChangeWatchedFiles(@params);
        }
    }
}
