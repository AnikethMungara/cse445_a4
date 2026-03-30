using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/
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
            // return "No errors are found" if XML is valid. Otherwise, return the desired exception message.
            try
            {
                string xmlContent = DownloadContent(xmlUrl);
                string xsdContent = DownloadContent(xsdUrl);

                XmlSchemaSet schemaSet = new XmlSchemaSet();
                using (StringReader xsdReader = new StringReader(xsdContent))
                {
                    schemaSet.Add(null, XmlReader.Create(xsdReader));
                }

                List<string> errors = new List<string>();

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas = schemaSet;
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationEventHandler += (sender, e) =>
                {
                    errors.Add(e.Message);
                };

                using (StringReader xmlStringReader = new StringReader(xmlContent))
                using (XmlReader reader = XmlReader.Create(xmlStringReader, settings))
                {
                    while (reader.Read()) { }
                }

                if (errors.Count == 0)
                {
                    return "No errors are found";
                }
                else
                {
                    return string.Join("\n", errors);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Q2.2
        public static string Xml2Json(string xmlUrl)
        {
            // The returned jsonText needs to be de-serializable by Newtonsoft.Json package.
            // (JsonConvert.DeserializeXmlNode(jsonText))
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlUrl);
                return JsonConvert.SerializeXmlNode(doc.DocumentElement, Formatting.Indented);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Helper method to download content from URL
        private static string DownloadContent(string url)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }
}