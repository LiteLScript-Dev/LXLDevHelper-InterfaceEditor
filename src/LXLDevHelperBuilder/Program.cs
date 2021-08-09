using System;
using LXLDevHelper.ViewModels;
namespace LXLDevHelperBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            string root = @"C:\Users\gaoxi\Documents\GitHub\LXLDevHelper-DocSrc\src";
            string tsout = @"C:\Users\gaoxi\Documents\GitHub\LXLDevHelper-DocSrc\tsout";
            if (args.Length > 0) { root = args[0]; }
            if (args.Length > 1) { root = args[1]; }
            var data = new LXLDevHelper.ViewModels.MainContentViewModel();
            LXLDevHelper.Loader.DataLoader.LoadFromPath(root, ref data);
            #region TS
            var tsbuildser = new TsBuilder(data);
            tsbuildser.Build(tsout);
            #endregion
        }
    }
}
