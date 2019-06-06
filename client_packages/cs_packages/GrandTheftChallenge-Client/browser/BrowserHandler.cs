using RAGE;
using RAGE.Ui;
using System.Linq;

namespace GrandTheftChallenge_Client.Browser
{
    public class BrowserHandler : Events.Script
    {
        public static HtmlWindow CreateBrowser(string path, object[] parameters)
        {
            HtmlWindow browser = new HtmlWindow(path);

            if(parameters != null && parameters.Length > 0)
            {
                // Bind the browser creation event
                Events.OnBrowserDomReady += (window) =>
                {
                    if (window != browser) return;

                    // Enable the cursor
                    Cursor.Visible = true;

                    if (parameters.Length > 0)
                    {
                        // Get the function name
                        string function = parameters[0].ToString();
                        object[] arguments = parameters.Skip(1).ToArray();

                        // Call the function passed as parameter
                        ExecuteBrowserFunction(browser, function, arguments);
                    }
                };
            }

            return browser;
        }

        public static void ExecuteBrowserFunction(HtmlWindow browser, string function, object[] parameters)
        {
            // Check for the parameters
            string input = string.Empty;

            foreach (object argument in parameters)
            {
                // Append all the arguments
                input += input.Length > 0 ? (", '" + argument.ToString() + "'") : ("'" + argument.ToString() + "'");
            }

            // Call the function with the parameters
            browser.ExecuteJs(function + "(" + input + ");");
        }

        public static void DestroyBrowser(HtmlWindow browser)
        {
            // Disable the cursor
            Cursor.Visible = false;

            // Destroy the browser
            browser.Destroy();
        }
    }
}
