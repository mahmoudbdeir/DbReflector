using System;

namespace DbReflector
{
    public static class Secrets
    {
        public static readonly string AdventureWorksCs = Environment.GetEnvironmentVariable("AdventureWorksCs");
    }
}