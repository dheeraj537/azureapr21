﻿using az.Models;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace az
{
    public class Helper
    {
        public static async Task<bool> UploadBlob(
                IConfiguration config,
                    Ticket ticket)
        {
            string blobConnString = config.GetConnectionString("StorAccConnString");
            BlobServiceClient client = new BlobServiceClient(blobConnString);
            string container = config.GetValue<string>("Container");
            var containerClient = client.GetBlobContainerClient(container);

            string fileName = "mtbs.ticket." + Guid.NewGuid().ToString() + ".json";
            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            //memorystream
            using (var stream = new MemoryStream())
            {
                var serializer = JsonSerializer.Create(new JsonSerializerSettings());

                // Use the 'leave open' option to keep the memory stream open after the stream writer is disposed
                using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
                {
                    // Serialize the job to the StreamWriter
                    serializer.Serialize(writer, ticket);
                }

                // Rewind the stream to the beginning
                stream.Position = 0;

                // Upload the job via the stream
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            await SendMessageToServiceBusQueue(config, ticket);
            return true;
        }

        private static async Task<bool> SendMessageToServiceBusQueue(IConfiguration config, Ticket ticket)
        {
            string ConnString = config.GetConnectionString("ServiceBusConString");
            ServiceBusClient client = new ServiceBusClient(ConnString);
            string queueName = config.GetValue<string>("QueueName");
            ServiceBusSender sender = client.CreateSender(queueName);
            

                await sender.SendMessageAsync(new ServiceBusMessage(JsonConvert.SerializeObject(ticket)));
            
           

            return true;
            
        }
    }
}
