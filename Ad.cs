using JahresprojektNeu.Classes;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JahresprojektNeu
{
    [ComVisible(true)]
    public partial class Ad : Form
    {
        private WebBrowser webBrowser1;
        private Button playButton;
        private string videoEmbedUrl = "https://www.youtube.com/embed/Vfm_vYylhdU";
        private string currentUsername;

        public Ad(string username)
        {
            currentUsername = username;
            InitializeComponent();
            SetBrowserVersion();
            InitializeForm();
        }

        private void SetBrowserVersion()
        {
            try
            {
                Microsoft.Win32.Registry.SetValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION",
                    System.AppDomain.CurrentDomain.FriendlyName, 11001, Microsoft.Win32.RegistryValueKind.DWord);
            }
            catch { }
        }

        private void InitializeForm()
        {
            this.Size = new System.Drawing.Size(800, 600);
            this.Text = "YouTube Video Player";
            this.StartPosition = FormStartPosition.CenterScreen;

            playButton = new Button
            {
                Text = "Video abspielen",
                Size = new System.Drawing.Size(200, 50),
                Location = new System.Drawing.Point(300, 10)
            };
            playButton.Click += PlayButton_Click;
            this.Controls.Add(playButton);

            webBrowser1 = new WebBrowser
            {
                Location = new System.Drawing.Point(50, 70),
                Size = new System.Drawing.Size(700, 480),
                ScriptErrorsSuppressed = true
            };
            this.Controls.Add(webBrowser1);
            webBrowser1.ObjectForScripting = new ScriptManager(this);

            ShowStartPage();
        }

        private void ShowStartPage()
        {
            webBrowser1.DocumentText = @"
                <html><body style='text-align:center; font-family:Arial; padding-top:200px;'>
                <h2>Klicken Sie auf 'Video abspielen'</h2><p>Das Video wird hier angezeigt</p></body></html>";
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            playButton.Enabled = false;
            playButton.Text = "Video läuft...";
            LoadYouTubeVideo(videoEmbedUrl);
        }

        private void LoadYouTubeVideo(string embedUrl)
        {
            string html = $@"
        <html>
            <head>
                <meta http-equiv='X-UA-Compatible' content='IE=edge' />
                <script type='text/javascript'>
                    function notifyAfterTimeout() {{
                    setTimeout(function() {{
                    window.external.Notify('videoFinished');
                }}, 23000); // 23 Sekunden
            }}
                </script>
            </head>
            <body onload='notifyAfterTimeout();' style='margin:0; background:black;'>
                <iframe width='100%' height='100%' 
                    src='{embedUrl}?autoplay=1&rel=0' 
                    frameborder='0' allow='autoplay; encrypted-media' allowfullscreen>
                </iframe>
            </body>
        </html>";

            webBrowser1.DocumentText = html;
        }


        public void OnVideoFinished()
        {
            this.Invoke((MethodInvoker)delegate
            {
                decimal currentBalance = SQLManagement.GetUserBalance(currentUsername);
                decimal newBalance = currentBalance + 100;
                SQLManagement.UpdateUserBalance(currentUsername, newBalance);

                MessageBox.Show("+100 € gutgeschrieben!", "Belohnung", MessageBoxButtons.OK, MessageBoxIcon.Information);
                playButton.Enabled = true;
                playButton.Text = "Video abspielen";
                ShowStartPage();
            });
        }
    }

    [ComVisible(true)]
    public class ScriptManager
    {
        private Ad parent;
        public ScriptManager(Ad form) => parent = form;
        public void Notify(string message)
        {
            if (message == "videoFinished")
                parent.OnVideoFinished();
        }
    }
}
