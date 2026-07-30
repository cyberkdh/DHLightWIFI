//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: Program
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using DHWifiClient.NET;
using DHWifiClient.NET.module;

namespace DHWifiClient2ConsoleSample
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            try
            {
                using (var client = CreateClient(args))
                {
                    Run(client);
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
                return 1;
            }
        }

        private static DHWifiClient2 CreateClient(string[] args)
        {
            if (args != null && args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
            {
                return new DHWifiClient2(args[0]);
            }

            return new DHWifiClient2();
        }

        private static void Run(DHWifiClient2 client)
        {
            Console.WriteLine("DHWifiClient2 Console Sample");
            Console.WriteLine("Interface: " + client.CurrentInterfaceName);
            Console.WriteLine();

            var scanStatus = client.ScanAndWait(10000);
            if (scanStatus == WifiWaitStatus.Failed)
            {
                Console.WriteLine("Warning: scan failed. The last known network list will be used.");
            }
            else if (scanStatus == WifiWaitStatus.TimedOut)
            {
                Console.WriteLine("Warning: scan timed out. The last known network list will be used.");
            }

            var networks = client.GetAvailableNetworks(mergeDuplicateBssids: true)
                .OrderByDescending(network => network.IsConnected)
                .ThenByDescending(network => network.SignalQuality)
                .ToList();

            if (networks.Count == 0)
            {
                Console.WriteLine("No Wi-Fi networks were found.");
                return;
            }

            PrintNetworks(networks);

            var selectedNetwork = SelectNetwork(networks);
            if (selectedNetwork == null)
            {
                Console.WriteLine("Canceled.");
                return;
            }

            ConnectAndWait(client, selectedNetwork);

            Console.WriteLine();
            Console.WriteLine("Press Enter to disconnect, or close the console to keep the current state.");
            Console.ReadLine();

            client.Disconnect();
            Console.WriteLine("Disconnected.");
        }

        private static void PrintNetworks(IReadOnlyList<WifiNetwork> networks)
        {
            for (int i = 0; i < networks.Count; i++)
            {
                var network = networks[i];
                string ssid = string.IsNullOrWhiteSpace(network.Ssid) ? "<hidden>" : network.Ssid;
                string security = network.SecurityEnabled ? network.Authentication.ToString() : "Open";
                string status = GetDisplayStatus(network);

                Console.WriteLine(
                    "{0,2}. [{1}] {2} | Signal {3,3}% | {4}",
                    i + 1,
                    status,
                    ssid,
                    network.SignalQuality,
                    security);
            }
        }

        private static string GetDisplayStatus(WifiNetwork network)
        {
            if (network.IsConnected)
            {
                return "Connected";
            }

            if (network.HasProfile)
            {
                return "Saved";
            }

            if (!network.SecurityEnabled)
            {
                return "Open";
            }

            return "-";
        }

        private static WifiNetwork SelectNetwork(IReadOnlyList<WifiNetwork> networks)
        {
            while (true)
            {
                Console.WriteLine();
                Console.Write("Select network number (Enter to cancel): ");
                string line = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(line))
                {
                    return null;
                }

                if (int.TryParse(line, out int index) && index >= 1 && index <= networks.Count)
                {
                    return networks[index - 1];
                }

                Console.WriteLine("Invalid selection.");
            }
        }

        private static void ConnectAndWait(DHWifiClient2 client, WifiNetwork network)
        {
            string ssid = string.IsNullOrWhiteSpace(network.Ssid) ? "<hidden>" : network.Ssid;
            string password = null;

            if (network.SecurityEnabled)
            {
                Console.Write("Password: ");
                password = Console.ReadLine();
            }

            Console.WriteLine();
            Console.WriteLine("Connecting to " + ssid + "...");
            Console.WriteLine("Connect request sent. Waiting for result...");

            var result = client.ConnectAndWait(
                network,
                password,
                millisecondsTimeout: 15000,
                mergeDuplicateBssids: true);

            Console.WriteLine(result.Message);
        }
    }
}
