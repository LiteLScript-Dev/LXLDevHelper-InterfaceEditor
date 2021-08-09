using LXLDevHelper.ViewModels;
using System.IO;

namespace LXLDevHelperBuilder
{
    internal abstract class BuilderBase
    {
        internal readonly MainContentViewModel Data;
        internal BuilderBase(MainContentViewModel _data)
        {
            Data = _data;
        }
        abstract internal void Build(string targetPath);
    }
}
