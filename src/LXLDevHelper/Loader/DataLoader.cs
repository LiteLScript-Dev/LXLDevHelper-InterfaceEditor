using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXLDevHelper.Loader
{
    public static class DataLoader
    {
        public static void LoadFromPath(string root, ref ViewModels.MainContentViewModel Data)
        {
            Data.DirCollection.Clear();
            foreach (var dir in Directory.GetDirectories(root))
            {
                var dirInfo = new ViewModels.LXLDirectory() { DirName = Path.GetFileName(dir) };
                foreach (var file in Directory.GetFiles(dir))
                {
                    try
                    {
                        var raw = File.ReadAllText(file);
                        dirInfo.AllClass.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<ViewModels.LXLClass>(raw));
                    }
                    catch (System.Exception ex)
                    {
                        try
                        {
                            Tools.Message.ShowWarn($"读取{file}时遇到错误\n{ex}");
                        }
                        catch (Exception) { Console.WriteLine($"读取{file}时遇到错误\n{ex}"); }
                    }
                }
                Data.DirCollection.Add(dirInfo);
            }
        }
    }
}
