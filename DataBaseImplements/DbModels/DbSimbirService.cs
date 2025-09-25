using Contracts.BindingModels;
using DataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;

namespace DataBaseImplement.DbModels {
    public class DbSimbirService : ISimbirService {
        public string ServiceName { get; set; }
        public IPEndPoint EndPointService { get; set; }

        public int Id { get; set; }

        public DbSimbirService(in SimbirServiceBindingModel model)
        {
            ServiceName = model.ServiceName;
            EndPointService = model.EndPointService;
            Id = model.Id;
        }

        public void Update(in SimbirServiceBindingModel editModel)
        {
            ServiceName = editModel.ServiceName;
            EndPointService = editModel.EndPointService;
            Id = editModel.Id;
        }

        public static explicit operator SimbirServiceBindingModel? (DbSimbirService? DbModel)
        {
            if (DbModel == null) {
                return null;
            }
            return new SimbirServiceBindingModel {
                ServiceName = DbModel.ServiceName,
                EndPointService = DbModel.EndPointService,
                Id = DbModel.Id
            };
        }
    }
}
