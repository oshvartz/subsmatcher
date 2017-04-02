using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg.API;
using CookComputing.XmlRpc;
using SubtitleTools.Infrastructure.Core.OpenSubtitlesOrg;

namespace OpenSubtitlesOrgMatcher
{
    public class LoginTokenHolder
    {
        private static LoginTokenHolder _instance = new LoginTokenHolder();

        readonly IOpenSubtitlesDb _client;
        string _loginToken;

        public LoginTokenHolder()
        {
            _client = XmlRpcProxyGen.Create<IOpenSubtitlesDb>();
            _client.Expect100Continue = false;
            _client.UserAgent = "SubtitleTools";
            _client.Url = OpenSubtitlesXmlRpc.OPENSUBTITLES_URI;

            var loginInfo = _client.LogIn(string.Empty, string.Empty, string.Empty, UA.MyUA);
            var status = loginInfo.status;
            if (string.IsNullOrWhiteSpace(status) || status != "200 OK")
            {
                throw new Exception("Couldn't login.");
            }
            _loginToken = loginInfo.token;
        }

        public static LoginTokenHolder Instance { get { return _instance; } }

        public string LoginToken
        {
            get
            {
                return _loginToken;
            }
        }



    }
}
