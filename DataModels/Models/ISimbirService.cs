using System.Net;

namespace DataModels.Models {
    public interface ISimbirService : IId {
        string ServiceName { get; }
        IPEndPoint EndPointService { get; }
    }
}
