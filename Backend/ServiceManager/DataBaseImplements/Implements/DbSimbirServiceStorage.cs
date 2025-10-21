using Contracts.BindingModels;
using Contracts.StorageContracts;
using DataBaseImplement.DbModels;

namespace DataBaseImplement.Implements {
    public class DbSimbirServiceStorage : ISimbirServiceStorage {
        public void InsertDbServiceInfo(in SimbirServiceBindingModel insertModel)
        {
            if (insertModel == null) { throw new ArgumentNullException(nameof(insertModel)); }
            DbSimbirService newRec = DbSimbirService.Insert(insertModel);
            using DataBase context = new DataBase();
            context.SimbirServices.Add(newRec);
            context.SaveChanges();
        }
        public void UpdateDbServiceInfo(in SimbirServiceBindingModel updateModel)
        {
            if (updateModel == null) { throw new ArgumentNullException(nameof(updateModel)); }
            using var context = new DataBase();
            int updateModelId = updateModel.Id;
            DbSimbirService updateRec = context.SimbirServices.First(x => x.Id == updateModelId);
            updateRec.Update(updateModel);
            context.SaveChanges(); 
        }
        public void DeleteDbServiceInfo(int deleteModelId)
        {
            using var context = new DataBase();
            DbSimbirService deleteRec = context.SimbirServices.First(x =>x.Id == deleteModelId);
            context.SimbirServices.Remove(deleteRec);
            context.SaveChanges();
        }

        public void GetServiceDbInfo(out List<SimbirServiceBindingModel> recordList)
        {
            using var context = new DataBase();
            recordList = context.SimbirServices.Select(x => (SimbirServiceBindingModel)x).ToList();
        }
        public void GetServiceDbInfo(out SimbirServiceBindingModel record, int serviceId)
        {
            using var context = new DataBase();
            record = (SimbirServiceBindingModel)context.SimbirServices.First(x => x.Id == serviceId);
        }
    }
}