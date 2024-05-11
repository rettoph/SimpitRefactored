namespace KerbalSimpit.Core.Utilities
{
    public static class Unmanaged
    {
        public static unsafe bool Equals<T>(in T x, in T y)
            where T : unmanaged
        {
            fixed (T* px = &x)
            fixed (T* py = &y)
            {
                var p1 = (byte*)px;
                var p2 = (byte*)py;

                for (var i = 0; i < sizeof(T); i++)
                {
                    if (p1[i] != p2[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
