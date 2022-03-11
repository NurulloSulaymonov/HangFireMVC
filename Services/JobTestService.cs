using System.Diagnostics;

namespace HangfireMVC.Services
{
    public class JobTestService : IJobTestService
    {
        public void FireAndForgetJob()
        {
            Debug.WriteLine("Hello from a Fire and Forget job!");
        }
        public void ReccuringJob()
        {
            Debug.WriteLine("Hello from a Scheduled job!");
        }
        public void DelayedJob()
        {
            Debug.WriteLine("Hello from a Delayed job!");
        }
        public void ContinuationJob()
        {
            Debug.WriteLine("Hello from a Continuation job!");
        }
    }
}
