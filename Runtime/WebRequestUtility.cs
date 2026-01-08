using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Net.Http;

namespace qb.Utility
{
    /// <summary>
    /// Provides utility methods for performing web requests, handling JSON deserialization, and checking file existence
    /// over HTTP.
    /// </summary>
    public static class WebRequestUtility
    {
        public class DataWebRequestResult<T>
        {
            public readonly bool success;
            public readonly string error;
            public readonly long responseCode;
            public readonly T data;
            public DataWebRequestResult(bool success, string error, long responseCode,T data)
            {
                this.success = success;
                this.error = error;
                this.responseCode = responseCode;
                this.data = data;
            }
            public DataWebRequestResult(string error, long responseCode)
            {
                this.error = error;
                this.responseCode = responseCode;
                success = false;
                data = default(T);
            }
            public string FormatedError => !success?$"[{responseCode}]\n{error}":"";
        }
        public static async Task<DataWebRequestResult<T>> RequestData<T>(string url, Action<float> onProgress = null, params string[] headerParameters)
        {
            try
            {
                using (UnityWebRequest uwr = UnityWebRequest.Get(new Uri(url)))
                {
                    if (headerParameters != null)
                    {
                        for (int i = 0; i < headerParameters.Length; i += 2)
                        {
                            uwr.SetRequestHeader(headerParameters[i], headerParameters[i + 1]);
                        }

                    }
                    var op = uwr.SendWebRequest();
                    while (!op.isDone)
                    {
                        onProgress?.Invoke(op.progress);
                        await Task.Yield();
                    }
                    
                    return new DataWebRequestResult<T>(uwr.result == UnityWebRequest.Result.Success, uwr.error, uwr.responseCode, UserializeFromJsonString<T>(uwr.downloadHandler.text));
                }
            }
            catch (Exception e)
            {
                return new DataWebRequestResult<T>(false, e.Message, -1, default(T));
            }

        }
        public static async Task<DataWebRequestResult<string>> RequestString(string url,Action<float> onProgress = null,params string[] headerParameters)
        {
            try
            {
                using (UnityWebRequest uwr = UnityWebRequest.Get(new Uri(url)))
                {
                    if (headerParameters != null)
                    {
                        for (int i = 0; i < headerParameters.Length; i+=2)
                        {
                            uwr.SetRequestHeader(headerParameters[i], headerParameters[i+1]);
                        }
                       
                    }
                    var op = uwr.SendWebRequest();
                    while (!op.isDone)
                    {
                        onProgress?.Invoke(op.progress);
                        await Task.Yield();
                    }
                    return new DataWebRequestResult<string>(uwr.result == UnityWebRequest.Result.Success, uwr.error, uwr.responseCode, uwr.downloadHandler.text);
                }
            }
            catch (Exception e)
            {
                return new DataWebRequestResult<string>(false, e.Message, -1, null);
            }
        }


        public class IgnoreUnexpectedArraysConverter<T> : IgnoreUnexpectedArraysConverterBase
        {
            public override bool CanConvert(Type objectType)
            {
                return typeof(T).IsAssignableFrom(objectType);
            }
        }
        #region json unserialization
        public class IgnoreUnexpectedArraysConverter : IgnoreUnexpectedArraysConverterBase
        {
            readonly IContractResolver resolver;

            public IgnoreUnexpectedArraysConverter(IContractResolver resolver)
            {
                if (resolver == null)
                    throw new ArgumentNullException();
                this.resolver = resolver;
            }

            public override bool CanConvert(Type objectType)
            {
                if (objectType.IsPrimitive || objectType == typeof(string))
                    return false;
                return resolver.ResolveContract(objectType) is JsonObjectContract;
            }
        }

        public abstract class IgnoreUnexpectedArraysConverterBase : JsonConverter
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var contract = serializer.ContractResolver.ResolveContract(objectType);
                if (!(contract is JsonObjectContract))
                {
                    throw new JsonSerializationException(string.Format("{0} is not a JSON object", objectType));
                }

                do
                {
                    if (reader.TokenType == JsonToken.Null)
                        return null;
                    else if (reader.TokenType == JsonToken.Comment)
                        continue;
                    else if (reader.TokenType == JsonToken.StartArray)
                    {
                        var array = JArray.Load(reader);
                        if (array.Count > 0)
                            throw new JsonSerializationException(string.Format("Array was not empty."));
                        return null;//existingValue ?? contract.DefaultCreator();
                    }
                    else if (reader.TokenType == JsonToken.StartObject)
                    {
                        // Prevent infinite recursion by using Populate()
                        existingValue = existingValue ?? contract.DefaultCreator();
                        serializer.Populate(reader, existingValue);
                        return existingValue;
                    }
                    else
                    {
                        throw new JsonSerializationException(string.Format("Unexpected token {0}", reader.TokenType));
                    }
                }
                while (reader.Read());
                throw new JsonSerializationException("Unexpected end of JSON.");
            }

            public override bool CanWrite { get { return false; } }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
        
        public static T UserializeFromJsonString<T>(string jsonString) 
        {
            var defaultJsonContratResolver = new DefaultContractResolver();
            var StaticJsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                Converters = { new IgnoreUnexpectedArraysConverter(defaultJsonContratResolver) },
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
            };
            return JsonConvert.DeserializeObject<T>(jsonString, StaticJsonSerializerSettings);
        }


        public static async Task<bool> CheckFileExists(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
                return response.IsSuccessStatusCode;
            }
        }

        #endregion
    }
}
