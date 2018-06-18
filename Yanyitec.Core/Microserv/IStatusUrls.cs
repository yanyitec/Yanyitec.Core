namespace Yanyitec.Svc
{
    public interface IStatusUrls
    {
        string BasUrl { get; }
        string HeartbeatUrl { get; }
        string MetaUrl { get; }
        string OnstartUrl { get; }
        string OnstopUrl { get; }
    }
}