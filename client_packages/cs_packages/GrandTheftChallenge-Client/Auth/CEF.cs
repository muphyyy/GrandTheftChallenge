using System;
using System.Collections.Generic;
using System.Text;
using RAGE;

namespace GrandTheftChallenge_Client.Auth
{
    public class CEF : Events.Script
    {
        private static RAGE.Ui.HtmlWindow login;
        private static RAGE.Ui.HtmlWindow register;
        private static RAGE.Ui.HtmlWindow ban;
        public CEF()
        {
            Events.Add("ShowRegisterWindow", show_register_window);
            Events.Add("DestroyRegisterWindow", destroy_register_window);
            Events.Add("ShowPlayer", show_player);
            Events.Add("DestroyShowPlayer", destroy_show_player);
            Events.Add("ShowMainMenu", show_main_menu);
            Events.Add("DestroyMainMenu", destroy_main_menu);
        }

        private void show_register_window(object[] args)
        {
            login = new RAGE.Ui.HtmlWindow("package://assets/register.html");
            RAGE.Ui.Cursor.Visible = true;
        }

        private void destroy_register_window(object[] args)
        {
            login.Destroy();
            RAGE.Ui.Cursor.Visible = false;
        }


        private void show_player(object[] args)
        {
            register = new RAGE.Ui.HtmlWindow("package://assets/menu.html");
            RAGE.Ui.Cursor.Visible = true;
        }

        private void destroy_show_player(object[] args)
        {
            register.Destroy();
            RAGE.Ui.Cursor.Visible = false;
        }

        private void show_main_menu(object[] args)
        {
            ban = new RAGE.Ui.HtmlWindow("package://assets/ban.html");
            RAGE.Ui.Cursor.Visible = true;
        }
        private void destroy_main_menu(object[] args)
        {
            ban.Destroy();
            RAGE.Ui.Cursor.Visible = false;
        }
    }
}
