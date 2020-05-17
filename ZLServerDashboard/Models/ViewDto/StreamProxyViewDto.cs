using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ZLServerDashboard.Models.Dto;
using static ZLServerDashboard.Models.Enums;

namespace ZLServerDashboard.Models.ViewDto
{

    public class StreamProxyViewDto
    {
        private static string ZLServerIp = Startup.Instance.Configuration.GetValue<String>("ZLMediaServer:Ip");
        private static string ZLServerHttpPort = Startup.Instance.Configuration.GetValue<String>("ZLMediaServer:HttpPort");
        private static string ZLServerHttpSSLPort = Startup.Instance.Configuration.GetValue<String>("ZLMediaServer:HttpSSLPort");

        private static string ZLServerRtspPort = Startup.Instance.Configuration.GetValue<String>("ZLMediaServer:RtspPort");
        private static string ZLServerRtspSSLPort = Startup.Instance.Configuration.GetValue<String>("ZLMediaServer:RtspSSLPort");

        private static string ZLServerRtmpPort = Startup.Instance.Configuration.GetValue<String>("ZLMediaServer:RtmpPort");

        public StreamProxyDto StreamProxy { get; set; }
        public ApplicationDto Application { get; set; }
        public DomainDto Domain { get; set; }
        public bool isOnline { get; set; }

        public String StreamProxyJson { get; set; }
        public String Token { get; set; }
        public String Ticket { get; set; }

        


        #region  RTSP地址

        public String RtspDomainUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {
                    // if (!String.IsNullOrWhiteSpace(Token))
                    // {
                    //     url = String.Format("rtsp://{0}{1}/{2}/{3}?Token={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Token);
                    // }
                    // else if (!String.IsNullOrWhiteSpace(Ticket))
                    // {
                    //     url = String.Format("rtsp://{0}{1}/{2}/{3}?Ticket={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Ticket);
                    // }
                    // else
                    // {
                    url = String.Format("rtsp://{0}{1}/{2}/{3}", Domain.DomainName, ZLServerRtspPort == "554" ? "" : ":" + ZLServerRtspPort, Application.App, StreamProxy.StreamName);
                    // }
                }
                return url;
            }
        }
        public String RtspUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {
                    // if (!String.IsNullOrWhiteSpace(Token))
                    // {
                    //     url = String.Format("rtsp://{0}{1}/{2}/{3}?vhost={4}&Token={5}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName, Token);
                    // }
                    // else if (!String.IsNullOrWhiteSpace(Ticket))
                    // {
                    //     url = String.Format("rtsp://{0}{1}/{2}/{3}?vhost={4}&Ticket={5}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName, Ticket);
                    // }
                    // else
                    // {
                    url = String.Format("rtsp://{0}{1}/{2}/{3}?vhost={4}", ZLServerIp, ZLServerRtspPort == "554" ? "" : ":" + ZLServerRtspPort, Application.App, StreamProxy.StreamName, Domain.DomainName);
                    // }
                }
                return url;
            }
        }

        public String RtspsDomainUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {
                    // if (!String.IsNullOrWhiteSpace(Token))
                    // {
                    //     url = String.Format("rtsps://{0}{1}/{2}/{3}?Token={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Token);
                    // }
                    // else if (!String.IsNullOrWhiteSpace(Ticket))
                    // {
                    //     url = String.Format("rtsps://{0}{1}/{2}/{3}?Ticket={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Ticket);
                    // }
                    //  else
                    // {
                    url = String.Format("rtsps://{0}{1}/{2}/{3}", Domain.DomainName, ZLServerRtspSSLPort == "332" ? "" : ":" + ZLServerRtspSSLPort, Application.App, StreamProxy.StreamName);
                    // }
                }
                return url;
            }
        }
        public String RtspsUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {

                    // if (!String.IsNullOrWhiteSpace(Token))
                    // {
                    //     url = String.Format("rtsps://{0}{1}/{2}/{3}?vhost={4}&Token={5}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName, Token);
                    // }
                    // else if (!String.IsNullOrWhiteSpace(Ticket))
                    // {
                    //     url = String.Format("rtsps://{0}{1}/{2}/{3}?vhost={4}&Ticket={5}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName, Ticket);
                    // }
                    //  else
                    // {
                    url = String.Format("rtsps://{0}{1}/{2}/{3}?vhost={4}", ZLServerIp, ZLServerRtspSSLPort == "332" ? "" : ":" + ZLServerRtspSSLPort, Application.App, StreamProxy.StreamName, Domain.DomainName);
                    // }
                }
                return url;
            }
        }

        #endregion



        #region  RTMP地址

        public String RtmpDomainUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {
                    // if (!String.IsNullOrWhiteSpace(Token))
                    // {
                    //     url = String.Format("rtmp://{0}{1}/{2}/{3}?Token={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Token);
                    // }
                    // else if (!String.IsNullOrWhiteSpace(Ticket))
                    // {
                    //     url = String.Format("rtmp://{0}{1}/{2}/{3}?Ticket={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Ticket);
                    // }
                    //  else
                    // {
                    url = String.Format("rtmp://{0}{1}/{2}/{3}", Domain.DomainName, ZLServerRtmpPort == "1935" ? "" : ":" + ZLServerRtmpPort, Application.App, StreamProxy.StreamName);
                    // }
                }
                return url;
            }
        }
        public String RtmpUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {
                    url = String.Format("rtmp://{0}{1}/{2}/{3}?vhost={4}", ZLServerIp, ZLServerRtmpPort == "1935" ? "" : ":" + ZLServerRtmpPort, Application.App, StreamProxy.StreamName, Domain.DomainName);
                }
                return url;
            }
        }

        public String RtmpsDomainUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {
                    // if (!String.IsNullOrWhiteSpace(Token))
                    // {
                    //     url = String.Format("rtmps://{0}{1}/{2}/{3}?Token={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Token);
                    // }
                    // else if (!String.IsNullOrWhiteSpace(Ticket))
                    // {
                    //     url = String.Format("rtmps://{0}{1}/{2}/{3}?Ticket={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Ticket);
                    // }
                    //  else
                    // {
                    url = String.Format("rtmps://{0}{1}/{2}/{3}", Domain.DomainName, "", Application.App, StreamProxy.StreamName);
                    // }
                }
                return url;
            }
        }
        public String RtmpsUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {
                    // if (!String.IsNullOrWhiteSpace(Token))
                    // {
                    //     url = String.Format("rtmps://{0}{1}/{2}/{3}?vhost={4}&Token={5}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName, Token);
                    // }
                    // else if (!String.IsNullOrWhiteSpace(Ticket))
                    // {
                    //     url = String.Format("rtmps://{0}{1}/{2}/{3}?vhost={4}&Ticket={5}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName, Ticket);
                    // }
                    //  else
                    // {
                    url = String.Format("rtmps://{0}{1}/{2}/{3}?vhost={4}", ZLServerIp,"", Application.App, StreamProxy.StreamName, Domain.DomainName);
                    // }
                }
                return url;
            }
        }

        #endregion



        #region  Http-flv地址

        public String HttpFlvDomainUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {
                    // if (!String.IsNullOrWhiteSpace(Token))
                    // {
                    //     url = String.Format("http://{0}{1}/{2}/{3}.flv?Token={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Token);
                    // }
                    // else if (!String.IsNullOrWhiteSpace(Ticket))
                    // {
                    //     url = String.Format("http://{0}{1}/{2}/{3}.flv?Ticket={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Ticket);
                    // }
                    //  else
                    // {
                    url = String.Format("http://{0}{1}/{2}/{3}.flv", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName);
                    // }
                }
                return url;
            }
        }
        public String HttpFlvUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {
                    // if (!String.IsNullOrWhiteSpace(Token))
                    // {
                    //     url = String.Format("http://{0}{1}/{2}/{3}.flv?vhost={4}&Token={5}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName, Token);
                    // }
                    // else if (!String.IsNullOrWhiteSpace(Ticket))
                    // {
                    //     url = String.Format("http://{0}{1}/{2}/{3}.flv?vhost={4}&Token={5}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName, Ticket);
                    // }
                    //  else
                    // {
                    url = String.Format("http://{0}{1}/{2}/{3}.flv?vhost={4}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName);
                    // }
                }
                return url;
            }
        }

        public String HttpsFlvDomainUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {
                    // if (!String.IsNullOrWhiteSpace(Token))
                    // {
                    //     url = String.Format("https://{0}{1}/{2}/{3}.flv?Token={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Token);
                    // }
                    // else if (!String.IsNullOrWhiteSpace(Ticket))
                    // {
                    //     url = String.Format("https://{0}{1}/{2}/{3}.flv?Ticket={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Ticket);
                    // }
                    //  else
                    // {
                    url = String.Format("https://{0}{1}/{2}/{3}.flv", Domain.DomainName, ZLServerHttpSSLPort == "80" ? "" : ":" + ZLServerHttpSSLPort, Application.App, StreamProxy.StreamName);
                    // }
                }
                return url;
            }
        }
        public String HttpsFlvUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {
                    // if (!String.IsNullOrWhiteSpace(Token))
                    // {
                    //     url = String.Format("https://{0}{1}/{2}/{3}.flv?vhost={4}&Token={5}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName, Token);
                    // }
                    // else if (!String.IsNullOrWhiteSpace(Ticket))
                    // {
                    //     url = String.Format("https://{0}{1}/{2}/{3}.flv?vhost={4}&Ticket={5}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName, Ticket);
                    // }
                    //  else
                    // {
                    url = String.Format("https://{0}{1}/{2}/{3}.flv?vhost={4}", ZLServerIp, ZLServerHttpSSLPort == "80" ? "" : ":" + ZLServerHttpSSLPort, Application.App, StreamProxy.StreamName, Domain.DomainName);
                    // }
                }
                return url;
            }
        }

        #endregion


        #region  Ws-flv地址

        public String WsFlvDomainUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {
                    // if (!String.IsNullOrWhiteSpace(Token))
                    // {
                    //     url = String.Format("ws://{0}{1}/{2}/{3}.flv?Token={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Token);
                    // }
                    // else if (!String.IsNullOrWhiteSpace(Ticket))
                    // {
                    //     url = String.Format("ws://{0}{1}/{2}/{3}.flv?Ticket={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Ticket);
                    // }
                    //  else
                    // {
                    url = String.Format("ws://{0}{1}/{2}/{3}.flv", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName);
                    // }
                }
                return url;
            }
        }
        public String WsFlvUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {
                    // if (!String.IsNullOrWhiteSpace(Token))
                    // {
                    //     url = String.Format("ws://{0}{1}/{2}/{3}.flv?vhost={4}&Token={5}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName,Token);
                    // }
                    // else if (!String.IsNullOrWhiteSpace(Ticket))
                    // {
                    //    url = String.Format("ws://{0}{1}/{2}/{3}.flv?vhost={4}&Ticket={5}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName,Ticket);
                    // }
                    //  else
                    // {
                    url = String.Format("ws://{0}{1}/{2}/{3}.flv?vhost={4}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName);
                    // }
                }
                return url;
            }
        }

        public String WssFlvDomainUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {
                    // if (!String.IsNullOrWhiteSpace(Token))
                    // {
                    //     url = String.Format("wss://{0}{1}/{2}/{3}.flv?Token={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName,Token);
                    // }
                    // else if (!String.IsNullOrWhiteSpace(Ticket))
                    // {
                    //     url = String.Format("wss://{0}{1}/{2}/{3}.flv?Ticket={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName,Ticket);
                    // }
                    //  else
                    // {
                    url = String.Format("wss://{0}{1}/{2}/{3}.flv", Domain.DomainName, ZLServerHttpSSLPort == "443" ? "" : ":" + ZLServerHttpSSLPort, Application.App, StreamProxy.StreamName);
                    // }
                }
                return url;
            }
        }
        public String WssFlvUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {
                    // if (!String.IsNullOrWhiteSpace(Token))
                    // {
                    //    url = String.Format("wss://{0}{1}/{2}/{3}.flv?vhost={4}&Token={5}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName,Token);
                    // }
                    // else if (!String.IsNullOrWhiteSpace(Ticket))
                    // {
                    //   url = String.Format("wss://{0}{1}/{2}/{3}.flv?vhost={4}&Ticket={5}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName,Ticket);
                    // }
                    //  else
                    // {
                    url = String.Format("wss://{0}{1}/{2}/{3}.flv?vhost={4}", ZLServerIp, ZLServerHttpSSLPort == "443" ? "" : ":" + ZLServerHttpSSLPort, Application.App, StreamProxy.StreamName, Domain.DomainName);
                    // }
                }
                return url;
            }
        }

        #endregion


        #region  Hls地址

        public String HlsHttpDomainUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {
                    // if (!String.IsNullOrWhiteSpace(Token))
                    // {
                    //     url = String.Format("http://{0}{1}/{2}/{3}/hls.m3u8?Token={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName,Token);
                    // }
                    // else if (!String.IsNullOrWhiteSpace(Ticket))
                    // {
                    //     url = String.Format("http://{0}{1}/{2}/{3}/hls.m3u8?Ticket={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName,Ticket);
                    // }
                    //  else
                    // {
                    url = String.Format("http://{0}{1}/{2}/{3}/hls.m3u8", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName);
                    // }
                }
                return url;
            }
        }
        public String HlsHttpUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {
                    // if (!String.IsNullOrWhiteSpace(Token))
                    // {
                    //     url = String.Format("http://{0}{1}/{2}/{3}/hls.m3u8?vhost={4}&Token={5}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName,Token);
                    // }
                    // else if (!String.IsNullOrWhiteSpace(Ticket))
                    // {
                    //    url = String.Format("http://{0}{1}/{2}/{3}/hls.m3u8?vhost={4}&Ticket={5}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName,Ticket);
                    // }
                    //  else
                    // {
                    url = String.Format("http://{0}{1}/{2}/{3}/hls.m3u8?vhost={4}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName);
                    // }
                }
                return url;
            }
        }

        public String HlsHttpsDomainUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {
                    // if (!String.IsNullOrWhiteSpace(Token))
                    // {
                    //     url = String.Format("https://{0}{1}/{2}/{3}/hls.m3u8?Token={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName,Token);
                    // }
                    // else if (!String.IsNullOrWhiteSpace(Ticket))
                    // {
                    //     url = String.Format("https://{0}{1}/{2}/{3}/hls.m3u8?Ticket={4}", Domain.DomainName, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName,Ticket);
                    // }
                    //  else
                    // {
                    url = String.Format("https://{0}{1}/{2}/{3}/hls.m3u8", Domain.DomainName, ZLServerHttpSSLPort == "443" ? "" : ":" + ZLServerHttpSSLPort, Application.App, StreamProxy.StreamName);
                    // }
                }
                return url;
            }
        }
        public String HlsHttpsUrl
        {
            get
            {
                var url = string.Empty;
                if (CheckData())
                {
                    // if (!String.IsNullOrWhiteSpace(Token))
                    // {
                    //    url = String.Format("https://{0}{1}/{2}/{3}/hls.m3u8?vhost={4}&Ticket={5}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName,Token);
                    // }
                    // else if (!String.IsNullOrWhiteSpace(Ticket))
                    // {
                    //   url = String.Format("https://{0}{1}/{2}/{3}/hls.m3u8?vhost={4}&Ticket={5}", ZLServerIp, ZLServerHttpPort == "80" ? "" : ":" + ZLServerHttpPort, Application.App, StreamProxy.StreamName, Domain.DomainName,Ticket);
                    // }
                    //  else
                    // {
                    url = String.Format("https://{0}{1}/{2}/{3}/hls.m3u8?vhost={4}", ZLServerIp, ZLServerHttpSSLPort == "443" ? "" : ":" + ZLServerHttpSSLPort, Application.App, StreamProxy.StreamName, Domain.DomainName);
                    // }
                }
                return url;
            }
        }

        #endregion

        private bool CheckData()
        {
            if (StreamProxy == null
                || StreamProxy.State != StreamProxyState.Normal
                || Application == null
                || Application.Status != ApplicationState.Normal
                 || Domain == null
                || Domain.Status != DomainState.Normal
                    )
            {
                return false;
            }
            return true;
        }

    }
}
