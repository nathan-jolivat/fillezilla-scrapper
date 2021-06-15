using System;
using System.Buffers.Text;
using System.Text;

namespace FileZKO
{
	public class Server
	{
		public string Host { get; private set; }
		public int Port { get; private set; }
		public int Protocol { get; private set; }

		public string User { get; private set; }
		public string Password { get; private set; }
		public string DecodedPassword { get; private set; }

		public string PasvMode { get; private set; }
		public string EncodingType { get; private set; }

		public string Name { get; private set; }
		public Server(string name, string host, int port, int protocol, string user, string password, string pasvMode, string encodingType)
		{
			Name = name;
			Host = host;
			Port = port;
			Protocol = protocol;
			User = user;
			Password = password;

			byte[] bytes = Convert.FromBase64String(Password);
			DecodedPassword = Encoding.UTF8.GetString(bytes);

			PasvMode = pasvMode;
			EncodingType = encodingType;
		}
	}
}
