using System.Threading;

namespace UserCreator.Core.Providers
{
    internal class IdentityProvider
    {
        private int _nextId = 0;

        internal int GetNext()
        {
            return Interlocked.Increment(ref _nextId);
        }

        public void SetId(int id)
        {
            _nextId = id;
        }
    }
}