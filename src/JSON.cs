using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp;
using Newtonsoft.Json;

namespace WebAPI_Client
{

    public class JSON
    {

        public JSON() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        private String Execute(String operation, String jsonData)
        {
            // Initialize client
            var client = new RestClient(Helper.restService);

            // Initialize Request
            var request = new RestRequest(operation);

            // Set Request Format => Json
            request.RequestFormat = DataFormat.Json;

            // Set Request Method => POST
            request.Method = Method.POST;

            // Request Header
            request.AddHeader(Helper.JsonHeader.ContentType.Name, Helper.JsonHeader.ContentType.Value);
            request.AddHeader(Helper.JsonHeader.Accept.Name, Helper.JsonHeader.Accept.Value);

            // Set POST Data 
            request.AddParameter(Helper.JsonHeader.Accept.Value, jsonData, ParameterType.RequestBody);

            // Execute the Request
            var response = client.Execute(request);

            // Check HTTP Status Code
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return JsonConvert.SerializeObject(String.Format("{0} - {1}", (int)response.StatusCode, response.StatusDescription), Formatting.None);

            // Return the result
            return response.Content;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        private byte[] ExecuteAndDownload(String operation, String jsonData, ref String fileName)
        {
            // Initialize client
            var client = new RestClient(Helper.restService);

            // Initialize Request
            var request = new RestRequest(operation);

            // Set Request Format => Json
            request.RequestFormat = DataFormat.Json;

            // Set Request Method => POST
            request.Method = Method.POST;

            // Request Header
            request.AddHeader(Helper.JsonHeader.ContentType.Name, Helper.JsonHeader.ContentType.Value);
            request.AddHeader(Helper.JsonHeader.Accept.Name, Helper.JsonHeader.Accept.Value);

            // Set POST Data 
            request.AddParameter(Helper.JsonHeader.Accept.Value, jsonData, ParameterType.RequestBody);

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

            // Json Data
            var jsonData = JsonConvert.SerializeObject(postData, Formatting.None);

            // Execute
            return this.Execute(Helper.RestOperations.Ping, jsonData);
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

            // Json Data
            var jsonData = JsonConvert.SerializeObject(postData, Formatting.None);

            // Execute
            return this.Execute(Helper.RestOperations.GetMessageId, jsonData);
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
            Collection<object> messages = new Collection<object>(); // Required

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

            messages.Add(new { 
                Id = id,
                IsUnicode = isUnicode,
                Mobile = mobile,
                Text = text
            });

            // SMS 2 - Unicode
            id = Guid.NewGuid().ToString();
            mobile = "+34653695842";
            text = String.Format("測試API sms.didimo.es，由C#客戶端 {0} - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), id);
            isUnicode = true;

            messages.Add(new
            {
                Id = id,
                IsUnicode = isUnicode,
                Mobile = mobile,
                Text = text
            });

            // POST Data
            var postData = new
            {
                UserName = userName,
                Password = password,
                Name = name,
                ScheduleDate = scheduleDate,
                Sender = sender,
                Messages = messages
            };

            // Json Data
            var jsonData = JsonConvert.SerializeObject(postData, Formatting.None);

            // Execute
            return this.Execute(Helper.RestOperations.CreateSend, jsonData);
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

            // Json Data
            var jsonData = JsonConvert.SerializeObject(postData, Formatting.None);

            // Execute
            return this.Execute(Helper.RestOperations.CreateMessage, jsonData);
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

            String id = "80d975cb-e2b5-4963-a751-2de427fa7b55";

            // POST Data
            var postData = new
            {
                UserName = userName,
                Password = password,
                Id = id.ToUpper()
            };

            // Json Data
            var jsonData = JsonConvert.SerializeObject(postData, Formatting.None);

            // Execute
            return this.Execute(Helper.RestOperations.GetMessageStatus, jsonData);
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

            // Json Data
            var jsonData = JsonConvert.SerializeObject(postData, Formatting.None);

            // Execute
            return this.Execute(Helper.RestOperations.GetCredits, jsonData);
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
            Collection<object> messages = new Collection<object>(); // Required

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

            messages.Add(new
            {
                Id = id,
                IsUnicode = isUnicode,
                Mobile = mobile,
                Text = text
            });

            // SMS 2 - Unicode
            id = Guid.NewGuid().ToString();
            mobile = "+34653695842";
            text = String.Format("測試API SMS Certified sms.didimo.es，由C#客戶端 {0} - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), id);
            isUnicode = true;

            messages.Add(new
            {
                Id = id,
                IsUnicode = isUnicode,
                Mobile = mobile,
                Text = text
            });

            // POST Data
            var postData = new
            {
                UserName = userName,
                Password = password,
                Name = name,
                ScheduleDate = scheduleDate,
                Sender = sender,
                Messages = messages
            };

            // Json Data
            var jsonData = JsonConvert.SerializeObject(postData, Formatting.None);

            // Execute
            return this.Execute(Helper.RestOperations.CreateCertifiedSend, jsonData);
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

            // Json Data
            var jsonData = JsonConvert.SerializeObject(postData, Formatting.None);

            // Execute
            return this.Execute(Helper.RestOperations.CreateCertifiedMessage, jsonData);
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

            // Json Data
            var jsonData = JsonConvert.SerializeObject(postData, Formatting.None);

            byte[] buffer;
            String fileName = String.Format("message_{0}.pdf", id); ;

            try
            {
                // Execute
                buffer = this.ExecuteAndDownload(Helper.RestOperations.GetCertifyFile, jsonData, ref fileName);
            }
            catch(Exception ex)
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
