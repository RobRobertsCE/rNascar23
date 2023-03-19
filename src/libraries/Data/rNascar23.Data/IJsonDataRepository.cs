using System;

namespace rNascar23.Data
{
    public interface IJsonDataRepository
    {
        string Url { get; }

        string Get(string url);
    }
}
