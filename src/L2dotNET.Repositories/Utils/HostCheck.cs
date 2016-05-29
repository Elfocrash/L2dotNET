using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.ServiceProcess;

namespace L2dotNET.Repositories.Utils
{
    public static class HostCheck
    {
        public static bool IsPingSuccessful(string host, int pingTimeout)
        {
            try { return new Ping().Send(host, pingTimeout, new byte[1]).Status == IPStatus.Success; }
            catch { }
            return false;
        }
        public static bool IsLocalIPAddress(string host)
        {
            try
            { // get host IP addresses
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);
                // get local IP addresses
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                // test if any host IP equals to any local IP or to localhost
                foreach (IPAddress hostIP in hostIPs)
                {
                    // is localhost
                    if (IPAddress.IsLoopback(hostIP)) return true;
                    // is local address
                    foreach (IPAddress localIP in localIPs)
                    {
                        if (hostIP.Equals(localIP)) return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public static bool IsServiceRunning(string serviceName)
        {
            try
            {
                return ServiceController.GetServices().Any(service => service.ServiceName.StartsWith(serviceName) &&
                                                                      service.Status == ServiceControllerStatus.Running);
            }
            catch { }
            return false;
        }

        public static void StartService(string serviceName, int timeoutMilliseconds)
        {
            TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
            ServiceController service = ServiceController.GetServices().FirstOrDefault(filter => filter.ServiceName.StartsWith(serviceName) &&
                                                                                                  filter.Status != ServiceControllerStatus.Running);

            try
            {
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch { }
        }
    }
}