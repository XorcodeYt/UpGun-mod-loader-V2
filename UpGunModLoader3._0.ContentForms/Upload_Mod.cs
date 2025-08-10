using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using CustomControls.RJControls;

namespace UpGunModLoader3._0.ContentForms;

public class Upload_Mod : Form
{
	private Color GetTextColor = Color.Red;

	private int GetLineCount;

	private IContainer components;

	private Button btnSendMod;

	private TextBox tbUserName;

	private TextBox tbModName;

	private TextBox tbModImage;

	private TextBox tbModARZip;

	private ComboBox cbModType;

	private Panel pnlUsername;

	private Label lblUsername;

	private Label lblModName;

	private Panel pnlModName;

	private Label lblModType;

	private Panel pnlModType;

	private Label lblModImage;

	private Panel pnlModImage;

	private Label lblModARZip;

	private Panel pnlModARZip;

	private Label lblSelectZip;

	private Label lblSelectImage;

	private Panel pnlUploadNewMod;

	private Panel pnlIsUpdate;

	private SwitchButton sbUpdate;

	private Label lblIsUpdate;

	private Panel pnlc;

	private Panel pnlUpdate;

	private FlowLayoutPanel flpMyModsList;

	private Label lblModUpdatePath;

	private Button btnSendModUpdate;

	private Panel pnlSelectZip2;

	private Label lblSelectZip2;

	private TextBox tbModUpdatePath;

	private Panel pnlSelectedMod;

	private TextBox tbSelectedMod;

	private Label lblSelectedMod;

	private Panel pnlc1;

	private Button btnDeleteMod;

	public Upload_Mod()
	{
		InitializeComponent();
		pnlUpdate.Visible = false;
		base.Size = new Size(406, 407);
		using (StreamReader streamReader = new StreamReader(Fonctions.appdatapath5))
		{
			string text = streamReader.ReadLine();
			if (text != null)
			{
				tbUserName.Text = text;
				tbUserName.Enabled = false;
			}
		}
		CheckMyMods();
	}

	private async void btnSendMod_Click(object sender, EventArgs e)
	{
		if (tbUserName.Text != "")
		{
			if (tbUserName.Enabled)
			{
				if (MessageBox.Show("Is that your username?\nIf you press \"Yes\" you couldnt go back!", "Confirmation", MessageBoxButtons.YesNo) != DialogResult.Yes)
				{
					MessageBox.Show("You have to set your username!");
					return;
				}
				File.AppendAllText(Fonctions.appdatapath5, tbUserName.Text);
				tbUserName.Enabled = false;
			}
			if (tbUserName.Enabled)
			{
				return;
			}
			if (tbModName.Text != "")
			{
				if (cbModType.Text != "")
				{
					if (tbModImage.Text != "")
					{
						if (tbModARZip.Text != "")
						{
							btnSendMod.Enabled = false;
							await SendMods(tbUserName.Text + "," + tbModName.Text + "," + DateTime.Now.ToString("dd/MM/yyyy") + "," + cbModType.Text, tbModImage.Text, tbModARZip.Text);
							tbModName.Text = null;
							cbModType.Text = null;
							tbModImage.Text = null;
							tbModARZip.Text = null;
							btnSendMod.Enabled = true;
						}
						else
						{
							MessageBox.Show("There is no mod!");
						}
					}
					else
					{
						MessageBox.Show("There is no mod image!");
					}
				}
				else
				{
					MessageBox.Show("There is no mod type!");
				}
			}
			else
			{
				MessageBox.Show("There is no mod name!");
			}
		}
		else
		{
			MessageBox.Show("There is no username!");
		}
	}

	private static bool CheckFileSizeLimit(string filePath1, string filePath2, long limitBytes)
	{
		return GetFileSize(filePath1) + GetFileSize(filePath2) <= limitBytes;
	}

	private static bool CheckFileSizeLimit(string filePath1, long limitBytes)
	{
		return GetFileSize(filePath1) <= limitBytes;
	}

	private static long GetFileSize(string filePath)
	{
		if (File.Exists(filePath))
		{
			return new FileInfo(filePath).Length;
		}
		return 0L;
	}

	private void lblSelectImage_Click(object sender, EventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "Image file (*.png;*.jpg)|*.png;*.jpg";
		openFileDialog.Title = "Select an image";
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			string fileName = openFileDialog.FileName;
			string extension = Path.GetExtension(fileName);
			if (extension.Equals(".png", StringComparison.OrdinalIgnoreCase) || extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase))
			{
				tbModImage.Text = fileName;
			}
			else
			{
				MessageBox.Show("This is not a PNG or JPG file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		else
		{
			MessageBox.Show("No file selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void lblSelectZip_Click(object sender, EventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "Zip file (*.zip)|*.zip";
		openFileDialog.Title = "Select a mod";
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			string fileName = openFileDialog.FileName;
			if (Path.GetExtension(fileName).Equals(".zip", StringComparison.OrdinalIgnoreCase))
			{
				tbModARZip.Text = fileName;
			}
			else
			{
				MessageBox.Show("This is not a zip file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		else
		{
			MessageBox.Show("No file selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void lblSelectImage_MouseEnter(object sender, EventArgs e)
	{
		lblSelectImage.BackColor = Color.FromArgb(70, 70, 70);
	}

	private void lblSelectImage_MouseLeave(object sender, EventArgs e)
	{
		lblSelectImage.BackColor = Color.FromArgb(40, 40, 40);
	}

	private void lblSelectZip_MouseEnter(object sender, EventArgs e)
	{
		lblSelectZip.BackColor = Color.FromArgb(70, 70, 70);
	}

	private void lblSelectZip_MouseLeave(object sender, EventArgs e)
	{
		lblSelectZip.BackColor = Color.FromArgb(40, 40, 40);
	}

	private void btnSendMod_MouseEnter(object sender, EventArgs e)
	{
		btnSendMod.BackColor = Color.FromArgb(30, 30, 30);
	}

	private void btnSendMod_MouseLeave(object sender, EventArgs e)
	{
		btnSendMod.BackColor = Color.Black;
	}

	private void sbUpdate_CheckedChanged(object sender, EventArgs e)
	{
		if (sbUpdate.Checked)
		{
			pnlUploadNewMod.Visible = false;
			pnlUpdate.Visible = true;
		}
		else
		{
			pnlUpdate.Visible = false;
			pnlUploadNewMod.Visible = true;
		}
	}

	private void lblSelectZip2_Click(object sender, EventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "Zip file (*.zip)|*.zip";
		openFileDialog.Title = "Select a mod";
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			string fileName = openFileDialog.FileName;
			if (Path.GetExtension(fileName).Equals(".zip", StringComparison.OrdinalIgnoreCase))
			{
				tbModUpdatePath.Text = fileName;
			}
			else
			{
				MessageBox.Show("This is not a zip file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		else
		{
			MessageBox.Show("No file selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private async void CheckMyMods()
	{
		using StreamReader sr = new StreamReader(Fonctions.appdatapath4);
		string text = await Fonctions.GetUploadedMods();
		int num = 1;
		string text2;
		while ((text2 = sr.ReadLine()) != null)
		{
			string[] array = text2.Split(',');
			string[] array2 = text.Split(new string[2] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			bool flag = false;
			string[] array3 = array2;
			for (int i = 0; i < array3.Length; i++)
			{
				string[] array4 = array3[i].Split(',');
				if (array[0] == array4[0] && array[1] == array4[1] && array[2] == array4[2] && array[3] == array4[6])
				{
					LoadMyMods(text2, Color.Green, num);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				LoadMyMods(text2, Color.Red, num);
			}
			num++;
		}
	}

	private void LoadMyMods(string Line, Color LabelColor, int IndexOfLine)
	{
		Panel panel = new Panel
		{
			BackColor = Color.FromArgb(20, 20, 20),
			Size = new Size(363, 35)
		};
		Label lblModName = new Label
		{
			Text = Line,
			ForeColor = LabelColor,
			Cursor = Cursors.Hand,
			TextAlign = ContentAlignment.MiddleCenter,
			AutoSize = false,
			Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Bold),
			BackColor = Color.FromArgb(20, 20, 20),
			Dock = DockStyle.Fill
		};
		lblModName.Click += delegate
		{
			GetTextColor = lblModName.ForeColor;
			GetLineCount = IndexOfLine;
			tbSelectedMod.Text = lblModName.Text;
		};
		flpMyModsList.Controls.Add(panel);
		panel.Controls.Add(lblModName);
	}

	private async void btnSendModUpdate_Click(object sender, EventArgs e)
	{
		if (tbSelectedMod.Text != "")
		{
			if (GetTextColor == Color.Green)
			{
				if (tbModUpdatePath.Text != "")
				{
					btnSendModUpdate.Enabled = false;
					await SendModUpdate("Update/ " + tbSelectedMod.Text, tbModUpdatePath.Text);
					tbSelectedMod.Text = null;
					tbModUpdatePath.Text = null;
					btnSendModUpdate.Enabled = true;
				}
				else
				{
					MessageBox.Show("There is no update file!");
				}
			}
			else
			{
				MessageBox.Show("Your mod is not on the loader!");
			}
		}
		else
		{
			MessageBox.Show("There is no selected mod!");
		}
	}

	private async void btnDeleteMod_Click(object sender, EventArgs e)
	{
		if (tbSelectedMod.Text != "")
		{
			btnDeleteMod.Enabled = false;
			string[] array = File.ReadAllLines(Fonctions.appdatapath4);
			if (GetLineCount >= 1 && GetLineCount <= array.Length)
			{
				array = array.Where((string line, int index) => index + 1 != GetLineCount).ToArray();
				File.WriteAllLines(Fonctions.appdatapath4, array);
			}
			string messageText = "Delete/ " + tbSelectedMod.Text;
			if (GetTextColor == Color.Green)
			{
				await SendModDelete(messageText);
			}
			tbSelectedMod.Text = null;
			tbModUpdatePath.Text = null;
			GetTextColor = Color.White;
			foreach (Control control in flpMyModsList.Controls)
			{
				control.Dispose();
			}
			flpMyModsList.Controls.Clear();
			CheckMyMods();
			btnDeleteMod.Enabled = true;
		}
		else
		{
			MessageBox.Show("There is no selected mod!");
		}
	}

	private static async Task SendModDelete(string messageText)
	{
		HttpClient httpClient = new HttpClient();
		try
		{
			MultipartFormDataContent multipartContent = new MultipartFormDataContent();
			try
			{
				multipartContent.Add((HttpContent)new StringContent(Fonctions.GetDiscordUserIdAndAvatar()[0]), "username");
				multipartContent.Add((HttpContent)new StringContent(messageText), "content");
				if (Fonctions.GetDiscordUserIdAndAvatar()[1] != null)
				{
					string text = "https://cdn.discordapp.com/avatars/" + Fonctions.GetDiscordUserIdAndAvatar()[0] + "/" + Fonctions.GetDiscordUserIdAndAvatar()[1] + "?size=1024";
					multipartContent.Add((HttpContent)new StringContent(text), "avatar_url");
				}
				HttpClient val = httpClient;
				if ((await val.PostAsync(await Fonctions.GetWHLink(), (HttpContent)(object)multipartContent)).IsSuccessStatusCode)
				{
					MessageBox.Show("The request for delete the mod has been uploaded!\nYou have to wait for the verification before seeing it deleted!");
				}
				else
				{
					MessageBox.Show("There is a probleme here...");
				}
			}
			finally
			{
				((IDisposable)multipartContent)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)httpClient)?.Dispose();
		}
	}

	private static async Task SendModUpdate(string messageText, string zipPath)
	{
		if (!Path.GetExtension(zipPath).Equals(".zip", StringComparison.OrdinalIgnoreCase))
		{
			MessageBox.Show("This is not a zip file!");
			return;
		}
		if (!CheckFileSizeLimit(zipPath, 26214400L))
		{
			MessageBox.Show("The zip file must not exceed 25 MO!");
			return;
		}
		HttpClient httpClient = new HttpClient();
		try
		{
			MultipartFormDataContent multipartContent = new MultipartFormDataContent();
			try
			{
				multipartContent.Add((HttpContent)new StringContent(Fonctions.GetDiscordUserIdAndAvatar()[0]), "username");
				multipartContent.Add((HttpContent)new StringContent(messageText), "content");
				if (Fonctions.GetDiscordUserIdAndAvatar()[1] != null)
				{
					string text = "https://cdn.discordapp.com/avatars/" + Fonctions.GetDiscordUserIdAndAvatar()[0] + "/" + Fonctions.GetDiscordUserIdAndAvatar()[1] + "?size=1024";
					multipartContent.Add((HttpContent)new StringContent(text), "avatar_url");
				}
				if (File.Exists(zipPath))
				{
					StreamContent val = new StreamContent((Stream)new FileStream(zipPath, FileMode.Open, FileAccess.Read));
					((HttpContent)val).Headers.ContentType = new MediaTypeHeaderValue("application/zip");
					multipartContent.Add((HttpContent)(object)val, Path.GetFileNameWithoutExtension(zipPath), Path.GetFileName(zipPath));
					HttpClient val2 = httpClient;
					if ((await val2.PostAsync(await Fonctions.GetWHLink(), (HttpContent)(object)multipartContent)).IsSuccessStatusCode)
					{
						MessageBox.Show("The mod update has been uploaded!\nYou have to wait for the verification before seeing it on the loader!");
					}
					else
					{
						MessageBox.Show("There is a probleme here...");
					}
					return;
				}
				MessageBox.Show("There is no mod!");
			}
			finally
			{
				((IDisposable)multipartContent)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)httpClient)?.Dispose();
		}
	}

	private static async Task SendMods(string messageText, string imagePath, string zipPath)
	{
		string extension = Path.GetExtension(imagePath);
		string extension2 = Path.GetExtension(zipPath);
		if (!extension.Equals(".png", StringComparison.OrdinalIgnoreCase) && !extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase))
		{
			MessageBox.Show("This is not an image!");
			return;
		}
		if (!extension2.Equals(".zip", StringComparison.OrdinalIgnoreCase))
		{
			MessageBox.Show("This is not a zip file!");
			return;
		}
		if (!CheckFileSizeLimit(zipPath, imagePath, 26214400L))
		{
			MessageBox.Show("The image and the zip file must not exceed 25 MO!");
			return;
		}
		HttpClient httpClient = new HttpClient();
		try
		{
			MultipartFormDataContent multipartContent = new MultipartFormDataContent();
			try
			{
				multipartContent.Add((HttpContent)new StringContent(Fonctions.GetDiscordUserIdAndAvatar()[0]), "username");
				multipartContent.Add((HttpContent)new StringContent("Upload/ " + messageText), "content");
				if (Fonctions.GetDiscordUserIdAndAvatar()[1] != null)
				{
					string text = "https://cdn.discordapp.com/avatars/" + Fonctions.GetDiscordUserIdAndAvatar()[0] + "/" + Fonctions.GetDiscordUserIdAndAvatar()[1] + "?size=1024";
					multipartContent.Add((HttpContent)new StringContent(text), "avatar_url");
				}
				if (File.Exists(imagePath))
				{
					StreamContent val = new StreamContent((Stream)new FileStream(imagePath, FileMode.Open, FileAccess.Read));
					((HttpContent)val).Headers.ContentType = new MediaTypeHeaderValue("image/png");
					multipartContent.Add((HttpContent)(object)val, "image", "image.png");
					if (File.Exists(zipPath))
					{
						StreamContent val2 = new StreamContent((Stream)new FileStream(zipPath, FileMode.Open, FileAccess.Read));
						((HttpContent)val2).Headers.ContentType = new MediaTypeHeaderValue("application/zip");
						multipartContent.Add((HttpContent)(object)val2, Path.GetFileNameWithoutExtension(zipPath), Path.GetFileName(zipPath));
						HttpClient val3 = httpClient;
						if ((await val3.PostAsync(await Fonctions.GetWHLink(), (HttpContent)(object)multipartContent)).IsSuccessStatusCode)
						{
							MessageBox.Show("The mod has been uploaded!\nYou have to wait for the verification before seeing it on the loader!");
							File.AppendAllText(Fonctions.appdatapath4, messageText + Environment.NewLine);
						}
						else
						{
							MessageBox.Show("There is a probleme here...");
						}
						return;
					}
					MessageBox.Show("There is no mod!");
					return;
				}
				MessageBox.Show("There is no mod image!");
			}
			finally
			{
				((IDisposable)multipartContent)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)httpClient)?.Dispose();
		}
	}

	private void btnDeleteMod_MouseEnter(object sender, EventArgs e)
	{
		btnDeleteMod.BackColor = Color.FromArgb(30, 30, 30);
	}

	private void btnDeleteMod_MouseLeave(object sender, EventArgs e)
	{
		btnDeleteMod.BackColor = Color.Black;
	}

	private void btnSendModUpdate_MouseEnter(object sender, EventArgs e)
	{
		btnSendModUpdate.BackColor = Color.FromArgb(30, 30, 30);
	}

	private void btnSendModUpdate_MouseLeave(object sender, EventArgs e)
	{
		btnSendModUpdate.BackColor = Color.Black;
	}

	private void lblSelectZip2_MouseEnter(object sender, EventArgs e)
	{
		lblSelectZip2.BackColor = Color.FromArgb(70, 70, 70);
	}

	private void lblSelectZip2_MouseLeave(object sender, EventArgs e)
	{
		lblSelectZip2.BackColor = Color.FromArgb(40, 40, 40);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpGunModLoader3._0.ContentForms.Upload_Mod));
		this.btnSendMod = new System.Windows.Forms.Button();
		this.tbUserName = new System.Windows.Forms.TextBox();
		this.tbModName = new System.Windows.Forms.TextBox();
		this.tbModImage = new System.Windows.Forms.TextBox();
		this.tbModARZip = new System.Windows.Forms.TextBox();
		this.cbModType = new System.Windows.Forms.ComboBox();
		this.pnlUsername = new System.Windows.Forms.Panel();
		this.lblUsername = new System.Windows.Forms.Label();
		this.lblModName = new System.Windows.Forms.Label();
		this.pnlModName = new System.Windows.Forms.Panel();
		this.lblModType = new System.Windows.Forms.Label();
		this.pnlModType = new System.Windows.Forms.Panel();
		this.lblModImage = new System.Windows.Forms.Label();
		this.pnlModImage = new System.Windows.Forms.Panel();
		this.lblSelectImage = new System.Windows.Forms.Label();
		this.lblModARZip = new System.Windows.Forms.Label();
		this.pnlModARZip = new System.Windows.Forms.Panel();
		this.lblSelectZip = new System.Windows.Forms.Label();
		this.pnlUploadNewMod = new System.Windows.Forms.Panel();
		this.pnlIsUpdate = new System.Windows.Forms.Panel();
		this.sbUpdate = new CustomControls.RJControls.SwitchButton();
		this.lblIsUpdate = new System.Windows.Forms.Label();
		this.pnlc = new System.Windows.Forms.Panel();
		this.pnlUpdate = new System.Windows.Forms.Panel();
		this.btnDeleteMod = new System.Windows.Forms.Button();
		this.pnlc1 = new System.Windows.Forms.Panel();
		this.pnlSelectedMod = new System.Windows.Forms.Panel();
		this.tbSelectedMod = new System.Windows.Forms.TextBox();
		this.lblSelectedMod = new System.Windows.Forms.Label();
		this.lblModUpdatePath = new System.Windows.Forms.Label();
		this.btnSendModUpdate = new System.Windows.Forms.Button();
		this.pnlSelectZip2 = new System.Windows.Forms.Panel();
		this.lblSelectZip2 = new System.Windows.Forms.Label();
		this.tbModUpdatePath = new System.Windows.Forms.TextBox();
		this.flpMyModsList = new System.Windows.Forms.FlowLayoutPanel();
		this.pnlUsername.SuspendLayout();
		this.pnlModName.SuspendLayout();
		this.pnlModType.SuspendLayout();
		this.pnlModImage.SuspendLayout();
		this.pnlModARZip.SuspendLayout();
		this.pnlUploadNewMod.SuspendLayout();
		this.pnlIsUpdate.SuspendLayout();
		this.pnlUpdate.SuspendLayout();
		this.pnlSelectedMod.SuspendLayout();
		this.pnlSelectZip2.SuspendLayout();
		base.SuspendLayout();
		this.btnSendMod.BackColor = System.Drawing.Color.Black;
		this.btnSendMod.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSendMod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSendMod.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSendMod.ForeColor = System.Drawing.Color.White;
		this.btnSendMod.Location = new System.Drawing.Point(153, 291);
		this.btnSendMod.Name = "btnSendMod";
		this.btnSendMod.Size = new System.Drawing.Size(76, 23);
		this.btnSendMod.TabIndex = 0;
		this.btnSendMod.Text = "Send Mod";
		this.btnSendMod.UseVisualStyleBackColor = false;
		this.btnSendMod.Click += new System.EventHandler(btnSendMod_Click);
		this.btnSendMod.MouseEnter += new System.EventHandler(btnSendMod_MouseEnter);
		this.btnSendMod.MouseLeave += new System.EventHandler(btnSendMod_MouseLeave);
		this.tbUserName.BackColor = System.Drawing.Color.Black;
		this.tbUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.tbUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tbUserName.ForeColor = System.Drawing.Color.White;
		this.tbUserName.Location = new System.Drawing.Point(5, 6);
		this.tbUserName.Name = "tbUserName";
		this.tbUserName.Size = new System.Drawing.Size(356, 20);
		this.tbUserName.TabIndex = 1;
		this.tbModName.BackColor = System.Drawing.Color.Black;
		this.tbModName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.tbModName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tbModName.ForeColor = System.Drawing.Color.White;
		this.tbModName.Location = new System.Drawing.Point(5, 6);
		this.tbModName.Name = "tbModName";
		this.tbModName.Size = new System.Drawing.Size(356, 20);
		this.tbModName.TabIndex = 2;
		this.tbModImage.BackColor = System.Drawing.Color.Black;
		this.tbModImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.tbModImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tbModImage.ForeColor = System.Drawing.Color.White;
		this.tbModImage.Location = new System.Drawing.Point(5, 6);
		this.tbModImage.Name = "tbModImage";
		this.tbModImage.Size = new System.Drawing.Size(328, 20);
		this.tbModImage.TabIndex = 3;
		this.tbModARZip.BackColor = System.Drawing.Color.Black;
		this.tbModARZip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.tbModARZip.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tbModARZip.ForeColor = System.Drawing.Color.White;
		this.tbModARZip.Location = new System.Drawing.Point(5, 6);
		this.tbModARZip.Name = "tbModARZip";
		this.tbModARZip.Size = new System.Drawing.Size(328, 20);
		this.tbModARZip.TabIndex = 5;
		this.cbModType.BackColor = System.Drawing.Color.Black;
		this.cbModType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbModType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.cbModType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.cbModType.ForeColor = System.Drawing.Color.White;
		this.cbModType.FormattingEnabled = true;
		this.cbModType.Items.AddRange(new object[8] { "Map", "Gun Skin", "Cosmetic", "Player Model", "Settings", "Lobby Settings", "HUD", "Font" });
		this.cbModType.Location = new System.Drawing.Point(5, 6);
		this.cbModType.Name = "cbModType";
		this.cbModType.Size = new System.Drawing.Size(356, 21);
		this.cbModType.TabIndex = 6;
		this.pnlUsername.BackColor = System.Drawing.Color.FromArgb(10, 10, 10);
		this.pnlUsername.Controls.Add(this.tbUserName);
		this.pnlUsername.Location = new System.Drawing.Point(12, 29);
		this.pnlUsername.Name = "pnlUsername";
		this.pnlUsername.Size = new System.Drawing.Size(366, 32);
		this.pnlUsername.TabIndex = 7;
		this.lblUsername.AutoSize = true;
		this.lblUsername.BackColor = System.Drawing.Color.FromArgb(10, 10, 10);
		this.lblUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblUsername.ForeColor = System.Drawing.Color.White;
		this.lblUsername.Location = new System.Drawing.Point(12, 13);
		this.lblUsername.Name = "lblUsername";
		this.lblUsername.Size = new System.Drawing.Size(78, 16);
		this.lblUsername.TabIndex = 8;
		this.lblUsername.Text = "Username";
		this.lblModName.AutoSize = true;
		this.lblModName.BackColor = System.Drawing.Color.FromArgb(10, 10, 10);
		this.lblModName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblModName.ForeColor = System.Drawing.Color.White;
		this.lblModName.Location = new System.Drawing.Point(12, 69);
		this.lblModName.Name = "lblModName";
		this.lblModName.Size = new System.Drawing.Size(82, 16);
		this.lblModName.TabIndex = 10;
		this.lblModName.Text = "Mod Name";
		this.pnlModName.BackColor = System.Drawing.Color.FromArgb(10, 10, 10);
		this.pnlModName.Controls.Add(this.tbModName);
		this.pnlModName.Location = new System.Drawing.Point(12, 85);
		this.pnlModName.Name = "pnlModName";
		this.pnlModName.Size = new System.Drawing.Size(366, 32);
		this.pnlModName.TabIndex = 9;
		this.lblModType.AutoSize = true;
		this.lblModType.BackColor = System.Drawing.Color.FromArgb(10, 10, 10);
		this.lblModType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblModType.ForeColor = System.Drawing.Color.White;
		this.lblModType.Location = new System.Drawing.Point(12, 125);
		this.lblModType.Name = "lblModType";
		this.lblModType.Size = new System.Drawing.Size(77, 16);
		this.lblModType.TabIndex = 12;
		this.lblModType.Text = "Mod Type";
		this.pnlModType.BackColor = System.Drawing.Color.FromArgb(10, 10, 10);
		this.pnlModType.Controls.Add(this.cbModType);
		this.pnlModType.Location = new System.Drawing.Point(12, 141);
		this.pnlModType.Name = "pnlModType";
		this.pnlModType.Size = new System.Drawing.Size(366, 32);
		this.pnlModType.TabIndex = 11;
		this.lblModImage.AutoSize = true;
		this.lblModImage.BackColor = System.Drawing.Color.FromArgb(10, 10, 10);
		this.lblModImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblModImage.ForeColor = System.Drawing.Color.White;
		this.lblModImage.Location = new System.Drawing.Point(12, 181);
		this.lblModImage.Name = "lblModImage";
		this.lblModImage.Size = new System.Drawing.Size(84, 16);
		this.lblModImage.TabIndex = 14;
		this.lblModImage.Text = "Mod Image";
		this.pnlModImage.BackColor = System.Drawing.Color.FromArgb(10, 10, 10);
		this.pnlModImage.Controls.Add(this.lblSelectImage);
		this.pnlModImage.Controls.Add(this.tbModImage);
		this.pnlModImage.Location = new System.Drawing.Point(12, 197);
		this.pnlModImage.Name = "pnlModImage";
		this.pnlModImage.Size = new System.Drawing.Size(366, 32);
		this.pnlModImage.TabIndex = 13;
		this.lblSelectImage.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
		this.lblSelectImage.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblSelectImage.Font = new System.Drawing.Font("MV Boli", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSelectImage.ForeColor = System.Drawing.Color.White;
		this.lblSelectImage.Location = new System.Drawing.Point(334, 6);
		this.lblSelectImage.Name = "lblSelectImage";
		this.lblSelectImage.Size = new System.Drawing.Size(27, 20);
		this.lblSelectImage.TabIndex = 7;
		this.lblSelectImage.Text = "...";
		this.lblSelectImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblSelectImage.Click += new System.EventHandler(lblSelectImage_Click);
		this.lblSelectImage.MouseEnter += new System.EventHandler(lblSelectImage_MouseEnter);
		this.lblSelectImage.MouseLeave += new System.EventHandler(lblSelectImage_MouseLeave);
		this.lblModARZip.AutoSize = true;
		this.lblModARZip.BackColor = System.Drawing.Color.FromArgb(10, 10, 10);
		this.lblModARZip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblModARZip.ForeColor = System.Drawing.Color.White;
		this.lblModARZip.Location = new System.Drawing.Point(12, 237);
		this.lblModARZip.Name = "lblModARZip";
		this.lblModARZip.Size = new System.Drawing.Size(93, 16);
		this.lblModARZip.TabIndex = 16;
		this.lblModARZip.Text = "Mod Zip File";
		this.pnlModARZip.BackColor = System.Drawing.Color.FromArgb(10, 10, 10);
		this.pnlModARZip.Controls.Add(this.lblSelectZip);
		this.pnlModARZip.Controls.Add(this.tbModARZip);
		this.pnlModARZip.Location = new System.Drawing.Point(12, 253);
		this.pnlModARZip.Name = "pnlModARZip";
		this.pnlModARZip.Size = new System.Drawing.Size(366, 32);
		this.pnlModARZip.TabIndex = 15;
		this.lblSelectZip.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
		this.lblSelectZip.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblSelectZip.Font = new System.Drawing.Font("MV Boli", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSelectZip.ForeColor = System.Drawing.Color.White;
		this.lblSelectZip.Location = new System.Drawing.Point(334, 6);
		this.lblSelectZip.Name = "lblSelectZip";
		this.lblSelectZip.Size = new System.Drawing.Size(27, 20);
		this.lblSelectZip.TabIndex = 6;
		this.lblSelectZip.Text = "...";
		this.lblSelectZip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblSelectZip.Click += new System.EventHandler(lblSelectZip_Click);
		this.lblSelectZip.MouseEnter += new System.EventHandler(lblSelectZip_MouseEnter);
		this.lblSelectZip.MouseLeave += new System.EventHandler(lblSelectZip_MouseLeave);
		this.pnlUploadNewMod.Controls.Add(this.pnlModName);
		this.pnlUploadNewMod.Controls.Add(this.lblModARZip);
		this.pnlUploadNewMod.Controls.Add(this.btnSendMod);
		this.pnlUploadNewMod.Controls.Add(this.pnlModARZip);
		this.pnlUploadNewMod.Controls.Add(this.pnlUsername);
		this.pnlUploadNewMod.Controls.Add(this.lblModImage);
		this.pnlUploadNewMod.Controls.Add(this.lblUsername);
		this.pnlUploadNewMod.Controls.Add(this.pnlModImage);
		this.pnlUploadNewMod.Controls.Add(this.lblModName);
		this.pnlUploadNewMod.Controls.Add(this.lblModType);
		this.pnlUploadNewMod.Controls.Add(this.pnlModType);
		this.pnlUploadNewMod.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.pnlUploadNewMod.Location = new System.Drawing.Point(0, 369);
		this.pnlUploadNewMod.Name = "pnlUploadNewMod";
		this.pnlUploadNewMod.Size = new System.Drawing.Size(390, 328);
		this.pnlUploadNewMod.TabIndex = 17;
		this.pnlIsUpdate.Controls.Add(this.sbUpdate);
		this.pnlIsUpdate.Controls.Add(this.lblIsUpdate);
		this.pnlIsUpdate.Dock = System.Windows.Forms.DockStyle.Top;
		this.pnlIsUpdate.Location = new System.Drawing.Point(0, 0);
		this.pnlIsUpdate.Name = "pnlIsUpdate";
		this.pnlIsUpdate.Size = new System.Drawing.Size(390, 39);
		this.pnlIsUpdate.TabIndex = 18;
		this.sbUpdate.AutoSize = true;
		this.sbUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
		this.sbUpdate.Location = new System.Drawing.Point(168, 8);
		this.sbUpdate.MinimumSize = new System.Drawing.Size(45, 22);
		this.sbUpdate.Name = "sbUpdate";
		this.sbUpdate.OffBackColor = System.Drawing.Color.Gray;
		this.sbUpdate.OffToggleColor = System.Drawing.Color.Gainsboro;
		this.sbUpdate.OnBackColor = System.Drawing.Color.Green;
		this.sbUpdate.OnToggleColor = System.Drawing.Color.WhiteSmoke;
		this.sbUpdate.Size = new System.Drawing.Size(45, 22);
		this.sbUpdate.TabIndex = 1;
		this.sbUpdate.UseVisualStyleBackColor = true;
		this.sbUpdate.CheckedChanged += new System.EventHandler(sbUpdate_CheckedChanged);
		this.lblIsUpdate.AutoSize = true;
		this.lblIsUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblIsUpdate.ForeColor = System.Drawing.Color.White;
		this.lblIsUpdate.Location = new System.Drawing.Point(9, 9);
		this.lblIsUpdate.Name = "lblIsUpdate";
		this.lblIsUpdate.Size = new System.Drawing.Size(157, 20);
		this.lblIsUpdate.TabIndex = 0;
		this.lblIsUpdate.Text = "Is that an update?";
		this.pnlc.BackColor = System.Drawing.Color.Purple;
		this.pnlc.Dock = System.Windows.Forms.DockStyle.Top;
		this.pnlc.Location = new System.Drawing.Point(0, 39);
		this.pnlc.Name = "pnlc";
		this.pnlc.Size = new System.Drawing.Size(390, 1);
		this.pnlc.TabIndex = 19;
		this.pnlUpdate.Controls.Add(this.btnDeleteMod);
		this.pnlUpdate.Controls.Add(this.pnlc1);
		this.pnlUpdate.Controls.Add(this.pnlSelectedMod);
		this.pnlUpdate.Controls.Add(this.lblSelectedMod);
		this.pnlUpdate.Controls.Add(this.lblModUpdatePath);
		this.pnlUpdate.Controls.Add(this.btnSendModUpdate);
		this.pnlUpdate.Controls.Add(this.pnlSelectZip2);
		this.pnlUpdate.Controls.Add(this.flpMyModsList);
		this.pnlUpdate.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.pnlUpdate.Location = new System.Drawing.Point(0, 41);
		this.pnlUpdate.Name = "pnlUpdate";
		this.pnlUpdate.Size = new System.Drawing.Size(390, 328);
		this.pnlUpdate.TabIndex = 20;
		this.btnDeleteMod.BackColor = System.Drawing.Color.Black;
		this.btnDeleteMod.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDeleteMod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDeleteMod.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDeleteMod.ForeColor = System.Drawing.Color.White;
		this.btnDeleteMod.Location = new System.Drawing.Point(12, 296);
		this.btnDeleteMod.Name = "btnDeleteMod";
		this.btnDeleteMod.Size = new System.Drawing.Size(83, 23);
		this.btnDeleteMod.TabIndex = 23;
		this.btnDeleteMod.Text = "Delete Mod";
		this.btnDeleteMod.UseVisualStyleBackColor = false;
		this.btnDeleteMod.Click += new System.EventHandler(btnDeleteMod_Click);
		this.btnDeleteMod.MouseEnter += new System.EventHandler(btnDeleteMod_MouseEnter);
		this.btnDeleteMod.MouseLeave += new System.EventHandler(btnDeleteMod_MouseLeave);
		this.pnlc1.BackColor = System.Drawing.Color.Purple;
		this.pnlc1.Dock = System.Windows.Forms.DockStyle.Top;
		this.pnlc1.Location = new System.Drawing.Point(0, 175);
		this.pnlc1.Name = "pnlc1";
		this.pnlc1.Size = new System.Drawing.Size(390, 1);
		this.pnlc1.TabIndex = 22;
		this.pnlSelectedMod.BackColor = System.Drawing.Color.FromArgb(10, 10, 10);
		this.pnlSelectedMod.Controls.Add(this.tbSelectedMod);
		this.pnlSelectedMod.Location = new System.Drawing.Point(12, 201);
		this.pnlSelectedMod.Name = "pnlSelectedMod";
		this.pnlSelectedMod.Size = new System.Drawing.Size(366, 32);
		this.pnlSelectedMod.TabIndex = 20;
		this.tbSelectedMod.BackColor = System.Drawing.Color.Black;
		this.tbSelectedMod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.tbSelectedMod.Enabled = false;
		this.tbSelectedMod.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tbSelectedMod.ForeColor = System.Drawing.Color.White;
		this.tbSelectedMod.Location = new System.Drawing.Point(5, 6);
		this.tbSelectedMod.Name = "tbSelectedMod";
		this.tbSelectedMod.Size = new System.Drawing.Size(356, 20);
		this.tbSelectedMod.TabIndex = 1;
		this.lblSelectedMod.AutoSize = true;
		this.lblSelectedMod.BackColor = System.Drawing.Color.FromArgb(10, 10, 10);
		this.lblSelectedMod.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSelectedMod.ForeColor = System.Drawing.Color.White;
		this.lblSelectedMod.Location = new System.Drawing.Point(12, 185);
		this.lblSelectedMod.Name = "lblSelectedMod";
		this.lblSelectedMod.Size = new System.Drawing.Size(131, 16);
		this.lblSelectedMod.TabIndex = 21;
		this.lblSelectedMod.Text = "Selected Mod ↑↑↑";
		this.lblModUpdatePath.AutoSize = true;
		this.lblModUpdatePath.BackColor = System.Drawing.Color.FromArgb(10, 10, 10);
		this.lblModUpdatePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblModUpdatePath.ForeColor = System.Drawing.Color.White;
		this.lblModUpdatePath.Location = new System.Drawing.Point(12, 241);
		this.lblModUpdatePath.Name = "lblModUpdatePath";
		this.lblModUpdatePath.Size = new System.Drawing.Size(148, 16);
		this.lblModUpdatePath.TabIndex = 19;
		this.lblModUpdatePath.Text = "Mod Update Zip File";
		this.btnSendModUpdate.BackColor = System.Drawing.Color.Black;
		this.btnSendModUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnSendModUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSendModUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSendModUpdate.ForeColor = System.Drawing.Color.White;
		this.btnSendModUpdate.Location = new System.Drawing.Point(257, 296);
		this.btnSendModUpdate.Name = "btnSendModUpdate";
		this.btnSendModUpdate.Size = new System.Drawing.Size(121, 23);
		this.btnSendModUpdate.TabIndex = 17;
		this.btnSendModUpdate.Text = "Send Mod Update";
		this.btnSendModUpdate.UseVisualStyleBackColor = false;
		this.btnSendModUpdate.Click += new System.EventHandler(btnSendModUpdate_Click);
		this.btnSendModUpdate.MouseEnter += new System.EventHandler(btnSendModUpdate_MouseEnter);
		this.btnSendModUpdate.MouseLeave += new System.EventHandler(btnSendModUpdate_MouseLeave);
		this.pnlSelectZip2.BackColor = System.Drawing.Color.FromArgb(10, 10, 10);
		this.pnlSelectZip2.Controls.Add(this.lblSelectZip2);
		this.pnlSelectZip2.Controls.Add(this.tbModUpdatePath);
		this.pnlSelectZip2.Location = new System.Drawing.Point(12, 257);
		this.pnlSelectZip2.Name = "pnlSelectZip2";
		this.pnlSelectZip2.Size = new System.Drawing.Size(366, 32);
		this.pnlSelectZip2.TabIndex = 18;
		this.lblSelectZip2.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
		this.lblSelectZip2.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblSelectZip2.Font = new System.Drawing.Font("MV Boli", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSelectZip2.ForeColor = System.Drawing.Color.White;
		this.lblSelectZip2.Location = new System.Drawing.Point(334, 6);
		this.lblSelectZip2.Name = "lblSelectZip2";
		this.lblSelectZip2.Size = new System.Drawing.Size(27, 20);
		this.lblSelectZip2.TabIndex = 6;
		this.lblSelectZip2.Text = "...";
		this.lblSelectZip2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblSelectZip2.Click += new System.EventHandler(lblSelectZip2_Click);
		this.lblSelectZip2.MouseEnter += new System.EventHandler(lblSelectZip2_MouseEnter);
		this.lblSelectZip2.MouseLeave += new System.EventHandler(lblSelectZip2_MouseLeave);
		this.tbModUpdatePath.BackColor = System.Drawing.Color.Black;
		this.tbModUpdatePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.tbModUpdatePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tbModUpdatePath.ForeColor = System.Drawing.Color.White;
		this.tbModUpdatePath.Location = new System.Drawing.Point(5, 6);
		this.tbModUpdatePath.Name = "tbModUpdatePath";
		this.tbModUpdatePath.Size = new System.Drawing.Size(328, 20);
		this.tbModUpdatePath.TabIndex = 5;
		this.flpMyModsList.AutoScroll = true;
		this.flpMyModsList.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
		this.flpMyModsList.Dock = System.Windows.Forms.DockStyle.Top;
		this.flpMyModsList.Location = new System.Drawing.Point(0, 0);
		this.flpMyModsList.Name = "flpMyModsList";
		this.flpMyModsList.Size = new System.Drawing.Size(390, 175);
		this.flpMyModsList.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
		base.ClientSize = new System.Drawing.Size(390, 697);
		base.Controls.Add(this.pnlUpdate);
		base.Controls.Add(this.pnlc);
		base.Controls.Add(this.pnlIsUpdate);
		base.Controls.Add(this.pnlUploadNewMod);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		this.MinimumSize = new System.Drawing.Size(406, 407);
		base.Name = "Upload_Mod";
		this.Text = "Upload Mod";
		this.pnlUsername.ResumeLayout(false);
		this.pnlUsername.PerformLayout();
		this.pnlModName.ResumeLayout(false);
		this.pnlModName.PerformLayout();
		this.pnlModType.ResumeLayout(false);
		this.pnlModImage.ResumeLayout(false);
		this.pnlModImage.PerformLayout();
		this.pnlModARZip.ResumeLayout(false);
		this.pnlModARZip.PerformLayout();
		this.pnlUploadNewMod.ResumeLayout(false);
		this.pnlUploadNewMod.PerformLayout();
		this.pnlIsUpdate.ResumeLayout(false);
		this.pnlIsUpdate.PerformLayout();
		this.pnlUpdate.ResumeLayout(false);
		this.pnlUpdate.PerformLayout();
		this.pnlSelectedMod.ResumeLayout(false);
		this.pnlSelectedMod.PerformLayout();
		this.pnlSelectZip2.ResumeLayout(false);
		this.pnlSelectZip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
