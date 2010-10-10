using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;

namespace MapDslTests
{
	public class CreateXmlDemo
	{
		[Test, Explicit]
		public void ShowXml()
		{
			var document = Serialize(IntegrationTest.GetMapping());
			Console.Write(document);
		}

		protected static string Serialize(HbmMapping hbmElement)
		{
			var setting = new XmlWriterSettings { Indent = true };
			var serializer = new XmlSerializer(typeof(HbmMapping));
			using (var memStream = new MemoryStream(2048))
			using (var xmlWriter = XmlWriter.Create(memStream, setting))
			{
				serializer.Serialize(xmlWriter, hbmElement);
				memStream.Flush();
				memStream.Position = 0;
				using (var sr = new StreamReader(memStream))
				{
					return sr.ReadToEnd();
				}
			}
		}
	}
}