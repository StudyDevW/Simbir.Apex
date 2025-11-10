using Contracts.BindingModels;
using Contracts.StorageContracts;
using DataBaseImplement.DbModels;


namespace DataBaseImplement.Implements {
    public class DbSimbirServiceStorage : ISimbirServiceStorage {
        private readonly DataBase context;
        public DbSimbirServiceStorage(IConfiguration conf) 
        {
            context = new DataBase(conf["DATABASE_CONNECT_M"]);
        }
        public void InsertDbServiceInfo(in SimbirServiceBindingModel insertModel)
        {
            if (insertModel == null) { throw new ArgumentNullException(nameof(insertModel)); }
            DbSimbirService newRec = DbSimbirService.Insert(insertModel);
            context.SimbirServices.Add(newRec);
            context.SaveChanges();
        }
        public void UpdateDbServiceInfo(in SimbirServiceBindingModel updateModel)
        {
            if (updateModel == null) { throw new ArgumentNullException(nameof(updateModel)); }
            int updateModelId = updateModel.Id;
            DbSimbirService updateRec = context.SimbirServices.First(x => x.Id == updateModelId);
            updateRec.Update(updateModel);
            context.SaveChanges(); 
        }
        public void DeleteDbServiceInfo(int deleteModelId)
        {
            DbSimbirService deleteRec = context.SimbirServices.First(x =>x.Id == deleteModelId);
            context.SimbirServices.Remove(deleteRec);
            context.SaveChanges();
        }

        public void GetServiceDbInfo(out List<SimbirServiceBindingModel> recordList)
        {
            recordList = context.SimbirServices.Select(x => (SimbirServiceBindingModel)x).ToList();
        }
        public void GetServiceDbInfo(out SimbirServiceBindingModel record, int serviceId)
        {
            record = (SimbirServiceBindingModel)context.SimbirServices.First(x => x.Id == serviceId);
        }
        public void GetServiceDbInfo(out SimbirServiceBindingModel record, string serviceName)
        {
            record = (SimbirServiceBindingModel)context.SimbirServices.First(x => x.ServiceName == serviceName);
        }
    }
}