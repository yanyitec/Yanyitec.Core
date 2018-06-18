using Yanyitec.Svc.Meta;

namespace Yanyitec.Svc
{

    public interface IApiService
    {
        string BasUrl { get;  }
        string Description { get; }

        OnlineStates Status { get; }

        ServiceTypes ServiceType { get; }
        IStatusUrls StatusUrls { get;  }

        ServiceMeta GetServiceMeta();
    }
}