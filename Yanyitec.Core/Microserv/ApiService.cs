using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Yanyitec.Svc.Meta;
using System.Reflection;

namespace Yanyitec.Svc
{
    public class ApiService : Entity<string>, IApiService
    {

        public string Description { get; set; }

        public string BasUrl { get; set; }

        public ServiceTypes ServiceType { get; set; }

        public OnlineStates Status { get; set; }

        StatusUrls _StatusUrls;
        public IStatusUrls StatusUrls {
            get {
                if (this._StatusUrls == null) {
                    lock (this) {
                        if (this._StatusUrls == null)
                        {
                            
                            this._StatusUrls = new StatusUrls();
                        }
                    }
                }
                return _StatusUrls;
            } set {
                _StatusUrls = value as StatusUrls;
            }
        }

        public IReadOnlyDictionary<string, IReadOnlyList<string>> Actions { get; set; }

        public List<Type> ControllerTypes { get; set; }

        public virtual T GetInterface<T>(string controllerName) where T : class
        {
            var typename = controllerName + "Controller";
            var type = this.ControllerTypes.FirstOrDefault(p=>p.Name == typename || p.Name == controllerName);
            return Activator.CreateInstance(type) as T;
        }

        public ServiceMeta GetServiceMeta() {
            var svc = new ServiceMeta();
            var controllers = svc.Controllers = new List<ControllerMeta>();
            foreach (var type in this.ControllerTypes) {
                controllers.Add(GetControllerMeta(type));
            }
            return svc;
        }

        ControllerMeta GetControllerMeta(Type type) {
            var controllerMeta = new ControllerMeta();
            #region name
            var controllerName = type.Name;
            var attr = type.GetCustomAttributes(false).FirstOrDefault(p=>p.GetType()==typeof(ApiSvcControllerAttribute)) as ApiSvcControllerAttribute;
            
            if (attr != null && !string.IsNullOrWhiteSpace(attr.ControllerName)) {
                controllerName = attr.ControllerName.Trim();
            }
            if (controllerName.EndsWith("Controller")) {
                controllerName = controllerName.Substring(0,controllerName.Length-"Controller".Length);
            }
            if (controllerName == string.Empty) controllerName = type.Name;
            controllerMeta.Name = controllerName;
            #endregion

            var actions = controllerMeta.Actions = new List<ActionMeta>();

            var methods = type.GetMethods();
            foreach (var method in methods) {
                var actionMeta = this.GetActionMeta(method);
                if (actionMeta == null) continue;
                actions.Add(actionMeta);
            }


            return controllerMeta;
        }

        ActionMeta GetActionMeta(MethodInfo method) {
            var actionMeta = new ActionMeta();
            #region name
            var actionName = method.Name;
            var attr = method.GetCustomAttributes(false).FirstOrDefault(p => p.GetType() == typeof(ApiSvcActionAttribute)) as ApiSvcActionAttribute;
            if (attr == null) return null;
            if (!string.IsNullOrWhiteSpace(attr.ActionName))
            {
                actionName = attr.ActionName.Trim();
            }
            
            if (actionName == string.Empty) actionName = method.Name;
            actionMeta.Name = actionName;
            #endregion

            #region params
            var pars = actionMeta.Parameters = new List<ParameterMeta>();
            foreach (var paramInfo in method.GetParameters()) {
                pars.Add(this.GetParameterMeta(paramInfo));
            }
            #endregion

            actionMeta.Return = this.GetParameterMeta(method.ReturnParameter);
            return actionMeta;
        }

        ParameterMeta GetParameterMeta(ParameterInfo param) {
            var paramMeta = new ParameterMeta();
            paramMeta.ParameterName = param.Name;
            paramMeta.ParameterType = param.ParameterType.FullName;
            if (param.HasDefaultValue) {
                paramMeta.DefaultValue = param.DefaultValue == null ? null : param.DefaultValue.ToString();
            }
            
            return paramMeta;
        }
    }
}
