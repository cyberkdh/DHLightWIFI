//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiClient
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH@HOTMAIL.COM. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DHWifiClient.NET.log;
using DHWifiClient.NET.win32;

namespace DHWifiClient.NET
{
    /// <summary>Entry point for the Windows Native WiFi API (wlanapi.dll).</summary>
    public class WifiClient : IDisposable
    {
        private const uint ClientVersion = 2; // Windows Vista or later

        private IntPtr m_pClientHandle;
        private bool m_bDisposed;

        // Kept as a field so the delegate is not garbage-collected while the native side still holds a pointer to it.
        private readonly WlanNotificationCallback m_oNotificationCallback;

        /// <summary>
        /// Raised on WLAN scan/connection lifecycle changes (see <see cref="WifiNotificationType"/>).
        /// Invoked on a native callback thread, not the UI thread — marshal to the UI thread yourself (e.g. Control.Invoke) before touching UI controls.
        /// </summary>
        public event EventHandler<WifiNotificationEventArgs> Notification;

        public WifiClient()
        {
            int nResult = WlanNativeMethods.WlanOpenHandle(
                ClientVersion, IntPtr.Zero, out _, out m_pClientHandle);
            WifiException.ThrowIfError(nResult);
            Logger.Info("WifiClient handle opened");

            m_oNotificationCallback = OnNativeNotification;
            int nNotifyResult = WlanNativeMethods.WlanRegisterNotification(
                m_pClientHandle, (uint)WlanNotificationSource.Acm, true,
                m_oNotificationCallback, IntPtr.Zero, IntPtr.Zero, out _);
            WifiException.ThrowIfError(nNotifyResult);
        }

        private void OnNativeNotification(ref WlanNotificationData notificationData, IntPtr context)
        {
            var eType = MapNotificationType(notificationData.NotificationCode);
            if (eType == WifiNotificationType.Unknown)
            {
                return;
            }

            Notification?.Invoke(this, new WifiNotificationEventArgs(notificationData.InterfaceGuid, eType));
        }

        private static WifiNotificationType MapNotificationType(uint notificationCode)
        {
            switch ((WlanNotificationCodeAcm)notificationCode)
            {
                case WlanNotificationCodeAcm.ScanComplete: return WifiNotificationType.ScanComplete;
                case WlanNotificationCodeAcm.ScanFail: return WifiNotificationType.ScanFailed;
                case WlanNotificationCodeAcm.ConnectionStart: return WifiNotificationType.ConnectionStarted;
                case WlanNotificationCodeAcm.ConnectionComplete: return WifiNotificationType.ConnectionCompleted;
                case WlanNotificationCodeAcm.ConnectionAttemptFail: return WifiNotificationType.ConnectionAttemptFailed;
                case WlanNotificationCodeAcm.Disconnecting: return WifiNotificationType.Disconnecting;
                case WlanNotificationCodeAcm.Disconnected: return WifiNotificationType.Disconnected;
                default: return WifiNotificationType.Unknown;
            }
        }

        /// <summary>Gets the list of WiFi interfaces (adapters) present on the system.</summary>
        public IReadOnlyList<WifiInterface> GetInterfaces()
        {
            ThrowIfDisposed();

            int nResult = WlanNativeMethods.WlanEnumInterfaces(m_pClientHandle, IntPtr.Zero, out IntPtr pListPtr);
            WifiException.ThrowIfError(nResult);

            var listInterfaces = new List<WifiInterface>();
            try
            {
                var oHeader = Marshal.PtrToStructure<WlanInterfaceInfoListHeader>(pListPtr);
                int nHeaderSize = Marshal.SizeOf<WlanInterfaceInfoListHeader>();
                int nItemSize = Marshal.SizeOf<WlanInterfaceInfo>();

                for (int ni = 0; ni < oHeader.NumberOfItems; ni++)
                {
                    IntPtr pItemPtr = IntPtr.Add(pListPtr, nHeaderSize + ni * nItemSize);
                    var oItem = Marshal.PtrToStructure<WlanInterfaceInfo>(pItemPtr);

                    listInterfaces.Add(new WifiInterface(
                        m_pClientHandle,
                        oItem.InterfaceGuid,
                        oItem.InterfaceDescription,
                        (WifiInterfaceState)oItem.State));
                }
            }
            finally
            {
                WlanNativeMethods.WlanFreeMemory(pListPtr);
            }

            Logger.Debug($"Found {listInterfaces.Count} WiFi interface(s)");
            return listInterfaces;
        }

        private void ThrowIfDisposed()
        {
            if (m_bDisposed)
            {
                throw new ObjectDisposedException(nameof(WifiClient));
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (m_bDisposed)
            {
                return;
            }

            if (m_pClientHandle != IntPtr.Zero)
            {
                WlanNativeMethods.WlanRegisterNotification(
                    m_pClientHandle, (uint)WlanNotificationSource.None, true,
                    null, IntPtr.Zero, IntPtr.Zero, out _);
                WlanNativeMethods.WlanCloseHandle(m_pClientHandle, IntPtr.Zero);
                m_pClientHandle = IntPtr.Zero;
                Logger.Info("WifiClient handle closed");
            }

            m_bDisposed = true;
        }

        ~WifiClient()
        {
            Dispose(false);
        }
    }
}
