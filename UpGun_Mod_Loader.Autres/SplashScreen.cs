using System;
using System.Drawing;
using System.Windows.Forms;

namespace UpGun_Mod_Loader.Autres
{
    public class SplashScreen : Form
    {
        private readonly Label _status;
        private readonly PictureBox _logo;
        private readonly Timer _timer;
        private string _baseText = "Opération en cours";
        private int _dotCount = 0;

        public SplashScreen()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(20, 20, 20);
            ForeColor = Color.White;
            TopMost = true;
            Width = 420; Height = 140;
            ShowInTaskbar = false;
            Padding = new Padding(20);

            _logo = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 64,
                Height = 64,
                Left = 24,
                Top = (Height - 64) / 2
            };
            try { _logo.Image = this.Icon?.ToBitmap(); } catch { }

            _status = new Label
            {
                AutoSize = false,
                Left = _logo.Right + 24,
                Top = 36,
                Width = Width - (_logo.Right + 24) - 24,
                Height = 40,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Text = _baseText
            };

            Controls.Add(_logo);
            Controls.Add(_status);

            _timer = new Timer { Interval = 400 };
            _timer.Tick += (s, e) =>
            {
                _dotCount = (_dotCount + 1) % 4;
                _status.Text = _baseText + new string('.', _dotCount);
            };
            _timer.Start();
        }

        public void SetStatus(string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(SetStatus), text);
                return;
            }
            _baseText = text;
            _status.Text = _baseText;
        }

        protected override bool ShowWithoutActivation => true;
    }
}
