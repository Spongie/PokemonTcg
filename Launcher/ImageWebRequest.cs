using System;
using System.Net;

namespace Launcher
{
    public partial class Controller
    {
        internal class ImageWebRequest : WebClient
		{
			protected override WebRequest GetWebRequest(Uri address)
			{
				WebRequest request = base.GetWebRequest(address);
				request.Timeout = 10000;
				return request;
			}
		}
	}
}
