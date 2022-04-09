namespace ddns
{
    using System;
    using System.Net;
    using System.Security;
    using System.Threading;

    public unsafe static class Program
    {
        private static void HttpGet(string openUrl)
        {
            using (WebClient wc = new WebClient())
            {
                DateTime b = DateTime.Now;
                try
                {
                    string r = (wc.DownloadString(openUrl) ?? string.Empty).Trim();
                    Console.WriteLine("[{0}][{1}] {2}", b.ToString("yyyy-MM-dd HH:mm:ss"),
                        ((int)(DateTime.Now - b).TotalMilliseconds).ToString("d4"), r);
                }
                catch (Exception)
                {
                    Console.WriteLine("[{0}][{1}] ERROR", b.ToString("yyyy - MM - dd HH: mm:ss"),
                        ((int)(DateTime.Now - b).TotalMilliseconds).ToString("d4"));
                }
            }
        }

        [MTAThread]
        private static void Main(string[] args)
        {
            Console.Title = "DDNS@localhost";
            InitialSecurityProtocol();
            int sleepTime = 1000 * int.Parse(args[0]);
            string openUrl = args[1];
            while (true)
            {
                HttpGet(openUrl);
                Thread.Sleep(sleepTime);
            }
        }

        [SecurityCritical]
        [SecuritySafeCritical]
        private static void InitialSecurityProtocol()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                                              | SecurityProtocolType.Tls
                                              | (SecurityProtocolType)0x300 // Tls11
                                              | (SecurityProtocolType)0xC00 // Tls12
                                              | (SecurityProtocolType)0x3000; // Tls13
            }
            catch (Exception)
            {
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                                                  | SecurityProtocolType.Tls
                                                  | (SecurityProtocolType)0x300 // Tls11
                                                  | (SecurityProtocolType)0xC00; // Tls12
                }
                catch (Exception)
                {
                    try
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                                                        | SecurityProtocolType.Tls
                                                        | (SecurityProtocolType)0x300; // Tls11
                    }
                    catch (Exception)
                    {
                        try
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                                                            | SecurityProtocolType.Tls;
                        }
                        catch (Exception) { }
                    }
                }
            }
        }
    }
}
