using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;

namespace ConsoleApp1
{
    public class Submission
    {
        public static string xmlURL = "https://anikethmungara.github.io/cse445_a4/NationalParks.xml";
        public static string xmlErrorURL = "https://anikethmungara.github.io/cse445_a4/NationalParksErrors.xml";
        public static string xsdURL = "https://anikethmungara.github.io/cse445_a4/NationalParks.xsd";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        public static string Verification(string xmlUrl, string xsdUrl)
        {
            System.Text.StringBuilder errors = new System.Text.StringBuilder();

            try
            {
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                schemaSet.Add(null, xsdUrl);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas = schemaSet;

                settings.ValidationEventHandler += (sender, e) =>
                {
                    errors.AppendLine(e.Message);
                };

                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    try
                    {
                        while (reader.Read()) { }
                    }
                    catch (XmlException ex)
                    {
                        errors.AppendLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                errors.AppendLine(ex.Message);
            }

            if (errors.Length == 0)
            {
                return "No errors are found";
            }
            else
            {
                return errors.ToString().Trim();
            }
        }

        public static string Xml2Json(string xmlUrl)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlUrl);

            string jsonText = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc.DocumentElement, Newtonsoft.Json.Formatting.Indented);
            return jsonText;
        }
    }
}