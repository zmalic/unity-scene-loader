using System;

namespace SLoader
{
    public class SceneLoadException : Exception
    {
        public SceneLoadException()
        {
        }

        public SceneLoadException(string message) : base(message)
        {
        }

        public SceneLoadException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}