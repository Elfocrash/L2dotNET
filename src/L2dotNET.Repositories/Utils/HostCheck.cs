using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Threading;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Shared;

namespace L2dotNET.Repositories.Utils
{
    public static class HostCheck
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        public static bool IsPingSuccessful(string host, int timeoutMs)
        {
            try
            {
                PingReply pingReply = new Ping().Send(host, timeoutMs, new byte[1]);
                return (pingReply != null) && (pingReply.Status == IPStatus.Success);
            }
            catch (Exception ex)
            {
                Log.Error($"HostCheck: IsPingSuccessful: {ex.Message}");
            }

            return false;
        }

        public static bool IsLocalIpAddress(string host)
        {
            try
            {
                // get host IP addresses
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);
                // get local IP addresses
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                // test if any host IP equals to any local IP or to localhost
                foreach (IPAddress hostIp in hostIPs)
                {
                    // is localhost
                    if (IPAddress.IsLoopback(hostIp))
                        return true;

                    // is local address
                    if (localIPs.Contains(hostIp))
                        return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"HostCheck: IsLocalIPAddress: {ex.Message}");
            }

            return false;
        }

        public static bool ServiceExists(string serviceName)
        {
            try
            {
              //  return ServiceController.GetServices().Any(service => service.ServiceName.StartsWithIgnoreCase(serviceName));
            }
            catch (Exception ex)
            {
                Log.Error($"HostCheck: ServiceExists: {ex.Message}");
            }

            return false;
        }

        public static bool IsServiceRunning(string serviceName)
        {
            try
            {
             //   return ServiceController.GetServices().Any(service => service.ServiceName.StartsWithIgnoreCase(serviceName) && (service.Status == ServiceControllerStatus.Running));
            }
            catch (Exception ex)
            {
                Log.Error($"HostCheck: IsServiceRunning: {ex.Message}");
            }

            return false;
        }

        public static void StartService(string serviceName, int timeoutMs)
        {
            /*ServiceController service = ServiceController.GetServices().FirstOrDefault(filter => filter.ServiceName.StartsWithIgnoreCase(serviceName));

            if (service == null)
                return;

            switch (service.Status)
            {
                case ServiceControllerStatus.Running:
                    break;
                case ServiceControllerStatus.Stopped:
                    try
                    {
                        TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMs);
                        service.Start();
                        service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"HostCheck: StartService: {ex.Message}");
                    }
                    break;
                default:
                    Thread.Sleep(timeoutMs);
                    break;
            }*/
        }
    }
}