using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Threading;
using log4net;

namespace L2dotNET.Repositories.Utils
{
    public static class HostCheck
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(HostCheck));

        public static bool IsPingSuccessful(string host, int timeoutMs)
        {
            try
            {
                var pingReply = new Ping().Send(host, timeoutMs, new byte[1]);
                return pingReply != null && pingReply.Status == IPStatus.Success;
            }
            catch (Exception ex)
            {
                log.Error($"HostCheck: IsPingSuccessful: {ex.Message}");
            }
            return false;
        }

        public static bool IsLocalIPAddress(string host)
        {
            try
            {
                // get host IP addresses
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);
                // get local IP addresses
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                // test if any host IP equals to any local IP or to localhost
                foreach (IPAddress hostIP in hostIPs)
                {
                    // is localhost
                    if (IPAddress.IsLoopback(hostIP))
                        return true;

                    // is local address
                    if (localIPs.Contains(hostIP))
                        return true;
                }
            }
            catch (Exception ex)
            {
                log.Error($"HostCheck: IsLocalIPAddress: {ex.Message}");
            }
            return false;
        }

        public static bool ServiceExists(string serviceName)
        {
            try
            {
                return ServiceController.GetServices().Any(service => service.ServiceName.StartsWith(serviceName));
            }
            catch (Exception ex)
            {
                log.Error($"HostCheck: ServiceExists: {ex.Message}");
            }
            return false;
        }

        public static bool IsServiceRunning(string serviceName)
        {
            try
            {
                return ServiceController.GetServices().Any(service => service.ServiceName.StartsWith(serviceName) && service.Status == ServiceControllerStatus.Running);
            }
            catch (Exception ex)
            {
                log.Error($"HostCheck: IsServiceRunning: {ex.Message}");
            }
            return false;
        }

        public static void StartService(string serviceName, int timeoutMs)
        {
            ServiceController service = ServiceController.GetServices().FirstOrDefault(filter => filter.ServiceName.StartsWith(serviceName));

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
                        log.Error($"HostCheck: StartService: {ex.Message}");
                    }
                    break;
                default:
                    Thread.Sleep(timeoutMs);
                    break;
            }
        }
    }
}