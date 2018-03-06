using System;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;

namespace IsDatSteve.Interfaces
{
    public interface IBruleDetector
    {
       Tuple<string, string> GetBrulesThoughts(MediaFile file);
    }
}
