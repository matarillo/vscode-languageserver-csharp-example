using LanguageServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageServer;
using LanguageServer.Parameters.General;
using LanguageServer.Parameters;

namespace SampleServer
{
    public class GeneralService : GeneralServiceTemplate
    {
        public App App => (App)Connection;

        protected override Result<InitializeResult, ResponseError<InitializeErrorData>> Initialize(InitializeParams @params)
        {
            return App.Initialize(@params);
        }
    }
}
