
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json.Linq;
using Yanyitec.Svc.Meta;

namespace Yanyitec.Svc
{
    public class ApiServiceFactory
    {
        public ApiServiceFactory(JObject config,string embededSvcPath=null) {
            this.Config = config;
            this._Services = new Dictionary<string, IApiService>();
            this._EmbededServices = new Dictionary<string, IApiService>();
            this.EmbededServicePath = embededSvcPath;
        }

        public string EmbededServicePath { get; private set; }

        void LoadServices() {
            foreach (var pair in this.Config) {
                var svcId = pair.Key;
                var svcInfo = pair.Value as JObject;
                
                var type = svcInfo.Value<ServiceTypes>("ServiceType");
                
            }
        }
        IApiService LoadServiceAgent(string id, string basUrl, JObject svcInfo) {
            throw new NotImplementedException();
        }





        IApiService LoadEmbededService(string id,string basUrl,JObject svcInfo) {
            var des = svcInfo.Value<string>("Description");
            
            var assemblyPath= System.IO.Path.Combine(this.EmbededServicePath,basUrl);
            var smbl = System.Reflection.Assembly.LoadFile(assemblyPath);
            var types = smbl.GetExportedTypes();
            var apiTypes = new List<Type>();
            foreach (var type in types) {
                if (!type.IsClass || type.IsAbstract) continue;
                var attr = type.GetCustomAttributes(false).FirstOrDefault(p=>p.GetType()== typeof(ApiSvcControllerAttribute));
                if (attr == null) continue;
                apiTypes.Add(type);
            }

            if (apiTypes.Count == 0)
            {
                //TO LOG
                return null;
            }
            else {
                var api = new ApiService()
                {
                    Description = svcInfo.Value<string>("Description"),
                    BasUrl = basUrl,
                    ControllerTypes = apiTypes,
                    ServiceType = ServiceTypes.Embeded,
                    Status = OnlineStates.Online
                };
                api.SetAssignedId(id);
                return api;
            }
        }

        public JObject Config { get; private set; }

        private Dictionary<string, IApiService> _Services;

        private Dictionary<string, IApiService> _EmbededServices;

        public List<ServiceMeta> GetMetas() {
            var result = new List<ServiceMeta>();
            foreach (var pair in this._EmbededServices) {
                result.Add(pair.Value.GetServiceMeta());
            }
            return result;
        }

        
    }
}
