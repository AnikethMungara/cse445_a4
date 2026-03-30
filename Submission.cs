using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;



/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{


    public class Submission
    {
        public static string xmlURL = "https://raw.githubusercontent.com/AnikethMungara/cse445_a4/main/NationalParks.xml";
        public static string xmlErrorURL = "https://raw.githubusercontent.com/AnikethMungara/cse445_a4/main/NationalParksErrors.xml";
        public static string xsdURL = "https://raw.githubusercontent.com/AnikethMungara/cse445_a4/main/NationalParks.xsd";

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
            try
            {
                string xsdContent = DownloadContent(xsdUrl);
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add(null, XmlReader.Create(new StringReader(xsdContent)));

                string xmlContent = DownloadContent(xmlUrl);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas = schemas;

                string errors = "";

                settings.ValidationEventHandler += (sender, e) =>
                {
                    if (errors != "")
                        errors += "\n";
                    errors += e.Message;
                };

                using (XmlReader reader = XmlReader.Create(new StringReader(xmlContent), settings))
                {
                    while (reader.Read()) { }
                }

                if (errors == "")
                    return "No errors are found";
                else
                    return errors;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string Xml2Json(string xmlUrl)
        {
            string xmlContent = DownloadContent(xmlUrl);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            string jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented, true);
            return jsonText;
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