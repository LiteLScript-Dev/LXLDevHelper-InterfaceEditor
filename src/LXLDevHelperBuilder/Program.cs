using System;
using LXLDevHelper.ViewModels;
namespace LXLDevHelperBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            string root = @"C:\Users\gaoxi\Documents\GitHub\LXLDevHelper-DocSrc\src";
            string tsout = @"C:\Users\gaoxi\Documents\GitHub\LXLDevHelper-DocSrc\tsout";
            string rawout = @"C:\Users\gaoxi\Documents\GitHub\LXLDevHelper-DocSrc\rawout\raw.json";
            if (args.Length > 0) { root = args[0]; }
            if (args.Length > 1) { root = args[1]; }
            if (args.Length > 2) { root = args[2]; }
#else
            if (args.Length < 3)
            {
                Console.WriteLine("参数不足。");
                return;
            }
            string root = System.IO.Path.GetFullPath(args[0]);
            string rawout = System.IO.Path.GetFullPath(args[1]);
            string tsout = System.IO.Path.GetFullPath(args[2]);
#endif
            Console.WriteLine("raw=>" + rawout);
            Console.WriteLine("ts=>" + tsout);
            var data = new MainContentViewModel();
            LXLDevHelper.Loader.DataLoader.LoadFromPath(root, ref data);
            #region Raw
            var rawbuildser = new RawBuilder(data);
            rawbuildser.Build(rawout);
            #endregion
            #region TS
            var tsbuildser = new TsBuilder(data);
            tsbuildser.Build(tsout);
            #endregion
        }
    }
}
