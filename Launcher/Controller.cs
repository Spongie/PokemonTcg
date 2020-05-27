using NetworkingCore;
using PokemonTcgSdk;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;

namespace Launcher
{
	public partial class Controller : INotifyPropertyChanged
    {
		private string info;
		private string versionNumber;
		private int minimumProgress;
		private long maximumProgress;
		private long progress;
		private bool updateEnabled;
		private bool launchEnabled;
		private ObservableCollection<string> logs;
		private NetworkPlayer networkConnection;
		private InfoService infoService;
		private VersionNumber newVersion;
		private Thread workerThread;
		public event PropertyChangedEventHandler PropertyChanged;

		public Controller()
		{
			MaximumProgress = 100;
			logs = new ObservableCollection<string>();
			BindingOperations.EnableCollectionSynchronization(Logs, new object());
			workerThread = new Thread(CheckVersions);
			workerThread.Start();
		}

		public void Disconnect()
		{
			networkConnection.Disconnect(true);
		}

		public void UpdateStart()
		{
			workerThread = new Thread(Update);
			workerThread.Start();
		}

		internal void LaunchGame()
		{
			networkConnection.Disconnect(true);
			Process.Start(Path.Combine("Client", "PokemonTCGClient.exe"));
			Environment.Exit(0);
		}

		private void Update()
		{
			Thread.Sleep(100);
			Directory.CreateDirectory("Client");
			MaximumProgress = 3;
			AddMessage("Downloading client archive...");

			var clientBytes = infoService.GetClientDownloadLink();

			Progress++;

			File.WriteAllBytes("tmpClient.zip", clientBytes);

			Progress++;

			AddMessage("Extracting client archive...");

			ZipFile.ExtractToDirectory("tmpClient.zip", "Client", true);
			File.Delete("tmpClient.zip");

			Progress++;

			Logs.Add("Cliented Extraced!");


			AddMessage("Verifying image-files...");

			var streamingAssetsFolder = Directory.GetDirectories("Client", "", SearchOption.AllDirectories).FirstOrDefault(x => x.Contains("StreamingAssets"));
			var assembliesFolder = Path.Combine(streamingAssetsFolder, "Assemblies");
			var sets = new List<IPokemonSet>();

			foreach (var cardFile in Directory.GetFiles(assembliesFolder))
			{
				var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(new FileInfo(cardFile).FullName);

				var types = assembly.GetExportedTypes();
				foreach (var typeInfo in types.Where(type => typeof(IPokemonSet).IsAssignableFrom(type) && !type.IsAbstract && type.Name != nameof(PokemonCard)))
				{
					sets.Add((IPokemonSet)Activator.CreateInstance(typeInfo));
				}
			}

			Progress = 0;
			MaximumProgress = sets.Count;

			AddMessage($"Loaded {MaximumProgress} sets, checking images...");

			foreach (var set in sets)
			{
				AddMessage("Checking " + set.GetBaseFolder());
				DownloadSet(set, Path.Combine(streamingAssetsFolder, "Cards", set.GetBaseFolder()));
			}

			AddMessage("Version updated! Press Launch to start the game");

			UpdateEnabled = false;
			LaunchEnabled = true;

			File.WriteAllText("version", newVersion.ToString());
		}

		private void DownloadSet(IPokemonSet set, string baseFolder)
		{
			AddMessage("Verifying cards...");

			var response = PokemonTcgSdk.Card.Get<Pokemon>(new Dictionary<string, string>
				{
					{ CardQueryTypes.SetCode, set.GetSetCode()},
					{ CardQueryTypes.PageSize, "500" },
				});

			var cards = response.Cards;

			Progress = 0;
			MaximumProgress = cards.Count;

			Directory.CreateDirectory(baseFolder);
			Directory.CreateDirectory(Path.Combine(baseFolder, "PokemonCards"));
			Directory.CreateDirectory(Path.Combine(baseFolder, "TrainerCards"));
			Directory.CreateDirectory(Path.Combine(baseFolder, "EnergyCards"));

			Parallel.ForEach(cards, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, (card) =>
			{
				if (card.Name == "Rainbow Energy")
				{
					card.SuperType = "Energy";
				}

				var typeFolder = GetTypeFolder(card.SuperType);
				string fileName = Path.Combine(baseFolder, typeFolder, GetPokemonFileName(card.Name));

				if (File.Exists(fileName))
				{
					Interlocked.Increment(ref progress);
					FirePropertyChanged(nameof(Progress));
					return;
				}

				DownloadCard(card.ImageUrlHiRes, card.ImageUrl, fileName);

				Interlocked.Increment(ref progress);
				FirePropertyChanged(nameof(Progress));
			});
		}

		private string GetTypeFolder(string superType)
		{
			switch (superType)
			{
				case "Trainer":
					return "TrainerCards";
				case "Pokémon":
					return "PokemonCards";
				case "Energy":
					return "EnergyCards";
				default:
					return "PokemonCards";
			}
		}

		private static void DownloadCard(string hiResUrl, string lowResUrl, string fileName)
		{
			using (var client = new ImageWebRequest())
			{
				try
				{
					client.DownloadFile(hiResUrl, fileName);
				}
				catch (WebException e)
				{
					if (e.Message.Contains("404"))
					{
						client.DownloadFile(lowResUrl, fileName);
					}
				}
			}
		}

		private static string GetPokemonFileName(string cardName)
		{
			return cardName.Replace(" ", "")
				.Replace("'", "")
				.Replace("´", "")
				.Replace("`", "")
				.Replace("-", "")
				.Replace(":", "")
				.Replace("é", "e")
				.Replace("’", "")
				.Replace("?", "")
				.Replace("!", "")
				.Replace("&#8217;", "") + ".png";
		}

		private void CheckVersions()
		{
			try
			{
				Logs.Add("Connecting to the server...");
				var tcp = new TcpClient();
				tcp.Connect("127.0.0.1", 1565);
				networkConnection = new NetworkPlayer(tcp);
				networkConnection.DataReceived += NetworkPlayer_DataReceived;

				while (networkConnection.Id == null)
				{
					Thread.Sleep(25);
				}

				infoService = new InfoService(networkConnection);

				AddMessage("Checking versions...");

				newVersion = infoService.GetVersion();

				if (!File.Exists("version"))
				{
					File.WriteAllText("version", "0.0.000");
				}

				var localVersion = new VersionNumber(File.ReadAllText("version"));

				if (localVersion < newVersion)
				{
					AddMessage($"Version {newVersion} available, click update to download the update");
					UpdateEnabled = true;
				}
				else
				{
					Info = "Version up to date, press Launch to start the game";
					LaunchEnabled = true;
				}

				VersionNumber = localVersion.ToString();

			}
			catch (Exception e)
			{
				Info = "Failed to connect to the server";
			}
		}

		private void AddMessage(string message)
		{
			Logs.Add(message);
			Info = message;
		}

		private void NetworkPlayer_DataReceived(object sender, NetworkDataRecievedEventArgs e)
		{
			if (e.Message.MessageType == MessageTypes.Connected)
			{
				networkConnection.Id = (NetworkId)e.Message.Data;
				Console.WriteLine("Received connectionId: " + networkConnection.Id);
			}
		}

		private void FirePropertyChanged([CallerMemberName]string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

		public long Progress
		{
			get { return progress; }
			set
			{
				progress = value;
				FirePropertyChanged();
			}
		}

		public long MaximumProgress
		{
			get { return maximumProgress; }
			set
			{
				maximumProgress = value;
				FirePropertyChanged();
			}
		}

		public int MinimumProgress
		{
			get { return minimumProgress; }
			set
			{
				minimumProgress = value;
				FirePropertyChanged();
			}
		}


		public string VersionNumber
		{
			get { return versionNumber; }
			set
			{
				versionNumber = value;
				FirePropertyChanged();
			}
		}


		public string Info
		{
			get { return info; }
			set
			{
				info = value;
				FirePropertyChanged();
			}
		}

		public ObservableCollection<string> Logs
		{
			get { return logs; }
			set
			{
				logs = value;
				FirePropertyChanged();
			}
		}

		public bool LaunchEnabled
		{
			get { return launchEnabled; }
			set
			{
				launchEnabled = value;
				FirePropertyChanged();
			}
		}

		public bool UpdateEnabled
		{
			get { return updateEnabled; }
			set
			{
				updateEnabled = value;
				FirePropertyChanged();
			}
		}
	}
}
