using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using CustomControls.RJControls;
using UpGunModLoader3._0.ContentForms;

namespace UpGunModLoader3._0;

public class UpGun_Mod_Loader : Form
{
	private static Mutex mutex = new Mutex(initiallyOwned: true, "{E3D2A813-62E2-4DDF-9E91-38C20EC890D6}");

	private int currentPage;

	private const int itemsPerPage = 10;

	private IContainer components;

	private Panel pnlTop;

	private Panel pnlCredits;

	private Panel pnlc1;

	private Panel pnlc2;

	private FlowLayoutPanel flpnlMods;

	private Panel pnlc4;

	private Panel pnlc3;

	private Panel pnlBR;

	private Panel pnlBL;

	private Panel pnlc5;

	private Panel pnlc6;

	private Panel pnlTR;

	private Panel pnlTL;

	private PictureBox pbNextPage;

	private PictureBox pbPreviousPage;

	private Panel pnlcale;

	private PictureBox pbUpGunLogo;

	private Label lblPageNum;

	private PictureBox pbUploadMod;

	private Panel pnlc8;

	private Label lblCopyright;

	private Panel pnlc9;

	private Panel pnlc10;

	private PictureBox pbDiscordInvite;

	private Panel pnlc11;

	private PictureBox pbYoutubeChannel;

	private Panel pnlc12;

	private Label lblDiscordFeedback;

	private Panel pnlc13;

	private Label lblMadeBy;

	private Button btnSearch;

	private ComboBox cbSearchModType;

	private TextBox tbModSearch;

	private Panel pnlc15;

	private Label lblModNameResearch;

	private Panel pnlc14;

	private Label lblModTypeResearch;

	private Panel pnlc17;

	private Panel pnlc18;

	private Panel pnlc16;

	private Panel pnlc20;

	private Panel pnlc19;

	private Panel pnlSearch2;

	private Panel panel2;

	private Panel pnlc21;

	public UpGun_Mod_Loader()
	{
		if (mutex.WaitOne(TimeSpan.Zero, exitContext: true))
		{
			InitializeComponent();
			Control.CheckForIllegalCrossThreadCalls = false;
			Fonctions.CheckIfModSupportInstalled();
			//Process.Start(Fonctions.appdatapath2 + "\\AutoUpdate.exe");
			if (File.Exists(Fonctions.PakModsSupportFilePath))
			{
				Fonctions.MajModLoaderUpGun();
			}
			GetModsFileIds();
			mutex.ReleaseMutex();
		}
		else
		{
			MessageBox.Show("The mod loader is already opened!");
			Close();
		}
	}

	private async void GetModsFileIds()
	{
		GetAllMods(await Fonctions.GetUploadedMods(), tbModSearch.Text, cbSearchModType.SelectedItem?.ToString());
	}

	private void GetAllMods(string allMods, string searchName, string searchType)
	{
		string[] array = allMods.Split(new string[2] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		List<string[]> list = new List<string[]>();
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string[] array3 = array2[i].Split(',');
			string text = array3[1];
			string text2 = array3[6];
			if ((string.IsNullOrEmpty(searchName) || text.IndexOf(searchName, StringComparison.OrdinalIgnoreCase) != -1) && (string.IsNullOrEmpty(searchType) || text2 == searchType))
			{
				list.Add(array3);
			}
		}
		int num = currentPage * 10;
		int num2 = Math.Min((currentPage + 1) * 10, list.Count);
		for (int j = num; j < num2; j++)
		{
			string[] values = list[j];
			Invoke((MethodInvoker)delegate
			{
				LoadMod(values[0], values[1], values[2], values[3], values[4], values[5], values[6]);
			});
		}
	}

	private void LoadMod(string OwnerName, string ModName, string Date, string ImageUrl, string ModDLUrl, string ZipFileName, string ModType)
	{
		string NewZipFileName = ZipFileName.Replace(".zip", "");
		CheckState checkState = CheckState.Unchecked;
		if (File.Exists(Fonctions.GetUpGunPath() + "\\" + NewZipFileName + ".pak"))
		{
			checkState = CheckState.Checked;
		}
		Panel panel = new Panel
		{
			Name = "pnlMod" + OwnerName,
			BackColor = Color.FromArgb(10, 10, 10),
			Size = new Size(340, 165)
		};
		Panel panel2 = new Panel
		{
			Name = "pnlMod" + OwnerName,
			BackColor = Color.FromArgb(10, 10, 10),
			Dock = DockStyle.Left,
			Size = new Size(143, 165)
		};
		Panel panel3 = new Panel
		{
			Name = "pnlMod" + OwnerName,
			BackColor = Color.FromArgb(10, 10, 10),
			Dock = DockStyle.Bottom,
			Size = new Size(143, 52)
		};
		Panel panel4 = new Panel
		{
			Name = "pnlColor2" + OwnerName,
			BackColor = Color.Purple,
			Dock = DockStyle.Left,
			Size = new Size(1, 165)
		};
		Panel panel5 = new Panel
		{
			Name = "pnlColor2" + OwnerName,
			BackColor = Color.Purple,
			Dock = DockStyle.Top,
			Size = new Size(143, 1)
		};
		Panel panel6 = new Panel
		{
			Name = "pnlCale" + OwnerName,
			BackColor = Color.FromArgb(10, 10, 10),
			Dock = DockStyle.Left,
			Size = new Size(32, 32)
		};
		Panel panel7 = new Panel
		{
			Name = "pnlCale" + OwnerName,
			BackColor = Color.FromArgb(10, 10, 10),
			Dock = DockStyle.Bottom,
			Size = new Size(22, 22)
		};
		SwitchButton SBDLSwitch = new SwitchButton
		{
			Name = "SBDLSwitch" + OwnerName,
			Dock = DockStyle.Left,
			AutoSize = false,
			Cursor = Cursors.Hand,
			CheckState = checkState,
			Size = new Size(77, 35)
		};
		PictureBox value = new PictureBox
		{
			Name = "pb" + OwnerName,
			ImageLocation = ImageUrl,
			ErrorImage = null,
			InitialImage = null,
			SizeMode = PictureBoxSizeMode.StretchImage,
			Dock = DockStyle.Top,
			Size = new Size(143, 91)
		};
		Label label = new Label
		{
			Name = "lbl" + OwnerName,
			Text = "Name: " + ModName,
			ForeColor = Color.White,
			AutoSize = false,
			Size = new Size(197, 21),
			Font = new Font("Segoe UI", 12f, FontStyle.Bold),
			BackColor = Color.FromArgb(10, 10, 10),
			Dock = DockStyle.Top
		};
		Label label2 = new Label
		{
			Name = "lbl" + OwnerName,
			Text = "Date: " + Date,
			ForeColor = Color.White,
			AutoSize = false,
			Size = new Size(197, 21),
			Font = new Font("Segoe UI", 12f, FontStyle.Bold),
			BackColor = Color.FromArgb(10, 10, 10),
			Dock = DockStyle.Top
		};
		Label label3 = new Label
		{
			Name = "lbl" + OwnerName,
			Text = "Creator: " + OwnerName,
			ForeColor = Color.White,
			AutoSize = false,
			Size = new Size(197, 21),
			Font = new Font("Segoe UI", 12f, FontStyle.Bold),
			BackColor = Color.FromArgb(10, 10, 10),
			Dock = DockStyle.Top
		};
		Label label4 = new Label
		{
			Name = "lbl" + ModType,
			Text = "Type: " + ModType,
			ForeColor = Color.White,
			AutoSize = false,
			Size = new Size(197, 21),
			Font = new Font("Segoe UI", 12f, FontStyle.Bold),
			BackColor = Color.FromArgb(10, 10, 10),
			Dock = DockStyle.Bottom
		};
		SBDLSwitch.CheckedChanged += delegate
		{
			if (SBDLSwitch.Checked)
			{
				if (Fonctions.CheckIfModSupportInstalled())
				{
					if (Process.GetProcessesByName("UpGun-Win64-Shipping").Length != 0)
					{
						Fonctions.ExecuteCmdCommand("taskkill /f /im UpGun-Win64-Shipping.exe");
						MessageBox.Show("UpGun.exe closed!");
					}
					Thread.Sleep(300);
					Fonctions.InstallMod(ModDLUrl, NewZipFileName);
				}
			}
			else if (Fonctions.CheckIfModSupportInstalled())
			{
				if (Process.GetProcessesByName("UpGun-Win64-Shipping").Length != 0)
				{
					Fonctions.ExecuteCmdCommand("taskkill /f /im UpGun-Win64-Shipping.exe");
					MessageBox.Show("UpGun.exe closed!");
				}
				Thread.Sleep(300);
				Fonctions.DeleteMod(NewZipFileName);
			}
		};
		flpnlMods.Controls.Add(panel);
		panel.BringToFront();
		panel.Controls.Add(panel2);
		panel.Controls.Add(panel4);
		panel4.BringToFront();
		panel2.Controls.Add(value);
		panel.Controls.Add(label);
		label.BringToFront();
		panel.Controls.Add(label2);
		label2.BringToFront();
		panel.Controls.Add(label3);
		label3.BringToFront();
		panel.Controls.Add(label4);
		label4.BringToFront();
		panel2.Controls.Add(panel5);
		panel5.BringToFront();
		panel2.Controls.Add(panel3);
		panel3.Controls.Add(panel6);
		panel6.BringToFront();
		panel3.Controls.Add(panel7);
		panel7.BringToFront();
		panel3.Controls.Add(SBDLSwitch);
		SBDLSwitch.BringToFront();
	}

	private void pbPreviousPage_Click(object sender, EventArgs e)
	{
		if (currentPage != 0)
		{
			currentPage--;
			foreach (Control control in flpnlMods.Controls)
			{
				control.Dispose();
			}
			flpnlMods.Controls.Clear();
			ResetPageNum();
			GetModsFileIds();
		}
		else
		{
			MessageBox.Show("There is no more pages here!");
		}
	}

	private void pbNextPage_Click(object sender, EventArgs e)
	{
		int num = 0;
		foreach (Control control in flpnlMods.Controls)
		{
			_ = control;
			num++;
		}
		if (num < 10)
		{
			MessageBox.Show("There is no more pages here!");
			return;
		}
		currentPage++;
		foreach (Control control2 in flpnlMods.Controls)
		{
			control2.Dispose();
		}
		flpnlMods.Controls.Clear();
		ResetPageNum();
		GetModsFileIds();
	}

	private void ResetPageNum()
	{
		int num = currentPage + 1;
		lblPageNum.Text = num.ToString();
	}

	private void pbSettings_Click(object sender, EventArgs e)
	{
		Process.Start(Fonctions.path1);
		MessageBox.Show("After editing the path in the text file, please restart the loader.");
	}

	private void pbUploadMod_Click(object sender, EventArgs e)
	{
		new Upload_Mod().Show();
	}

	private void pbYoutubeChannel_Click(object sender, EventArgs e)
	{
		Process.Start("https://www.youtube.com/channel/UCE2NKV2Rs-ag9M79hudMvog");
	}

	private void pbDiscordInvite_Click(object sender, EventArgs e)
	{
		Process.Start("https://discord.gg/B9tbZ9qv9e");
	}

	private async void btnSearch_Click(object sender, EventArgs e)
	{
		btnSearch.Enabled = false;
		foreach (Control control in flpnlMods.Controls)
		{
			control.Dispose();
		}
		flpnlMods.Controls.Clear();
		currentPage = 0;
		ResetPageNum();
		GetAllMods(await Fonctions.GetUploadedMods(), tbModSearch.Text, cbSearchModType.SelectedItem?.ToString());
		btnSearch.Enabled = true;
	}

	private void pbUploadMod_MouseEnter(object sender, EventArgs e)
	{
		pbUploadMod.BackColor = Color.FromArgb(50, 50, 50);
	}

	private void pbUploadMod_MouseLeave(object sender, EventArgs e)
	{
		pbUploadMod.BackColor = Color.FromArgb(20, 20, 20);
	}

	private void pbDiscordInvite_MouseEnter(object sender, EventArgs e)
	{
		pbDiscordInvite.BackColor = Color.FromArgb(50, 50, 50);
	}

	private void pbDiscordInvite_MouseLeave(object sender, EventArgs e)
	{
		pbDiscordInvite.BackColor = Color.FromArgb(20, 20, 20);
	}

	private void pbYoutubeChannel_MouseEnter(object sender, EventArgs e)
	{
		pbYoutubeChannel.BackColor = Color.FromArgb(50, 50, 50);
	}

	private void pbYoutubeChannel_MouseLeave(object sender, EventArgs e)
	{
		pbYoutubeChannel.BackColor = Color.FromArgb(20, 20, 20);
	}

	private void pbPreviousPage_MouseEnter(object sender, EventArgs e)
	{
		pbPreviousPage.BackColor = Color.FromArgb(50, 50, 50);
	}

	private void pbPreviousPage_MouseLeave(object sender, EventArgs e)
	{
		pbPreviousPage.BackColor = Color.FromArgb(20, 20, 20);
	}

	private void pbNextPage_MouseEnter(object sender, EventArgs e)
	{
		pbNextPage.BackColor = Color.FromArgb(50, 50, 50);
	}

	private void pbNextPage_MouseLeave(object sender, EventArgs e)
	{
		pbNextPage.BackColor = Color.FromArgb(20, 20, 20);
	}

	private void btnSearch_MouseEnter(object sender, EventArgs e)
	{
		btnSearch.BackColor = Color.FromArgb(30, 30, 30);
	}

	private void btnSearch_MouseLeave(object sender, EventArgs e)
	{
		btnSearch.BackColor = Color.Black;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpGunModLoader3._0.UpGun_Mod_Loader));
		this.pnlTop = new System.Windows.Forms.Panel();
		this.pbUpGunLogo = new System.Windows.Forms.PictureBox();
		this.pnlc5 = new System.Windows.Forms.Panel();
		this.pnlc6 = new System.Windows.Forms.Panel();
		this.pnlTR = new System.Windows.Forms.Panel();
		this.pnlc8 = new System.Windows.Forms.Panel();
		this.pbUploadMod = new System.Windows.Forms.PictureBox();
		this.pnlTL = new System.Windows.Forms.Panel();
		this.pnlc16 = new System.Windows.Forms.Panel();
		this.lblModNameResearch = new System.Windows.Forms.Label();
		this.pnlc14 = new System.Windows.Forms.Panel();
		this.tbModSearch = new System.Windows.Forms.TextBox();
		this.pnlSearch2 = new System.Windows.Forms.Panel();
		this.pnlc15 = new System.Windows.Forms.Panel();
		this.lblModTypeResearch = new System.Windows.Forms.Label();
		this.pnlc17 = new System.Windows.Forms.Panel();
		this.cbSearchModType = new System.Windows.Forms.ComboBox();
		this.pnlc21 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.btnSearch = new System.Windows.Forms.Button();
		this.pnlc18 = new System.Windows.Forms.Panel();
		this.pnlCredits = new System.Windows.Forms.Panel();
		this.lblPageNum = new System.Windows.Forms.Label();
		this.pnlc20 = new System.Windows.Forms.Panel();
		this.pnlc19 = new System.Windows.Forms.Panel();
		this.pbNextPage = new System.Windows.Forms.PictureBox();
		this.pbPreviousPage = new System.Windows.Forms.PictureBox();
		this.pnlcale = new System.Windows.Forms.Panel();
		this.lblCopyright = new System.Windows.Forms.Label();
		this.pnlc9 = new System.Windows.Forms.Panel();
		this.pnlc4 = new System.Windows.Forms.Panel();
		this.pnlc3 = new System.Windows.Forms.Panel();
		this.pnlBR = new System.Windows.Forms.Panel();
		this.pnlc12 = new System.Windows.Forms.Panel();
		this.lblDiscordFeedback = new System.Windows.Forms.Label();
		this.pnlc10 = new System.Windows.Forms.Panel();
		this.pbDiscordInvite = new System.Windows.Forms.PictureBox();
		this.pnlBL = new System.Windows.Forms.Panel();
		this.pnlc13 = new System.Windows.Forms.Panel();
		this.lblMadeBy = new System.Windows.Forms.Label();
		this.pnlc11 = new System.Windows.Forms.Panel();
		this.pbYoutubeChannel = new System.Windows.Forms.PictureBox();
		this.pnlc1 = new System.Windows.Forms.Panel();
		this.pnlc2 = new System.Windows.Forms.Panel();
		this.flpnlMods = new System.Windows.Forms.FlowLayoutPanel();
		this.pnlTop.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pbUpGunLogo).BeginInit();
		this.pnlTR.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pbUploadMod).BeginInit();
		this.pnlTL.SuspendLayout();
		this.pnlSearch2.SuspendLayout();
		this.panel2.SuspendLayout();
		this.pnlCredits.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pbNextPage).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pbPreviousPage).BeginInit();
		this.pnlcale.SuspendLayout();
		this.pnlBR.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pbDiscordInvite).BeginInit();
		this.pnlBL.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pbYoutubeChannel).BeginInit();
		base.SuspendLayout();
		this.pnlTop.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
		this.pnlTop.Controls.Add(this.pbUpGunLogo);
		this.pnlTop.Controls.Add(this.pnlc5);
		this.pnlTop.Controls.Add(this.pnlc6);
		this.pnlTop.Controls.Add(this.pnlTR);
		this.pnlTop.Controls.Add(this.pnlTL);
		this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
		this.pnlTop.Location = new System.Drawing.Point(0, 0);
		this.pnlTop.Name = "pnlTop";
		this.pnlTop.Size = new System.Drawing.Size(710, 57);
		this.pnlTop.TabIndex = 0;
		this.pbUpGunLogo.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pbUpGunLogo.ErrorImage = null;
		this.pbUpGunLogo.ImageLocation = "https://dl.dropboxusercontent.com/scl/fi/1h4jelwt41e5h5e71ol0h/UpGunLogo.png?rlkey=y62ynbqscsj3sf8qkpr8ns7io&raw=1";
		this.pbUpGunLogo.InitialImage = null;
		this.pbUpGunLogo.Location = new System.Drawing.Point(384, 0);
		this.pbUpGunLogo.Name = "pbUpGunLogo";
		this.pbUpGunLogo.Size = new System.Drawing.Size(111, 57);
		this.pbUpGunLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pbUpGunLogo.TabIndex = 8;
		this.pbUpGunLogo.TabStop = false;
		this.pnlc5.BackColor = System.Drawing.Color.Purple;
		this.pnlc5.Dock = System.Windows.Forms.DockStyle.Left;
		this.pnlc5.Location = new System.Drawing.Point(383, 0);
		this.pnlc5.Name = "pnlc5";
		this.pnlc5.Size = new System.Drawing.Size(1, 57);
		this.pnlc5.TabIndex = 7;
		this.pnlc6.BackColor = System.Drawing.Color.Purple;
		this.pnlc6.Dock = System.Windows.Forms.DockStyle.Right;
		this.pnlc6.Location = new System.Drawing.Point(495, 0);
		this.pnlc6.Name = "pnlc6";
		this.pnlc6.Size = new System.Drawing.Size(1, 57);
		this.pnlc6.TabIndex = 6;
		this.pnlTR.Controls.Add(this.pnlc8);
		this.pnlTR.Controls.Add(this.pbUploadMod);
		this.pnlTR.Dock = System.Windows.Forms.DockStyle.Right;
		this.pnlTR.Location = new System.Drawing.Point(496, 0);
		this.pnlTR.Name = "pnlTR";
		this.pnlTR.Size = new System.Drawing.Size(214, 57);
		this.pnlTR.TabIndex = 5;
		this.pnlc8.BackColor = System.Drawing.Color.Purple;
		this.pnlc8.Dock = System.Windows.Forms.DockStyle.Right;
		this.pnlc8.Location = new System.Drawing.Point(156, 0);
		this.pnlc8.Name = "pnlc8";
		this.pnlc8.Size = new System.Drawing.Size(1, 57);
		this.pnlc8.TabIndex = 2;
		this.pbUploadMod.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pbUploadMod.Dock = System.Windows.Forms.DockStyle.Right;
		this.pbUploadMod.ErrorImage = null;
		this.pbUploadMod.ImageLocation = "https://dl.dropboxusercontent.com/scl/fi/3sb65dniokq1bsw742ahe/UploadLogo.png?rlkey=ss4gyhbsxt8u92wqqkt18dse5&raw=1";
		this.pbUploadMod.InitialImage = null;
		this.pbUploadMod.Location = new System.Drawing.Point(157, 0);
		this.pbUploadMod.Name = "pbUploadMod";
		this.pbUploadMod.Size = new System.Drawing.Size(57, 57);
		this.pbUploadMod.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pbUploadMod.TabIndex = 0;
		this.pbUploadMod.TabStop = false;
		this.pbUploadMod.Click += new System.EventHandler(pbUploadMod_Click);
		this.pbUploadMod.MouseEnter += new System.EventHandler(pbUploadMod_MouseEnter);
		this.pbUploadMod.MouseLeave += new System.EventHandler(pbUploadMod_MouseLeave);
		this.pnlTL.Controls.Add(this.pnlc16);
		this.pnlTL.Controls.Add(this.lblModNameResearch);
		this.pnlTL.Controls.Add(this.pnlc14);
		this.pnlTL.Controls.Add(this.tbModSearch);
		this.pnlTL.Controls.Add(this.pnlSearch2);
		this.pnlTL.Controls.Add(this.panel2);
		this.pnlTL.Dock = System.Windows.Forms.DockStyle.Left;
		this.pnlTL.Location = new System.Drawing.Point(0, 0);
		this.pnlTL.Name = "pnlTL";
		this.pnlTL.Size = new System.Drawing.Size(383, 57);
		this.pnlTL.TabIndex = 4;
		this.pnlc16.BackColor = System.Drawing.Color.Purple;
		this.pnlc16.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.pnlc16.Location = new System.Drawing.Point(155, 19);
		this.pnlc16.Name = "pnlc16";
		this.pnlc16.Size = new System.Drawing.Size(160, 1);
		this.pnlc16.TabIndex = 8;
		this.lblModNameResearch.BackColor = System.Drawing.Color.FromArgb(5, 5, 5);
		this.lblModNameResearch.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.lblModNameResearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblModNameResearch.ForeColor = System.Drawing.Color.White;
		this.lblModNameResearch.Location = new System.Drawing.Point(155, 20);
		this.lblModNameResearch.Name = "lblModNameResearch";
		this.lblModNameResearch.Size = new System.Drawing.Size(160, 16);
		this.lblModNameResearch.TabIndex = 7;
		this.lblModNameResearch.Text = "↓↓↓Mod name research↓↓↓";
		this.lblModNameResearch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.pnlc14.BackColor = System.Drawing.Color.Purple;
		this.pnlc14.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.pnlc14.Location = new System.Drawing.Point(155, 36);
		this.pnlc14.Name = "pnlc14";
		this.pnlc14.Size = new System.Drawing.Size(160, 1);
		this.pnlc14.TabIndex = 6;
		this.tbModSearch.BackColor = System.Drawing.Color.Black;
		this.tbModSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.tbModSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tbModSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tbModSearch.ForeColor = System.Drawing.Color.White;
		this.tbModSearch.Location = new System.Drawing.Point(155, 37);
		this.tbModSearch.Name = "tbModSearch";
		this.tbModSearch.Size = new System.Drawing.Size(160, 20);
		this.tbModSearch.TabIndex = 5;
		this.pnlSearch2.Controls.Add(this.pnlc15);
		this.pnlSearch2.Controls.Add(this.lblModTypeResearch);
		this.pnlSearch2.Controls.Add(this.pnlc17);
		this.pnlSearch2.Controls.Add(this.cbSearchModType);
		this.pnlSearch2.Controls.Add(this.pnlc21);
		this.pnlSearch2.Dock = System.Windows.Forms.DockStyle.Left;
		this.pnlSearch2.Location = new System.Drawing.Point(0, 0);
		this.pnlSearch2.Name = "pnlSearch2";
		this.pnlSearch2.Size = new System.Drawing.Size(155, 57);
		this.pnlSearch2.TabIndex = 9;
		this.pnlc15.BackColor = System.Drawing.Color.Purple;
		this.pnlc15.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.pnlc15.Location = new System.Drawing.Point(0, 18);
		this.pnlc15.Name = "pnlc15";
		this.pnlc15.Size = new System.Drawing.Size(154, 1);
		this.pnlc15.TabIndex = 7;
		this.lblModTypeResearch.BackColor = System.Drawing.Color.FromArgb(5, 5, 5);
		this.lblModTypeResearch.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.lblModTypeResearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblModTypeResearch.ForeColor = System.Drawing.Color.White;
		this.lblModTypeResearch.Location = new System.Drawing.Point(0, 19);
		this.lblModTypeResearch.Name = "lblModTypeResearch";
		this.lblModTypeResearch.Size = new System.Drawing.Size(154, 16);
		this.lblModTypeResearch.TabIndex = 8;
		this.lblModTypeResearch.Text = "↓↓↓Mod type research↓↓↓";
		this.lblModTypeResearch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.pnlc17.BackColor = System.Drawing.Color.Purple;
		this.pnlc17.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.pnlc17.Location = new System.Drawing.Point(0, 35);
		this.pnlc17.Name = "pnlc17";
		this.pnlc17.Size = new System.Drawing.Size(154, 1);
		this.pnlc17.TabIndex = 10;
		this.cbSearchModType.BackColor = System.Drawing.Color.Black;
		this.cbSearchModType.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.cbSearchModType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbSearchModType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.cbSearchModType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.cbSearchModType.ForeColor = System.Drawing.Color.White;
		this.cbSearchModType.FormattingEnabled = true;
		this.cbSearchModType.Items.AddRange(new object[10] { "", "Map", "Gun Skin", "Cosmetic", "Player Model", "Settings", "Lobby Settings", "HUD", "Font", "Event" });
		this.cbSearchModType.Location = new System.Drawing.Point(0, 36);
		this.cbSearchModType.Name = "cbSearchModType";
		this.cbSearchModType.Size = new System.Drawing.Size(154, 21);
		this.cbSearchModType.TabIndex = 3;
		this.pnlc21.BackColor = System.Drawing.Color.Purple;
		this.pnlc21.Dock = System.Windows.Forms.DockStyle.Right;
		this.pnlc21.Location = new System.Drawing.Point(154, 0);
		this.pnlc21.Name = "pnlc21";
		this.pnlc21.Size = new System.Drawing.Size(1, 57);
		this.pnlc21.TabIndex = 11;
		this.panel2.Controls.Add(this.btnSearch);
		this.panel2.Controls.Add(this.pnlc18);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
		this.panel2.Location = new System.Drawing.Point(315, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(68, 57);
		this.panel2.TabIndex = 10;
		this.btnSearch.BackColor = System.Drawing.Color.Black;
		this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSearch.ForeColor = System.Drawing.Color.White;
		this.btnSearch.Location = new System.Drawing.Point(1, 36);
		this.btnSearch.Name = "btnSearch";
		this.btnSearch.Size = new System.Drawing.Size(67, 21);
		this.btnSearch.TabIndex = 4;
		this.btnSearch.Text = "Search";
		this.btnSearch.UseVisualStyleBackColor = false;
		this.btnSearch.Click += new System.EventHandler(btnSearch_Click);
		this.btnSearch.MouseEnter += new System.EventHandler(btnSearch_MouseEnter);
		this.btnSearch.MouseLeave += new System.EventHandler(btnSearch_MouseLeave);
		this.pnlc18.BackColor = System.Drawing.Color.Purple;
		this.pnlc18.Dock = System.Windows.Forms.DockStyle.Left;
		this.pnlc18.Location = new System.Drawing.Point(0, 0);
		this.pnlc18.Name = "pnlc18";
		this.pnlc18.Size = new System.Drawing.Size(1, 57);
		this.pnlc18.TabIndex = 7;
		this.pnlCredits.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
		this.pnlCredits.Controls.Add(this.lblPageNum);
		this.pnlCredits.Controls.Add(this.pnlc20);
		this.pnlCredits.Controls.Add(this.pnlc19);
		this.pnlCredits.Controls.Add(this.pbNextPage);
		this.pnlCredits.Controls.Add(this.pbPreviousPage);
		this.pnlCredits.Controls.Add(this.pnlcale);
		this.pnlCredits.Controls.Add(this.pnlc4);
		this.pnlCredits.Controls.Add(this.pnlc3);
		this.pnlCredits.Controls.Add(this.pnlBR);
		this.pnlCredits.Controls.Add(this.pnlBL);
		this.pnlCredits.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.pnlCredits.Location = new System.Drawing.Point(0, 402);
		this.pnlCredits.Name = "pnlCredits";
		this.pnlCredits.Size = new System.Drawing.Size(710, 55);
		this.pnlCredits.TabIndex = 1;
		this.lblPageNum.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPageNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPageNum.ForeColor = System.Drawing.Color.White;
		this.lblPageNum.Location = new System.Drawing.Point(326, 0);
		this.lblPageNum.Name = "lblPageNum";
		this.lblPageNum.Size = new System.Drawing.Size(58, 35);
		this.lblPageNum.TabIndex = 7;
		this.lblPageNum.Text = "1";
		this.lblPageNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.pnlc20.BackColor = System.Drawing.Color.Purple;
		this.pnlc20.Dock = System.Windows.Forms.DockStyle.Right;
		this.pnlc20.Location = new System.Drawing.Point(384, 0);
		this.pnlc20.Name = "pnlc20";
		this.pnlc20.Size = new System.Drawing.Size(1, 35);
		this.pnlc20.TabIndex = 9;
		this.pnlc19.BackColor = System.Drawing.Color.Purple;
		this.pnlc19.Dock = System.Windows.Forms.DockStyle.Left;
		this.pnlc19.Location = new System.Drawing.Point(325, 0);
		this.pnlc19.Name = "pnlc19";
		this.pnlc19.Size = new System.Drawing.Size(1, 35);
		this.pnlc19.TabIndex = 8;
		this.pbNextPage.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pbNextPage.Dock = System.Windows.Forms.DockStyle.Right;
		this.pbNextPage.ErrorImage = null;
		this.pbNextPage.ImageLocation = "https://www.iconsdb.com/icons/preview/white/arrow-32-xxl.png";
		this.pbNextPage.InitialImage = null;
		this.pbNextPage.Location = new System.Drawing.Point(385, 0);
		this.pbNextPage.Name = "pbNextPage";
		this.pbNextPage.Size = new System.Drawing.Size(35, 35);
		this.pbNextPage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pbNextPage.TabIndex = 6;
		this.pbNextPage.TabStop = false;
		this.pbNextPage.Click += new System.EventHandler(pbNextPage_Click);
		this.pbNextPage.MouseEnter += new System.EventHandler(pbNextPage_MouseEnter);
		this.pbNextPage.MouseLeave += new System.EventHandler(pbNextPage_MouseLeave);
		this.pbPreviousPage.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pbPreviousPage.Dock = System.Windows.Forms.DockStyle.Left;
		this.pbPreviousPage.ErrorImage = null;
		this.pbPreviousPage.ImageLocation = "https://www.iconsdb.com/icons/preview/white/arrow-97-xxl.png";
		this.pbPreviousPage.InitialImage = null;
		this.pbPreviousPage.Location = new System.Drawing.Point(290, 0);
		this.pbPreviousPage.Name = "pbPreviousPage";
		this.pbPreviousPage.Size = new System.Drawing.Size(35, 35);
		this.pbPreviousPage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pbPreviousPage.TabIndex = 5;
		this.pbPreviousPage.TabStop = false;
		this.pbPreviousPage.Click += new System.EventHandler(pbPreviousPage_Click);
		this.pbPreviousPage.MouseEnter += new System.EventHandler(pbPreviousPage_MouseEnter);
		this.pbPreviousPage.MouseLeave += new System.EventHandler(pbPreviousPage_MouseLeave);
		this.pnlcale.Controls.Add(this.lblCopyright);
		this.pnlcale.Controls.Add(this.pnlc9);
		this.pnlcale.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.pnlcale.Location = new System.Drawing.Point(290, 35);
		this.pnlcale.Name = "pnlcale";
		this.pnlcale.Size = new System.Drawing.Size(130, 20);
		this.pnlcale.TabIndex = 4;
		this.lblCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblCopyright.ForeColor = System.Drawing.Color.White;
		this.lblCopyright.Location = new System.Drawing.Point(0, 1);
		this.lblCopyright.Name = "lblCopyright";
		this.lblCopyright.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.lblCopyright.Size = new System.Drawing.Size(130, 19);
		this.lblCopyright.TabIndex = 0;
		this.lblCopyright.Text = "Copyright © 2023";
		this.lblCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.pnlc9.BackColor = System.Drawing.Color.Purple;
		this.pnlc9.Dock = System.Windows.Forms.DockStyle.Top;
		this.pnlc9.Location = new System.Drawing.Point(0, 0);
		this.pnlc9.Name = "pnlc9";
		this.pnlc9.Size = new System.Drawing.Size(130, 1);
		this.pnlc9.TabIndex = 0;
		this.pnlc4.BackColor = System.Drawing.Color.Purple;
		this.pnlc4.Dock = System.Windows.Forms.DockStyle.Left;
		this.pnlc4.Location = new System.Drawing.Point(289, 0);
		this.pnlc4.Name = "pnlc4";
		this.pnlc4.Size = new System.Drawing.Size(1, 55);
		this.pnlc4.TabIndex = 3;
		this.pnlc3.BackColor = System.Drawing.Color.Purple;
		this.pnlc3.Dock = System.Windows.Forms.DockStyle.Right;
		this.pnlc3.Location = new System.Drawing.Point(420, 0);
		this.pnlc3.Name = "pnlc3";
		this.pnlc3.Size = new System.Drawing.Size(1, 55);
		this.pnlc3.TabIndex = 2;
		this.pnlBR.Controls.Add(this.pnlc12);
		this.pnlBR.Controls.Add(this.lblDiscordFeedback);
		this.pnlBR.Controls.Add(this.pnlc10);
		this.pnlBR.Controls.Add(this.pbDiscordInvite);
		this.pnlBR.Dock = System.Windows.Forms.DockStyle.Right;
		this.pnlBR.Location = new System.Drawing.Point(421, 0);
		this.pnlBR.Name = "pnlBR";
		this.pnlBR.Size = new System.Drawing.Size(289, 55);
		this.pnlBR.TabIndex = 1;
		this.pnlc12.BackColor = System.Drawing.Color.Purple;
		this.pnlc12.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.pnlc12.Location = new System.Drawing.Point(0, 35);
		this.pnlc12.Name = "pnlc12";
		this.pnlc12.Size = new System.Drawing.Size(231, 1);
		this.pnlc12.TabIndex = 6;
		this.lblDiscordFeedback.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.lblDiscordFeedback.ForeColor = System.Drawing.Color.White;
		this.lblDiscordFeedback.Location = new System.Drawing.Point(0, 36);
		this.lblDiscordFeedback.Name = "lblDiscordFeedback";
		this.lblDiscordFeedback.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.lblDiscordFeedback.Size = new System.Drawing.Size(231, 19);
		this.lblDiscordFeedback.TabIndex = 5;
		this.lblDiscordFeedback.Text = "Send feedback on my discord :)";
		this.lblDiscordFeedback.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.pnlc10.BackColor = System.Drawing.Color.Purple;
		this.pnlc10.Dock = System.Windows.Forms.DockStyle.Right;
		this.pnlc10.Location = new System.Drawing.Point(231, 0);
		this.pnlc10.Name = "pnlc10";
		this.pnlc10.Size = new System.Drawing.Size(1, 55);
		this.pnlc10.TabIndex = 4;
		this.pbDiscordInvite.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pbDiscordInvite.Dock = System.Windows.Forms.DockStyle.Right;
		this.pbDiscordInvite.ErrorImage = null;
		this.pbDiscordInvite.ImageLocation = "https://dl.dropboxusercontent.com/scl/fi/9wwjy3c3m1k93cukji8pi/DiscordLogo.png?rlkey=jepz6b1mwopavefhvtbu7slmh&raw=1";
		this.pbDiscordInvite.InitialImage = null;
		this.pbDiscordInvite.Location = new System.Drawing.Point(232, 0);
		this.pbDiscordInvite.Name = "pbDiscordInvite";
		this.pbDiscordInvite.Size = new System.Drawing.Size(57, 55);
		this.pbDiscordInvite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pbDiscordInvite.TabIndex = 3;
		this.pbDiscordInvite.TabStop = false;
		this.pbDiscordInvite.Click += new System.EventHandler(pbDiscordInvite_Click);
		this.pbDiscordInvite.MouseEnter += new System.EventHandler(pbDiscordInvite_MouseEnter);
		this.pbDiscordInvite.MouseLeave += new System.EventHandler(pbDiscordInvite_MouseLeave);
		this.pnlBL.Controls.Add(this.pnlc13);
		this.pnlBL.Controls.Add(this.lblMadeBy);
		this.pnlBL.Controls.Add(this.pnlc11);
		this.pnlBL.Controls.Add(this.pbYoutubeChannel);
		this.pnlBL.Dock = System.Windows.Forms.DockStyle.Left;
		this.pnlBL.Location = new System.Drawing.Point(0, 0);
		this.pnlBL.Name = "pnlBL";
		this.pnlBL.Size = new System.Drawing.Size(289, 55);
		this.pnlBL.TabIndex = 0;
		this.pnlc13.BackColor = System.Drawing.Color.Purple;
		this.pnlc13.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.pnlc13.Location = new System.Drawing.Point(58, 35);
		this.pnlc13.Name = "pnlc13";
		this.pnlc13.Size = new System.Drawing.Size(231, 1);
		this.pnlc13.TabIndex = 5;
		this.lblMadeBy.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.lblMadeBy.ForeColor = System.Drawing.Color.White;
		this.lblMadeBy.Location = new System.Drawing.Point(58, 36);
		this.lblMadeBy.Name = "lblMadeBy";
		this.lblMadeBy.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.lblMadeBy.Size = new System.Drawing.Size(231, 19);
		this.lblMadeBy.TabIndex = 4;
		this.lblMadeBy.Text = "Made by FLIPPY";
		this.lblMadeBy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.pnlc11.BackColor = System.Drawing.Color.Purple;
		this.pnlc11.Dock = System.Windows.Forms.DockStyle.Left;
		this.pnlc11.Location = new System.Drawing.Point(57, 0);
		this.pnlc11.Name = "pnlc11";
		this.pnlc11.Size = new System.Drawing.Size(1, 55);
		this.pnlc11.TabIndex = 3;
		this.pbYoutubeChannel.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pbYoutubeChannel.Dock = System.Windows.Forms.DockStyle.Left;
		this.pbYoutubeChannel.ErrorImage = null;
		this.pbYoutubeChannel.ImageLocation = "https://dl.dropboxusercontent.com/scl/fi/pd76fw36u1eg5uf02ocwq/YoutubeLogo.png?rlkey=9mqxrbao076llxub0glvyp25o&raw=1";
		this.pbYoutubeChannel.InitialImage = null;
		this.pbYoutubeChannel.Location = new System.Drawing.Point(0, 0);
		this.pbYoutubeChannel.Name = "pbYoutubeChannel";
		this.pbYoutubeChannel.Size = new System.Drawing.Size(57, 55);
		this.pbYoutubeChannel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pbYoutubeChannel.TabIndex = 2;
		this.pbYoutubeChannel.TabStop = false;
		this.pbYoutubeChannel.Click += new System.EventHandler(pbYoutubeChannel_Click);
		this.pbYoutubeChannel.MouseEnter += new System.EventHandler(pbYoutubeChannel_MouseEnter);
		this.pbYoutubeChannel.MouseLeave += new System.EventHandler(pbYoutubeChannel_MouseLeave);
		this.pnlc1.BackColor = System.Drawing.Color.Purple;
		this.pnlc1.Dock = System.Windows.Forms.DockStyle.Top;
		this.pnlc1.Location = new System.Drawing.Point(0, 57);
		this.pnlc1.Name = "pnlc1";
		this.pnlc1.Size = new System.Drawing.Size(710, 1);
		this.pnlc1.TabIndex = 2;
		this.pnlc2.BackColor = System.Drawing.Color.Purple;
		this.pnlc2.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.pnlc2.Location = new System.Drawing.Point(0, 401);
		this.pnlc2.Name = "pnlc2";
		this.pnlc2.Size = new System.Drawing.Size(710, 1);
		this.pnlc2.TabIndex = 3;
		this.flpnlMods.AutoScroll = true;
		this.flpnlMods.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
		this.flpnlMods.Dock = System.Windows.Forms.DockStyle.Fill;
		this.flpnlMods.Location = new System.Drawing.Point(0, 58);
		this.flpnlMods.Name = "flpnlMods";
		this.flpnlMods.Size = new System.Drawing.Size(710, 343);
		this.flpnlMods.TabIndex = 4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(10, 10, 10);
		base.ClientSize = new System.Drawing.Size(710, 457);
		base.Controls.Add(this.flpnlMods);
		base.Controls.Add(this.pnlc2);
		base.Controls.Add(this.pnlc1);
		base.Controls.Add(this.pnlCredits);
		base.Controls.Add(this.pnlTop);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		this.MaximumSize = new System.Drawing.Size(726, 496);
		this.MinimumSize = new System.Drawing.Size(726, 496);
		base.Name = "UpGun_Mod_Loader";
		this.Text = "Mod Loader";
		this.pnlTop.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pbUpGunLogo).EndInit();
		this.pnlTR.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pbUploadMod).EndInit();
		this.pnlTL.ResumeLayout(false);
		this.pnlTL.PerformLayout();
		this.pnlSearch2.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.pnlCredits.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pbNextPage).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pbPreviousPage).EndInit();
		this.pnlcale.ResumeLayout(false);
		this.pnlBR.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pbDiscordInvite).EndInit();
		this.pnlBL.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pbYoutubeChannel).EndInit();
		base.ResumeLayout(false);
	}
}
