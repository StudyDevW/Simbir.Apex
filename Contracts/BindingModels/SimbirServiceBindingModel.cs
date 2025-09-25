using DataModels.Models;
using System.Net;

namespace Contracts.BindingModels {
    public class SimbirServiceBindingModel : ISimbirService {
        public required string ServiceName { get; set; }
        public required IPEndPoint EndPointService { get; set; }

        public required int Id { get; set; }
    }
}
