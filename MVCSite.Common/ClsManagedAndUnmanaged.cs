using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCSite.Common
{
    class ClsManagedAndUnmanaged : IDisposable
    {
        //Keep track if all resources are already freed.
        private bool ResourcesAreFreed = false;

        //Used to explicitly call Dispose() to free both managed & unmanaged resources
        public void Dispose()
        {
            FreeResources(ifFreeManagedResource:true);
        }

        //IN CASE forgot to explicitly call Dispose() method,
        //let system to call this destructor to free any unmanaged resources,
        //as Garbage collector will help release any managed resources.
        ~ClsManagedAndUnmanaged()
        {
            FreeResources(ifFreeManagedResource: false);
        }

        //The system will convert above destructor into an override version of the Finalize method
        //protected override void Finalize()
        //{
        //    try
        //    {
        //        //FreeResources(ifFreeManagedResource: false);
        //    }
        //    finally
        //    {
        //        base.Finalize();
        //    }
        //}
        private void FreeResources(bool ifFreeManagedResource)
        {
            if (!ResourcesAreFreed)
            {
                
                if (ifFreeManagedResource)
                {
                    //Free Managed Resources here
                }

                //Free Unmanaged Resources here

                ResourcesAreFreed = true;

                //At this step, even unmanaged resources are released. Managed resources are released as well.
                //Therefore, there's no need to run the destructor before an instance of this class is destroyed
                GC.SuppressFinalize(this);
            }
        }
    }

}
