using System.Threading;

namespace Application.Features.Implementation.Common
{
    public static class DbLock
    {
        public static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);
    }
}