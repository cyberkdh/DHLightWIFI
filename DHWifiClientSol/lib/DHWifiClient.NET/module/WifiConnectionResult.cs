//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiConnectionResult
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
namespace DHWifiClient.NET.module
{
    /// <summary>
    /// Connection outcome returned by the `DHWifiClient2` wait helper methods.
    /// A `Failed` result can mean either an explicit failure notification or a completion notification
    /// whose final connected network did not match the caller's expectation.
    /// </summary>
    public sealed class WifiConnectionResult
    {
        internal WifiConnectionResult(WifiWaitStatus status, WifiNotificationType notificationType,
            string expectedSsid, WifiNetwork connectedNetwork, string message)
        {
            Status = status;
            NotificationType = notificationType;
            ExpectedSsid = expectedSsid;
            ConnectedNetwork = connectedNetwork;
            Message = message;
        }

        /// <summary>Overall wait result status.</summary>
        public WifiWaitStatus Status { get; }

        /// <summary>The notification type that ended the wait, if any.</summary>
        public WifiNotificationType NotificationType { get; }

        /// <summary>The SSID the caller expected to connect to, when known.</summary>
        public string ExpectedSsid { get; }

        /// <summary>The connection confirmed after the completion notification, or null.</summary>
        public WifiNetwork ConnectedNetwork { get; }

        /// <summary>
        /// Human-readable summary of the connection outcome.
        /// Intended for logs or UI messages rather than stable programmatic branching.
        /// </summary>
        public string Message { get; }

        /// <summary>True when the wait completed successfully and the expected network was confirmed.</summary>
        public bool IsSuccess => Status == WifiWaitStatus.Success;

        internal static WifiConnectionResult CreateSuccess(WifiNotificationType notificationType,
            string expectedSsid, WifiNetwork connectedNetwork, string message)
        {
            return new WifiConnectionResult(
                WifiWaitStatus.Success,
                notificationType,
                expectedSsid,
                connectedNetwork,
                message);
        }

        internal static WifiConnectionResult CreateFailed(WifiNotificationType notificationType,
            string expectedSsid, WifiNetwork connectedNetwork, string message)
        {
            return new WifiConnectionResult(
                WifiWaitStatus.Failed,
                notificationType,
                expectedSsid,
                connectedNetwork,
                message);
        }

        internal static WifiConnectionResult CreateTimedOut(string expectedSsid)
        {
            return new WifiConnectionResult(
                WifiWaitStatus.TimedOut,
                WifiNotificationType.Unknown,
                expectedSsid,
                connectedNetwork: null,
                "Timed out while waiting for a connection result notification.");
        }
    }
}
