using System;
namespace SLoader
{
    [Serializable]
    // Serializable object for getting information from the web server
    public class UrlResponse
    {
        public bool success;
        public TipList results;
    }
}
