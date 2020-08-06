using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformRestore.Services
{
    public interface IBlobService
    {
        public System.Threading.Tasks.Task<bool> ListBlobs();
    }
}
