using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace FileZKO
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
			if (Environment.OSVersion.Version.Major >= 6)
			{
				path = Directory.GetParent(path).ToString();
				Console.WriteLine(path);

				//var userFiles = Directory.GetFiles(Path.Combine(path, "AppData", "Roaming", "FileZilla"));

				XmlDocument file = new XmlDocument();
				file.LoadXml(File.ReadAllText(Path.Combine(path, "AppData", "Roaming", "FileZilla", "sitemanager.xml")));

				XmlNodeList xnList = file.SelectNodes("/FileZilla3/Servers/Server");

				List<Server> keys = new List<Server>();

				foreach (XmlNode node in xnList)
				{
					keys.Add(new Server(
						node["Name"]?.InnerText,
						node["Host"]?.InnerText,
						int.Parse(node["Port"]?.InnerText),
						int.Parse(node["Protocol"]?.InnerText),
						node["User"]?.InnerText,
						node["Pass"]?.InnerText,
						node["PasvMode"]?.InnerText,
						node["EncodingType"]?.InnerText
						)
					);
				}

				string outputPath = Path.Combine(path, "servers_configs_exports");

				var parsedKeys = JsonConvert.SerializeObject(keys);

				try
				{
					// Create the file, or overwrite if the file exists.
					using (FileStream fs = File.Create(outputPath + ".json"))
					{
						string jsonFormatted = JToken.Parse(parsedKeys).ToString(Newtonsoft.Json.Formatting.Indented);

						byte[] info = new UTF8Encoding(true).GetBytes(jsonFormatted);
						// Add some information to the file.
						fs.Write(info, 0, info.Length);
					}
				}

				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}

				Console.ReadKey();
			}
		}
	}
}
