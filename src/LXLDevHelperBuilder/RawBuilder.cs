using LXLDevHelper.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXLDevHelperBuilder
{
    internal class RawBuilder : BuilderBase
    {
        internal RawBuilder(MainContentViewModel _data) : base(_data) { }

        internal override void Build(string targetPath)
        {
            var dir = Path.GetDirectoryName(targetPath);
            if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
            File.WriteAllText(targetPath, Newtonsoft.Json.JsonConvert.SerializeObject(Data,Newtonsoft.Json.Formatting.Indented));
        }
    }
}
