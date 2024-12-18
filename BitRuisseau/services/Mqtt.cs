﻿using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.Design.AxImporter;

namespace BitRuisseau.services
{
    public class Mqtt
    {
        private static IMqttClient _client;
        private MqttClientOptions _options = new MqttClientOptions();
        public Mqtt(string broker, int port, string clientId, string username, string password)
        {
            // Create a MQTT client factory
            var factory = new MqttFactory();

            // Create a MQTT client instance
            _client = factory.CreateMqttClient();

            // Create MQTT client options
            _options = new MqttClientOptionsBuilder()
                .WithTcpServer(broker, port)
                .WithCredentials(username, password)
                .WithClientId(clientId)
                .Build();
        }
        public async Task ConnectToBroker()
        {
            var connectResult = await _client.ConnectAsync(_options);
            _client.ConnectedAsync += e =>
            {
                Debug.WriteLine($"Connected to Broker");
                return Task.CompletedTask;
            };
        }
        public async Task SubscribeToTopic(string topic)
        {
            await _client.SubscribeAsync(topic);

            _client.ApplicationMessageReceivedAsync += e =>
            {
                Debug.WriteLine($"Received message: {Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment)}");
                return Task.CompletedTask;
            };

        }
    }
}
