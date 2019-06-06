using System;
using System.Collections.Generic;
using System.Text;
using RAGE;

namespace GrandTheftChallenge_Client.Auth
{
    public class Auth : Events.Script
    {
        public Auth()
        {
            Events.Add("LoginServer", login_server);
            Events.Add("RegisterServer", register_server);
        }

        private void login_server(object[] args)
        {
            Events.CallRemote("login_auth", (string)args[0], (string)args[1]);
        }

        private void register_server(object[] args)
        {
            Events.CallRemote("register_auth", (string)args[0], (string)args[1], (string)args[2]);
        }
    }
}
