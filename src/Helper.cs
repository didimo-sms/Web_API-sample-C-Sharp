using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp;
using Newtonsoft.Json;

namespace WebAPI_Client
{
    public class Helper
    {
        public const String restService = "https://sms.didimo.es/wcf/Service.svc/rest";

        public const String urlService = "https://sms.didimo.es/sapi";

        public struct RestOperations
        {
            public const String Ping = "ping";
            public const String CreateMessage = "createmessage";
            public const String CreateSend = "createsend";
            public const String GetCredits = "getcredits";
            public const String GetMessageId = "getmessageid";
            public const String GetMessageStatus = "getmessagestatus";
            public const String CreateCertifiedMessage = "createcertifiedmessage";
            public const String CreateCertifiedSend = "createcertifiedsend";
            public const String GetCertifyFile = "getcertifyfile";
        }

        public struct UrlOperations
        {
            public const String Ping = "ping";
            public const String CreateMessage = "sms";
            public const String CreateCertifiedMessage = "certifiedsms";
            public const String GetCredits = "user/credits";
            public const String GetMessageStatus = "sms/status";
            public const String GetCertifyFile = "sms/certifyfile";
        }

        public class JsonHeader
        {
            public struct ContentType
            {
                public const String Name = "Content-Type";
                public const String Value = "application/json; charset=utf-8";
            }

            public struct Accept
            {
                public const String Name = "Accept";
                public const String Value = "application/json";
            }
        }

        public struct XmlHeader
        {
            public struct ContentType
            {
                public const String Name = "Content-Type";
                public const String Value = "application/xml; charset=utf-8";
            }

            public struct Accept
            {
                public const String Name = "Accept";
                public const String Value = "application/xml";
            }
        }
    }
}
