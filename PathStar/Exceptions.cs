using System;

namespace YonatanMankovich.PathStar
{
    public class PathNotFoundException : Exception
    {
        public PathNotFoundException() : this("") { }

        public PathNotFoundException(string msg) : base(msg + "Path to destination point was not found.") { }
    }
}