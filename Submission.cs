using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Net;

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

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            List<string> errors = new List<string>();

            try
            {
                XmlSchemaSet schemaSet = new XmlSchemaSet();

                XmlReaderSettings xsdReaderSettings = new XmlReaderSettings();
                xsdReaderSettings.DtdProcessing = DtdProcessing.Ignore;
                using (XmlReader xsdReader = XmlReader.Create(xsdUrl, xsdReaderSettings))
                {
                    XmlSchema schema = XmlSchema.Read(xsdReader, null);
                    schemaSet.Add(schema);
                }

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas = schemaSet;
                settings.DtdProcessing = DtdProcessing.Ignore;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

                settings.ValidationEventHandler += (sender, e) =>
                {
                    errors.Add(e.Message);
                };

                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read()) { }
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }

            if (errors.Count == 0)
            {
                return "No errors are found";
            }

            return string.Join("\n", errors);
        }

        // Q2.2
        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlUrl);
                string jsonText = JsonConvert.SerializeXmlNode(doc.DocumentElement, Newtonsoft.Json.Formatting.Indented);

                return jsonText;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}