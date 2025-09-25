using Contracts.BindingModels;
using Contracts.StorageContracts;
using DataBaseImplement.DbModels;

namespace DataBaseImplement.Implements {
    public class DbSimbirServiceStorage : ISimbirServiceStorage {
        public bool InsertDbServiceInfo(in SimbirServiceBindingModel insertModel)
        {
            DbSimbirService newRec = new DbSimbirService(insertModel);
            using DataBase context = new DataBase();
            try {
                context.SimbirServices.Add(newRec);
                context.SaveChanges();
            }
            catch { return false; }
            return true;
        }
        public bool UpdateDbServiceInfo(in SimbirServiceBindingModel updateModel)
        {
            using var context = new DataBase();
            int updateModelId = updateModel.Id;
            DbSimbirService updateRec = context.SimbirServices.First(x => x.Id == updateModelId);
            updateRec.Update(updateModel);
            try { context.SaveChanges(); }
            catch { return false; }
            return true;
        }
        public bool DeleteDbServiceInfo(int deleteModelId)
        {
            using var context = new DataBase();
            DbSimbirService deleteRec = context.SimbirServices.First(x =>x.Id == deleteModelId);
            try {
                context.SimbirServices.Remove(deleteRec);
                context.SaveChanges();
            }
            catch { return false; }
            return true;
        }

        public void GetServiceDbInfo(out List<SimbirServiceBindingModel?> recordList)
        {
            using var context = new DataBase();
            recordList = context.SimbirServices.Select(x => (SimbirServiceBindingModel?)x).ToList();
        }
        public void GetServiceDbInfo(out SimbirServiceBindingModel? record, int serviceId)
        {
            using var context = new DataBase();
            record = (SimbirServiceBindingModel?)context.SimbirServices.FirstOrDefault(x => x.Id == serviceId);
        }
    }
}