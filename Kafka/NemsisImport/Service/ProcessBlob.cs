using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Azure;
using NemsisImport.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NemsisImport.Service
{
    public class ProcessBlob : IFileExtractService
    {
        private IBlobService blobService;
        public ProcessBlob(IBlobService _blobService)
        {
            blobService = _blobService;
        }

        public async Task GetFiles()
        {
            //ReadCSVFiles();

            //blobService.
            BlobService blobServ = new BlobService();
            Console.WriteLine("********************List of Containers************************");
            await blobServ.ListContainers();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("**************************************************************");
            Console.WriteLine("******************Save a File/Blob****************************");
            await blobServ.UploadBlobAsync();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("**************************************************************");
            Console.WriteLine("******************List all the blobs of a container with indexTags****************************");
            await blobServ.ListBlobsWithMetadataAsync();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("**************************************************************");
            Console.WriteLine("******************List all the blobs of a container****************************");
            await blobServ.GetAllBlobsinContainer();
            Console.WriteLine("******************List all the blobs of a container****************************");
            Console.WriteLine("******************Move a blob from Source to Processed container****************************");
            await blobServ.MoveFileAsync();
            Console.WriteLine("******************Move a blob from Source to Processed container****************************");



        }

        private void ReadCSVFiles()
        {
            string jsonFilesDirectory = @"C:\Projects\EPCR1500\EPCR7Log\EPCR7Log";
            string csvOutputFile = @"C:\Projects\EPCR1500\EPCR7Log\output\file.csv";

            List<YourJsonObjectType> combinedData = new List<YourJsonObjectType>();

            // Assuming YourJsonObjectType is the class representing the structure of your JSON object
            foreach (string jsonFilePath in Directory.GetFiles(jsonFilesDirectory, "*.*"))
            {
                string jsonContent = "["+File.ReadAllText(jsonFilePath)+"]";

                List<YourJsonObjectType> jsonData = JsonConvert.DeserializeObject<List<YourJsonObjectType>>(jsonContent);

                combinedData.AddRange(jsonData);
            }

            SaveToCsv(combinedData, csvOutputFile);

            Console.WriteLine("CSV file created successfully.");
        }

         void SaveToCsv<T>(List<T> data, string filePath)
        {
            using (var writer = new StreamWriter(filePath,false,System.Text.Encoding.UTF8))
            using (var csv = new CsvWriter(writer,CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(data);
            }
        }
    }

    public class YourJsonObjectType
    {
        public string time { get; set; }
        public string  level { get; set; }// Define properties corresponding to your JSON structure

        public string logger { get; set; }
        public string message { get; set; }
    }
}
