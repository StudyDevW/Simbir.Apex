using Contracts.BindingModels;
using DataModels.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace DataBaseImplement.DbModels {
    public class DbSimbirService : ISimbirService {
        public string ServiceName { get ; set; } = string.Empty;

        public string EndPointServiceStr { get; set; } = string.Empty;

        private IPEndPoint? _endPointService;
        [NotMapped]
        public IPEndPoint EndPointService {
            get {
                if (_endPointService == null) { _endPointService = IPEndPoint.Parse(EndPointServiceStr); }
                return _endPointService;
            } 
            set {
                EndPointServiceStr = value.ToString();
                _endPointService = value; 
            } 
        }

        public int Id { get; set; }

        public static DbSimbirService Insert(in SimbirServiceBindingModel model)
        {
            return new DbSimbirService() {
                ServiceName = model.ServiceName,
                EndPointService = model.EndPointService,
                Id = model.Id
            };
        }

        public void Update(in SimbirServiceBindingModel editModel)
        {
            ServiceName = editModel.ServiceName;
            EndPointService = editModel.EndPointService;
            Id = editModel.Id;
        }

        public static explicit operator SimbirServiceBindingModel (DbSimbirService DbModel)
        {
            return new SimbirServiceBindingModel {
                ServiceName = DbModel.ServiceName,
                EndPointService = DbModel.EndPointService,
                Id = DbModel.Id
            };
        }
    }
}
