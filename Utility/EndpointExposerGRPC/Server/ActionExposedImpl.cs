using ActionExposedGRPC;
using EndpointGRPC.Exposed;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EndpointGRPC.Server
{
    public class ActionExposedImpl: ActionExposedServiceGRPC.ActionExposedServiceGRPCBase 
    {
        readonly string _apiBaseName;
        readonly Assembly _assembly;
        readonly Type _iFeature;
        public ActionExposedImpl(string ApiBaseName, Assembly assembly, Type iFeature)
        {
            _apiBaseName = ApiBaseName;
            _assembly = assembly;
            _iFeature = iFeature;
        }

        public override async Task<ApiResponseTemplate> GetActions(RequestGetActions request, ServerCallContext context)
        {
            ApiResponseTemplate response = new ApiResponseTemplate();
            response.IsApiHandled = true;

            ResponseGetActions data = new ResponseGetActions();
            data.ApiName = _apiBaseName;
            response.StatusCode = 200;
            response.IsRequestSuccess = true;
            try
            {
                var routes = ExposedAllEndpoints.GetAllActions(_apiBaseName, _assembly, _iFeature);
                data.Routes.AddRange(routes);
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.IsApiHandled = false;
                response.IsRequestSuccess = false;
                response.StatusCode = 500;
                response.Exceptions.Add(ex.Message);
                //TODO: Loging here
               
            }






            return response; 
        }
    }
}
