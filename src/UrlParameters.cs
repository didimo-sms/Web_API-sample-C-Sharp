using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

using RestSharp;

namespace WebAPI_Client
{
    public class UrlParameters
    {

        public UrlParameters() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private String Execute(String operation, NameValueCollection parameters)
        {
            // Initialize client
            var client = new RestClient(Helper.urlService);

            // Initialize Request
            var request = new RestRequest(operation);

            // Set Request Method => POST
            request.Method = Method.POST;

            // Request Header
            request.AddHeader(Helper.XmlHeader.ContentType.Name, Helper.XmlHeader.ContentType.Value);
            request.AddHeader(Helper.JsonHeader.Accept.Name, Helper.JsonHeader.Accept.Value);

            // Set Parameters in QueryString
            foreach (String key in parameters.AllKeys)
                request.AddParameter(key, parameters[key], ParameterType.QueryString);

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
        private byte[] ExecuteAndDownload(String operation, NameValueCollection parameters, ref String fileName)
        {
            // Initialize client
            var client = new RestClient(Helper.urlService);

            // Initialize Request
            var request = new RestRequest(operation);

            // Set Request Method => GET
            request.Method = Method.GET;

            // Request Header
            request.AddHeader(Helper.XmlHeader.ContentType.Name, Helper.XmlHeader.ContentType.Value);
            request.AddHeader(Helper.JsonHeader.Accept.Name, Helper.JsonHeader.Accept.Value);

            // Set Parameters in QueryString
            foreach (String key in parameters.AllKeys)
                request.AddParameter(key, parameters[key], ParameterType.QueryString);

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
            var postData = new
            {
                user = userName,
                password = password
            };

            NameValueCollection parameters = new NameValueCollection();
            postData.GetType().GetProperties()
                    .ToList()
                    .ForEach(pi => parameters.Add(pi.Name, pi.GetValue(postData, null).ToString()));

            // Execute
            return this.Execute(Helper.UrlOperations.Ping, parameters);
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
                user = userName,
                password = password
            };

            NameValueCollection parameters = new NameValueCollection();
            postData.GetType().GetProperties()
                    .ToList()
                    .ForEach(pi => parameters.Add(pi.Name, pi.GetValue(postData, null).ToString()));

            // Execute
            return this.Execute(Helper.UrlOperations.GetCredits, parameters);
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
            String sender = "didimo"; //Optional
            String id = Guid.NewGuid().ToString(); //Optional
            String mobile = "+34653695688"; //Required
            String text = String.Format("Test API sms.didimo.es, by C# client {0} - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), id); //Required
            Boolean isUnicode = false; //Optional - Values: 'true' or 'false'. Default value: 'false'

            // POST Data
            var postData = new
            {
                user = userName,
                password = password,
                sender = sender,
                mobile = mobile,
                text = text,
                id = id,
                isUnicode = isUnicode,
            };

            // Json Data
            NameValueCollection parameters = new NameValueCollection();
            postData.GetType().GetProperties()
                    .ToList()
                    .ForEach(pi => parameters.Add(pi.Name, pi.GetValue(postData, null).ToString()));

            // Execute
            return this.Execute(Helper.UrlOperations.CreateMessage, parameters);
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
            String sender = "didimo"; //Optional
            String id = Guid.NewGuid().ToString(); //Optional
            String mobile = "+34653695688"; //Required
            String text = String.Format("Test API SMS Certified sms.didimo.es, by C# client {0} - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), id); //Required
            Boolean isUnicode = false; //Optional - Values: 'true' or 'false'. Default value: 'false'

            // POST Data
            var postData = new
            {
                user = userName,
                password = password,
                sender = sender,
                mobile = mobile,
                text = text,
                id = id,
                isUnicode = isUnicode,
            };

            // Json Data
            NameValueCollection parameters = new NameValueCollection();
            postData.GetType().GetProperties()
                    .ToList()
                    .ForEach(pi => parameters.Add(pi.Name, pi.GetValue(postData, null).ToString()));

            // Execute
            return this.Execute(Helper.UrlOperations.CreateCertifiedMessage, parameters);
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

            String id = "da275e31-7999-4b7b-8994-dfb7c13bc8e6";

            // POST Data
            var postData = new
            {
                user = userName,
                password = password,
                id= id.ToUpper()
            };

            NameValueCollection parameters = new NameValueCollection();
            postData.GetType().GetProperties()
                    .ToList()
                    .ForEach(pi => parameters.Add(pi.Name, pi.GetValue(postData, null).ToString()));

            // Execute
            return this.Execute(Helper.UrlOperations.GetMessageStatus, parameters);
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
                user = userName,
                password = password,
                id = id.ToUpper()
            };

            NameValueCollection parameters = new NameValueCollection();
            postData.GetType().GetProperties()
                    .ToList()
                    .ForEach(pi => parameters.Add(pi.Name, pi.GetValue(postData, null).ToString()));

            byte[] buffer;
            String fileName = String.Format("message_{0}.pdf", id); ;

            try
            {
                // Execute
                buffer = this.ExecuteAndDownload(Helper.UrlOperations.GetCertifyFile, parameters, ref fileName);
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
