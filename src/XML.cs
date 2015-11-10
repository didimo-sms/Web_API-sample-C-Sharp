using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp;

namespace WebAPI_Client
{
    public class XML
    {
        public XML() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        private String Execute(String operation, String xmlData)
        {
            // Initialize client
            var client = new RestClient(Helper.restService);

            // Initialize Request
            var request = new RestRequest(operation);

            // Set Request Format => Xml
            request.RequestFormat = DataFormat.Xml;

            // Set Request Method => POST
            request.Method = Method.POST;

            // Request Header
            request.AddHeader(Helper.XmlHeader.ContentType.Name, Helper.XmlHeader.ContentType.Value);
            request.AddHeader(Helper.XmlHeader.Accept.Name, Helper.XmlHeader.Accept.Value);

            // Set POST Data 
            request.AddParameter(Helper.XmlHeader.Accept.Value, xmlData, ParameterType.RequestBody);

            // Execute the Request
            var response = client.Execute(request);

            // Check HTTP Status Code
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return String.Format("{0} - {1}", (int)response.StatusCode, response.StatusDescription);

            // Return the result
            return response.Content;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        private byte[] ExecuteAndDownload(String operation, String xmlData, ref String fileName)
        {
            // Initialize client
            var client = new RestClient(Helper.restService);

            // Initialize Request
            var request = new RestRequest(operation);

            // Set Request Format => Json
            request.RequestFormat = DataFormat.Xml;

            // Set Request Method => POST
            request.Method = Method.POST;

            // Request Header
            request.AddHeader(Helper.XmlHeader.ContentType.Name, Helper.XmlHeader.ContentType.Value);
            request.AddHeader(Helper.XmlHeader.Accept.Name, Helper.XmlHeader.Accept.Value);

            // Set POST Data 
            request.AddParameter(Helper.XmlHeader.Accept.Value, xmlData, ParameterType.RequestBody);

            // Execute the Request
            var response = client.Execute(request);

            // Check HTTP Status Code
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(String.Format("{0} - {1} - {2}", (int)response.StatusCode, response.StatusDescription, response.Content));

            // Get file name from header response
            string cdValue = response.Headers.ToList()
                    .Find(x => x.Name.ToLower() == "content-disposition")
                    .Value.ToString();
            String filenameHolder = "filename=";

            fileName = cdValue.Substring(cdValue.IndexOf(filenameHolder) + filenameHolder.Length).Replace("\"", "");

            // Return the result
            return response.RawBytes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String Ping()
        {
            // User Data
            String userName = "email@domain.com";
            String password = "password";

            // POST Data
            var postData = new { 
                    UserName = userName,
                    Password = password
            };

            // Xml Data
            var xmlData = String.Format(@"
                <PingRequest xmlns='https://sms.didimo.es/wcf/PingRequest'>
                    <UserName>{0}</UserName>
                    <Password>{1}</Password>
                </PingRequest>
                ", postData.UserName, postData.Password);

            // Execute
            return this.Execute(Helper.RestOperations.Ping, xmlData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String GetMessageId()
        {
            // User Data
            String userName = "email@domain.com";
            String password = "password";

            // POST Data
            var postData = new
            {
                UserName = userName,
                Password = password
            };

            // Xml Data
            var xmlData = String.Format(@"
                <GetMessageIdRequest xmlns='https://sms.didimo.es/wcf/GetMessageIdRequest'>
                    <UserName>{0}</UserName>
                    <Password>{1}</Password>
                    </GetMessageIdRequest>
                ", postData.UserName, postData.Password);

            // Execute
            return this.Execute(Helper.RestOperations.GetMessageId, xmlData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String CreateSend()
        {
            // User Data
            String userName = "email@domain.com";
            String password = "password";

            // Schedule Data
            String name = String.Format("Test Web API - C# Client - {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); //Optional
            String scheduleDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"); //Optional
            String sender = "didimo"; //Optional

            // Messages Data
            String id; //Optional
            String mobile; //Required
            String text; //Required
            Boolean isUnicode; //Optional - Values: 'true' or 'false'. Default value: 'false'

            // SMS 1 - GSM7
            id = Guid.NewGuid().ToString();
            mobile = "+34653695688";
            text = String.Format("Test API sms.didimo.es, by C# client {0} - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), id);
            isUnicode = false;

            var message1 = new { 
                Id = id,
                IsUnicode = isUnicode,
                Mobile = mobile,
                Text = text
            };

            // SMS 2 - Unicode
            id = Guid.NewGuid().ToString();
            mobile = "+34653695842";
            text = String.Format("測試API sms.didimo.es，由C#客戶端 {0} - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), id);
            isUnicode = true;

            var message2 = new
            {
                Id = id,
                IsUnicode = isUnicode,
                Mobile = mobile,
                Text = text
            };

            // POST Data
            var postData = new
            {
                UserName = userName,
                Password = password,
                Name = name,
                ScheduleDate = scheduleDate,
                Sender = sender,
                Messages = new[] { message1, message2 }
            };

            String xmlMessages = String.Empty;
            
            foreach (var message in postData.Messages)
            {
                xmlMessages += String.Format(@"
                    <MessageSend xmlns='https://sms.didimo.es/wcf/CreateSendRequest/MessageSend'>
                      <Id>{0}</Id> 
                      <IsUnicode>{1}</IsUnicode>
                      <Mobile>{2}</Mobile>
                      <Text>{3}</Text>
                    </MessageSend>
                ", message.Id, message.IsUnicode, message.Mobile, message.Text);
            }

            // Xml Data
            var xmlData = String.Format(@"
                <CreateSendRequest xmlns='https://sms.didimo.es/wcf/CreateSendRequest'>
                  <UserName>{0}</UserName>
                  <Password>{1}</Password>
                  <Name>{2}</Name>
                  <ScheduleDate>{3}</ScheduleDate>
                  <Sender>{4}</Sender>
                  <Messages>
                    {5}
                  </Messages>
                </CreateSendRequest>
            ", postData.UserName, postData.Password, postData.Name, postData.ScheduleDate, postData.Sender, xmlMessages);

            // Execute
            return this.Execute(Helper.RestOperations.CreateSend, xmlData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String CreateMessage()
        {
            // User Data
            String userName = "email@domain.com";
            String password = "password";

            // SMS Data
            String name = String.Format("Test Web API - C# Client - {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); //Optional
            String scheduleDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"); //Optional
            String sender = "didimo"; //Optional
            String id = Guid.NewGuid().ToString(); //Optional
            String mobile = "+34653695688"; //Required
            String text = String.Format("Test API sms.didimo.es, by C# client {0} - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), id); //Required
            Boolean isUnicode = false; //Optional - Values: 'true' or 'false'. Default value: 'false'
            
            // POST Data
            var postData = new
            {
                UserName = userName,
                Password = password,
                Name = name,
                ScheduleDate = scheduleDate,
                Sender = sender,
                Id = id,
                IsUnicode = isUnicode,
                Mobile = mobile,
                Text = text
            };

            // Xml Data
            var xmlData = String.Format(@"
                <CreateMessageRequest xmlns='https://sms.didimo.es/wcf/CreateMessageRequest'>
                  <UserName>{0}</UserName>
                  <Password>{1}</Password>
                  <Id>{2}</Id>
                  <Name>{3}</Name>
                  <Sender>{4}</Sender>
                  <Text>{5}</Text>
                  <Mobile>{6}</Mobile>
                  <ScheduleDate>{7}</ScheduleDate>
                  <IsUnicode>{8}</IsUnicode>
                </CreateMessageRequest>
            ", postData.UserName, postData.Password, postData.Id, postData.Name, postData.Sender, postData.Text, postData.Mobile, postData.ScheduleDate, postData.IsUnicode);

            // Execute
            return this.Execute(Helper.RestOperations.CreateMessage, xmlData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String GetMessageStatus()
        {
            // User Data
            String userName = "email@domain.com";
            String password = "password";

            String id = "3c618414-7d80-46d0-b5c2-c5d5ee3f8d8c";

            // POST Data
            var postData = new
            {
                UserName = userName,
                Password = password,
                Id = id.ToUpper()
            };

            // Xml Data
            var xmlData = String.Format(@"
                <GetMessageStatusRequest xmlns='https://sms.didimo.es/wcf/GetMessageStatusRequest'>
                  <UserName>{0}</UserName>
                  <Password>{1}</Password>
                  <Id>{2}</Id>
                </GetMessageStatusRequest>
            ", postData.UserName, postData.Password, postData.Id);

            // Execute
            return this.Execute(Helper.RestOperations.GetMessageStatus, xmlData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String GetCredits()
        {
            // User Data
            String userName = "email@domain.com";
            String password = "password";

            // POST Data
            var postData = new
            {
                UserName = userName,
                Password = password
            };

            // Xml Data
            var xmlData = String.Format(@"
                <GetCreditsRequest xmlns='https://sms.didimo.es/wcf/GetCreditsRequest'>
                  <UserName>{0}</UserName>
                  <Password>{1}</Password>
                </GetCreditsRequest>
            ", postData.UserName, postData.Password);

            // Execute
            return this.Execute(Helper.RestOperations.GetCredits, xmlData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String CreateCertifiedSend()
        {
            // User Data
            String userName = "email@domain.com";
            String password = "password";

            // Schedule Data
            String name = String.Format("Test Web API SMS Certified - C# Client - {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); //Optional
            String scheduleDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"); //Optional
            String sender = "didimo"; //Optional

            // Messages Data
            String id; //Optional
            String mobile; //Required
            String text; //Required
            Boolean isUnicode; //Optional - Values: 'true' or 'false'. Default value: 'false'

            // SMS 1 - GSM7
            id = Guid.NewGuid().ToString();
            mobile = "+34653695688";
            text = String.Format("Test API SMS Certified sms.didimo.es, by C# client {0} - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), id);
            isUnicode = false;

            var message1 = new
            {
                Id = id,
                IsUnicode = isUnicode,
                Mobile = mobile,
                Text = text
            };

            // SMS 2 - Unicode
            id = Guid.NewGuid().ToString();
            mobile = "+34653695842";
            text = String.Format("測試API SMS Certified sms.didimo.es，由C#客戶端 {0} - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), id);
            isUnicode = true;

            var message2 = new
            {
                Id = id,
                IsUnicode = isUnicode,
                Mobile = mobile,
                Text = text
            };

            // POST Data
            var postData = new
            {
                UserName = userName,
                Password = password,
                Name = name,
                ScheduleDate = scheduleDate,
                Sender = sender,
                Messages = new[] { message1, message2 }
            };

            String xmlMessages = String.Empty;

            foreach (var message in postData.Messages)
            {
                xmlMessages += String.Format(@"
                    <MessageSend xmlns='https://sms.didimo.es/wcf/CreateSendRequest/MessageSend'>
                      <Id>{0}</Id> 
                      <IsUnicode>{1}</IsUnicode>
                      <Mobile>{2}</Mobile>
                      <Text>{3}</Text>
                    </MessageSend>
                ", message.Id, message.IsUnicode, message.Mobile, message.Text);
            }

            // Xml Data
            var xmlData = String.Format(@"
                <CreateSendRequest xmlns='https://sms.didimo.es/wcf/CreateSendRequest'>
                  <UserName>{0}</UserName>
                  <Password>{1}</Password>
                  <Name>{2}</Name>
                  <ScheduleDate>{3}</ScheduleDate>
                  <Sender>{4}</Sender>
                  <Messages>
                    {5}
                  </Messages>
                </CreateSendRequest>
            ", postData.UserName, postData.Password, postData.Name, postData.ScheduleDate, postData.Sender, xmlMessages);

            // Execute
            return this.Execute(Helper.RestOperations.CreateCertifiedSend, xmlData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String CreateCertifiedMessage()
        {
            // User Data
            String userName = "email@domain.com";
            String password = "password";

            // SMS Data
            String name = String.Format("Test Web API SMS Certified - C# Client - {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); //Optional
            String scheduleDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"); //Optional
            String sender = "didimo"; //Optional
            String id = Guid.NewGuid().ToString(); //Optional
            String mobile = "+34653695688"; //Required
            String text = String.Format("Test API SMS Certified sms.didimo.es, by C# client {0} - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), id); //Required
            Boolean isUnicode = false; //Optional - Values: 'true' or 'false'. Default value: 'false'

            // POST Data
            var postData = new
            {
                UserName = userName,
                Password = password,
                Name = name,
                ScheduleDate = scheduleDate,
                Sender = sender,
                Id = id,
                IsUnicode = isUnicode,
                Mobile = mobile,
                Text = text
            };

            // Xml Data
            var xmlData = String.Format(@"
                <CreateMessageRequest xmlns='https://sms.didimo.es/wcf/CreateMessageRequest'>
                  <UserName>{0}</UserName>
                  <Password>{1}</Password>
                  <Id>{2}</Id>
                  <Name>{3}</Name>
                  <Sender>{4}</Sender>
                  <Text>{5}</Text>
                  <Mobile>{6}</Mobile>
                  <ScheduleDate>{7}</ScheduleDate>
                  <IsUnicode>{8}</IsUnicode>
                </CreateMessageRequest>
            ", postData.UserName, postData.Password, postData.Id, postData.Name, postData.Sender, postData.Text, postData.Mobile, postData.ScheduleDate, postData.IsUnicode);

            // Execute
            return this.Execute(Helper.RestOperations.CreateCertifiedMessage, xmlData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String GetCertifyFile()
        {
            String result = String.Empty;

            // User Data
            String userName = "email@domain.com";
            String password = "password";

            String id = "1244eaaa-9cbe-434a-a3eb-762fa8be865f";

            // POST Data
            var postData = new
            {
                UserName = userName,
                Password = password,
                Id = id.ToUpper()
            };

            // Xml Data
            var xmlData = String.Format(@"
                <GetCertifyFileRequest xmlns='https://sms.didimo.es/wcf/GetCertifyFileRequest'>
                  <UserName>{0}</UserName>
                  <Password>{1}</Password>
                  <Id>{2}</Id>
                </GetCertifyFileRequest>
            ", postData.UserName, postData.Password, postData.Id);

            byte[] buffer;
            String fileName = String.Format("message_{0}.pdf", id); ;

            try
            {
                // Execute
                buffer = this.ExecuteAndDownload(Helper.RestOperations.GetCertifyFile, xmlData, ref fileName);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            String fileDirectory = @"C:\SMSCertifies\sms.didimo\";

            using (System.IO.FileStream fs = new System.IO.FileStream(System.IO.Path.Combine(fileDirectory, fileName), System.IO.FileMode.OpenOrCreate))
            {
                fs.WriteAsync(buffer, 0, buffer.Length).Wait();
            }

            result = String.Format("File saved on {0}", System.IO.Path.Combine(fileDirectory, fileName));

            return result;
        }
    }
}
