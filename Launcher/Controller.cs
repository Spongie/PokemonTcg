using NetworkingCore;
using Newtonsoft.Json;
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
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using TCGCards;
using TCGCards.Core;

namespace Launcher
{
    public partial class Controller : INotifyPropertyChanged
    {
		private string info;
		private string versionNumber;
		private string cardsVersionNumber;
		private int minimumProgress;
		private long maximumProgress;
		private long progress;
		private bool updateEnabled;
		private bool launchEnabled;
		private bool updateClient = false;
		private bool updateCards = false;
		private ObservableCollection<string> logs;
		private NetworkPlayer networkConnection;
		private InfoService infoService;
		private VersionNumber newVersion;
		private VersionNumber newCardVersion;
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
			UpdateEnabled = false;
			LaunchEnabled = false;
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
            if (updateClient)
            {
                DownloadClientUpdate();
            }

			if (updateCards)
            {
				UpdateCards();
			}

			Progress = maximumProgress;
			AddMessage("Version updated! Press Launch to start the game");

			UpdateEnabled = false;
			LaunchEnabled = true;
		}

        private void UpdateCards()
        {
            AddMessage("Updating card-files...");

            var streamingAssetsFolder = Directory.GetDirectories("Client", "", SearchOption.AllDirectories).FirstOrDefault(x => x.Contains("StreamingAssets"));
            var dataFolder = Path.Combine(streamingAssetsFolder, "Data");

            Directory.CreateDirectory(dataFolder);

            UpdateSets(streamingAssetsFolder, dataFolder);
            UpdatePokemonCards(streamingAssetsFolder, dataFolder);
            UpdateEnergyCards(streamingAssetsFolder, dataFolder);
            UpdateTrainerCards(streamingAssetsFolder, dataFolder);

            Directory.CreateDirectory(Path.Combine(streamingAssetsFolder, "Decks"));

            CardsVersionNumber = newCardVersion.ToString();

            File.WriteAllText("cards.version", CardsVersionNumber.ToString());
        }

        private void UpdateSets(string streamingAssetsFolder, string dataFolder)
        {
            var setsJson = infoService.GetSetsJson();
            File.WriteAllText(Path.Combine(dataFolder, "sets.json"), setsJson);

            foreach (var set in JsonConvert.DeserializeObject<List<Entities.Models.Set>>(setsJson))
            {
                var folder = Path.Combine(streamingAssetsFolder, "Cards", set.SetCode);

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
            }
        }

        private void UpdateTrainerCards(string streamingAssetsFolder, string dataFolder)
        {
            AddMessage("Updating trainer-cards...");
            var trainerJson = infoService.GetTrainerJson();
            File.WriteAllText(Path.Combine(dataFolder, "trainers.json"), trainerJson);
            var trainerCards = JsonConvert.DeserializeObject<List<LauncherCard>>(trainerJson).Where(x => x.Completed).ToList();
            progress = 0;
            MaximumProgress = trainerCards.Count;

            Parallel.For(0, trainerCards.Count, (i) =>
            {
                var card = trainerCards[i];

                var imageFile = Path.Combine(streamingAssetsFolder, "Cards", card.SetCode, card.GetImageName());

                if (File.Exists(imageFile))
                {
                    Interlocked.Increment(ref progress);
                    return;
                }

                DownloadCard(card.ImageUrl, card.ImageUrl, imageFile);
                Interlocked.Increment(ref progress);
                FirePropertyChanged(nameof(Progress));
            });
        }

        private void UpdateEnergyCards(string streamingAssetsFolder, string dataFolder)
        {
            AddMessage("Updating energy-cards...");
            var energyJson = infoService.GetEnergyJson();
            File.WriteAllText(Path.Combine(dataFolder, "energy.json"), energyJson);
            var energyCards = JsonConvert.DeserializeObject<List<LauncherCard>>(energyJson).Where(x => x.Completed).ToList();
            progress = 0;
            MaximumProgress = energyCards.Count;

            Parallel.For(0, energyCards.Count, (i) =>
            {
                var card = energyCards[i];

                var imageFile = Path.Combine(streamingAssetsFolder, "Cards", card.SetCode, card.GetImageName());

                if (File.Exists(imageFile))
                {
                    Interlocked.Increment(ref progress);
                    return;
                }

                DownloadCard(card.ImageUrl, card.ImageUrl, imageFile);
                Interlocked.Increment(ref progress);
                FirePropertyChanged(nameof(Progress));
            });
        }

        private void UpdatePokemonCards(string streamingAssetsFolder, string dataFolder)
        {
            AddMessage("Updating pokemon-cards...");
            var pokemonJson = infoService.GetPokemonJson();
            File.WriteAllText(Path.Combine(dataFolder, "pokemon.json"), pokemonJson);
            var pokemonCards = JsonConvert.DeserializeObject<List<LauncherCard>>(pokemonJson).Where(x => x.Completed).ToList();
            progress = 0;
            MaximumProgress = pokemonCards.Count;

            Parallel.For(0, pokemonCards.Count, (i) =>
            {
                var card = pokemonCards[i];

                var imageFile = Path.Combine(streamingAssetsFolder, "Cards", card.SetCode, card.GetImageName());

                if (File.Exists(imageFile))
                {
                    Interlocked.Increment(ref progress);
                    return;
                }

                DownloadCard(card.ImageUrl, card.ImageUrl, imageFile);
                Interlocked.Increment(ref progress);
                FirePropertyChanged(nameof(Progress));
            });
        }

        private void DownloadClientUpdate()
        {
            Thread.Sleep(100);
            Directory.CreateDirectory("Client");
            MaximumProgress = 3;
            AddMessage("Downloading client archive...");

            var clientBytes = infoService.GetClientBytes();

            Progress++;

            File.WriteAllBytes("tmpClient.zip", clientBytes);

            Progress++;

            AddMessage("Extracting client archive...");

            ZipFile.ExtractToDirectory("tmpClient.zip", "Client", true);
            File.Delete("tmpClient.zip");

            Progress++;

            Logs.Add("Cliented Extraced!");


			VersionNumber = newVersion.ToString();

			File.WriteAllText("client.version", newVersion.ToString());
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
				.Replace("♂", "")
				.Replace("&#8217;", "") + ".png";
		}

		private void CheckVersions()
		{
			try
			{
				Logs.Add("Connecting to the server...");
				
				var tcp = new TcpClient(AddressFamily.InterNetwork);

				tcp.Connect("85.90.244.171", 8080);
				//tcp.Connect("127.0.0.1", 8080);
				networkConnection = new NetworkPlayer(tcp);
				networkConnection.DataReceived += NetworkPlayer_DataReceived;

				while (networkConnection.Id == null)
				{
					Thread.Sleep(25);
				}

				infoService = new InfoService(networkConnection);

				AddMessage("Checking versions...");

				newVersion = infoService.GetVersion();

				if (!File.Exists("client.version"))
				{
					File.WriteAllText("client.version", "0.0.000");
				}

				var localVersion = new VersionNumber(File.ReadAllText("client.version"));

				if (localVersion < newVersion)
				{
					AddMessage($"Version {newVersion} available, click update to download the update");
					updateClient = true;
					UpdateEnabled = true;
				}
				else
				{
					Info = "Version up to date, press Launch to start the game";
					LaunchEnabled = true;
				}

				VersionNumber = localVersion.ToString();

				newCardVersion = infoService.GetCardsVersion();

				AddMessage("Checking card versions...");

				if (!File.Exists("cards.version"))
				{
					File.WriteAllText("cards.version", "0.0.000");
				}

				var localCardsVersion = new VersionNumber(File.ReadAllText("cards.version"));

				if (localCardsVersion < newCardVersion)
				{
					AddMessage($"Card version {newCardVersion} available, click update to download the update");
					updateCards = true;
					UpdateEnabled = true;
				}
				else
				{
					Info = "Version up to date, press Launch to start the game";
					LaunchEnabled = true;
				}

				CardsVersionNumber = localCardsVersion.ToString();
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

		public string CardsVersionNumber
		{
			get { return cardsVersionNumber; }
			set
			{
				cardsVersionNumber = value;
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
