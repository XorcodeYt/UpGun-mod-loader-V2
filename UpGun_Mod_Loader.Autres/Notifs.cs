using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UpGun_Mod_Loader.Autres;

public class Notifs : Form
{
	private IContainer components;

	private Label Message;

	private Timer TClose;

	public Notifs(string message, int time)
	{
		InitializeComponent();
		base.ShowInTaskbar = false;
		int length = message.Length;
		int num = 100 + length * 10;
		int num2 = 40;
		base.Size = new Size(num, num2);
		base.StartPosition = FormStartPosition.Manual;
		base.Location = new Point(10, 10);
		base.TopMost = true;
		Focus();
		BringToFront();
		Message.Text = message;
		TClose.Tick += TClose_Tick;
		int interval = time * 1000;
		TClose.Interval = interval;
		TClose.Start();
	}

	private void TClose_Tick(object sender, EventArgs e)
	{
		TClose.Stop();
		Close();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpGun_Mod_Loader.Autres.Notifs));
		this.Message = new System.Windows.Forms.Label();
		this.TClose = new System.Windows.Forms.Timer(this.components);
		base.SuspendLayout();
		this.Message.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
		this.Message.Dock = System.Windows.Forms.DockStyle.Fill;
		this.Message.Font = new System.Drawing.Font("Palatino Linotype", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Message.ForeColor = System.Drawing.Color.White;
		this.Message.Location = new System.Drawing.Point(0, 0);
		this.Message.Name = "Message";
		this.Message.Size = new System.Drawing.Size(100, 40);
		this.Message.TabIndex = 0;
		this.Message.Text = "Temp";
		this.Message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
		base.ClientSize = new System.Drawing.Size(100, 40);
		base.Controls.Add(this.Message);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "Notifs";
		this.Text = "Notification";
		base.ResumeLayout(false);
	}
}
