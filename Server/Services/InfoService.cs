﻿using System.IO;
using TCGCards.Core;

namespace Server.Services
{
    public class InfoService : IService
    {
        public void InitTypes()
        {
            
        }

        public VersionNumber GetVersion() => new VersionNumber(File.ReadAllText("version"));

        public byte[] GetClientDownloadLink() => File.ReadAllBytes("Client.zip");
    }
}
