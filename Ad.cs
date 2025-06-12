using System;
using System.Windows.Forms;
using System.IO;

namespace JahresprojektNeu
{
    public partial class Ad : Form
    {
        private WebBrowser webBrowser1;
        private Button playButton;
        private string videoPath = @"C:\APR\Schumachers Empfehlung_wirkaufendeinauto.de.mp4";

        public Ad()
        {
            InitializeComponent();
            SetBrowserVersion();
            InitializeForm();
        }

        private void SetBrowserVersion()
        {
            try
            {
                Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION",
                    System.AppDomain.CurrentDomain.FriendlyName, 11001);
            }
            catch { }
        }

        private void InitializeForm()
        {
            this.Size = new System.Drawing.Size(800, 600);
            this.Text = "Video Player";
            this.StartPosition = FormStartPosition.CenterScreen;

            // Button
            playButton = new Button();
            playButton.Text = "Video abspielen";
            playButton.Size = new System.Drawing.Size(200, 50);
            playButton.Location = new System.Drawing.Point(300, 10);
            playButton.Click += PlayButton_Click;
            this.Controls.Add(playButton);

            // WebBrowser
            webBrowser1 = new WebBrowser();
            webBrowser1.Location = new System.Drawing.Point(50, 70);
            webBrowser1.Size = new System.Drawing.Size(700, 480);
            webBrowser1.ScrollBarsEnabled = false;
            webBrowser1.ScriptErrorsSuppressed = true;
            this.Controls.Add(webBrowser1);

            // Zeige Startseite
            ShowStartPage();
        }

        private void ShowStartPage()
        {
            string html = @"
            <html>
            <body style='margin:0; padding:0; background:#f0f0f0; font-family:Arial;'>
                <div style='text-align:center; padding-top:200px;'>
                    <h2>Klicken Sie auf 'Video abspielen'</h2>
                    <p>Das Video wird hier angezeigt</p>
                </div>
            </body>
            </html>";
            webBrowser1.DocumentText = html;
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists(videoPath))
            {
                MessageBox.Show("Video nicht gefunden: " + videoPath);
                return;
            }

            playButton.Enabled = false;
            playButton.Text = "Video läuft...";

            // Kopiere Video in temp-Ordner mit einfacherem Namen
            string tempPath = Path.Combine(Path.GetTempPath(), "video.mp4");
            try
            {
                File.Copy(videoPath, tempPath, true);
                LoadVideo(tempPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler: " + ex.Message);
                playButton.Enabled = true;
                playButton.Text = "Video abspielen";
            }
        }

        private void LoadVideo(string path)
        {
            string fileUrl = new Uri(path).AbsoluteUri;

            string html = $@"
            <html>
            <head>
                <meta http-equiv='X-UA-Compatible' content='IE=edge' />
            </head>
            <body style='margin:0; padding:0; background:black;'>
                <video width='100%' height='100%' autoplay controls onended='videoEnded()' onerror='videoError()'>
                    <source src='{fileUrl}' type='video/mp4'>
                    <p style='color:white; text-align:center; margin-top:200px;'>Video kann nicht geladen werden</p>
                </video>
                <script>
                    function videoEnded() {{
                        window.external.VideoFinished();
                    }}
                    function videoError() {{
                        alert('Video konnte nicht geladen werden');
                        window.external.VideoFinished();
                    }}
                </script>
            </body>
            </html>";

            webBrowser1.DocumentText = html;
            webBrowser1.ObjectForScripting = new ScriptInterface(this);
        }

        public void VideoFinished()
        {
            playButton.Enabled = true;
            playButton.Text = "Video abspielen";
            ShowStartPage();
        }
    }

    [System.Runtime.InteropServices.ComVisible(true)]
    public class ScriptInterface
    {
        private Ad form;

        public ScriptInterface(Ad form)
        {
            this.form = form;
        }

        public void VideoFinished()
        {
            form.Invoke(new Action(() => form.VideoFinished()));
        }
    }
}