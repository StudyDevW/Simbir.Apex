using Microsoft.AspNetCore.Mvc;

namespace ServiceUI.Interfaces {
    public interface IServiceManager {
        public Task ServiceInit();
    }
}
