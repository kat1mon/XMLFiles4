using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;



/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{


    public class Submission
    {
        public static string xmlURL = "https://raw.githubusercontent.com/kat1mon/XMLFiles4/refs/heads/main/NationalParks.xml";
        public static string xmlErrorURL = "https://raw.githubusercontent.com/kat1mon/XMLFiles4/refs/heads/main/NationalParksErrors.xml";
        public static string xsdURL = "https://raw.githubusercontent.com/kat1mon/XMLFiles4/refs/heads/main/NationalParks.xsd";
        public static bool validError;
        public static string xsdError;

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);


            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);


            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        private static void ValidationCallback(object s, ValidationEventArgs e)
        {
            validError = true;
            xsdError += ("Validation Error: " + e.Message) + "\n";
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            //return "No Error" if XML is valid. Otherwise, return the desired exception message.
            validError = false;
            xsdError = "";

            //Create schema set
            XmlSchemaSet fileSchema = new XmlSchemaSet();

            //Setup the XSD file for checking schema
            XmlReaderSettings xsdSettings = new XmlReaderSettings();
            xsdSettings.DtdProcessing = DtdProcessing.Parse;

            //Create reader for XSD
            using (XmlReader xsdReader = XmlReader.Create(xsdUrl, xsdSettings))
            {
                fileSchema.Add(null, xsdReader);
            }

            //Configure XML validation settings
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallback);
            settings.Schemas = fileSchema;
            Console.WriteLine("XSD setup successful");

            //Create and validate XML
            XmlReader r = XmlReader.Create(xmlUrl, settings);
            try
            {
                while (r.Read()) ;
            }
            catch (XmlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("The XML file validation has completed");
            if (validError)
            {
                return ("Errors found in XML file: " + xsdError);
            }
            else
            {
                return ("No Error");
            }
        }

        public static string Xml2Json(string xmlUrl)
        {
            // The returned jsonText needs to be de-serializable by Newtonsoft.Json package. (JsonConvert.DeserializeXmlNode(jsonText))
            //Load XmlDocument, then convert to Json
            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader r = XmlReader.Create(xmlUrl, settings);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(r);

            string jsonTxt = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented);
            return jsonTxt;
        }

}