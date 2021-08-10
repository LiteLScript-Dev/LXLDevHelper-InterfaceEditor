using LXLDevHelper.ViewModels;
using System.Collections.Generic;
using System.IO;

namespace LXLDevHelperBuilder
{
    internal class TsBuilder : BuilderBase
    {
        internal TsBuilder(MainContentViewModel _data) : base(_data) { }
        internal override void Build(string targetPath)
        {
            //删除旧的文件
            if (Directory.Exists(targetPath))
            {
                Directory.Delete(targetPath, true);
                WriteLine("已清除旧文件。");
            }
            //创建目录
            Directory.CreateDirectory(targetPath);
            WriteLine("创建目录成功。" + targetPath);
            #region 写入
            foreach (var dirInfo in Data.DirCollection)
            {
                var dir = Path.Combine(targetPath, dirInfo.DirName);
                Directory.CreateDirectory(dir);
                WriteLine("创建目录成功。" + dir);
                foreach (var cla in dirInfo.AllClass)
                {
                    var sb = new System.Text.StringBuilder();
                    List<string> addedRef = new List<string>();
                    if (!string.IsNullOrWhiteSpace(cla.Description))
                    {
                        sb.AppendLine("/**");
                        foreach (string line in cla.Description.Replace("\r", "").Split('\n'))
                        { sb.AppendLine(" * " + line); }
                        sb.AppendLine(" */");
                    }
                    sb.AppendLine($"declare class {cla.ClassName} {{");//类起始位置
                    void AppendLine(string str) => sb.AppendLine("\t" + str);
                    string DecodeType(string raw)//转换类型
                    {
                        var sp = raw.Split('|');
                        for (int i = 0; i < sp.Length; i++)
                        {
                            string v = sp[i].Trim();
                            if (v.StartsWith("Array@"))
                            {
                                sp[i] = v.Substring(6) + "[]";
                            }
                            else if (v.StartsWith("Function@"))
                            {
                                var funcraw = v.Substring(9);
                             
                                sp[i] = "(any)=>any";
                            }
                            else
                            {
                                switch (v)
                                {
                                    case "Null": sp[i] = "null"; break;
                                    case "Integer": sp[i] = "number"; break;
                                    case "Float": sp[i] = "number"; break;
                                    case "String": sp[i] = "string"; break;
                                    case "Boolean": sp[i] = "boolean"; break;
                                    case "Array": sp[i] = "Array<any>"; break;
                                    case "Object": sp[i] = "any"; break;
                                    case "ByteBuffer": sp[i] = "ArrayBuffer"; break;
                                    default:
                                        var notFound = true;
                                        foreach (var classTemp in dirInfo.AllClass)
                                        {
                                            var name = classTemp.ClassName.Trim();
                                            //if (name == "IntPos")
                                            //{
                                            //    WriteLine("test");
                                            //    WriteLine("test" + addedRef.Contains(name)+ (name == v));
                                            //    WriteLine("test" + name  );
                                            //    WriteLine("test" + v);
                                            //    WriteLine("test:" + string.Join(",", addedRef));
                                            //}
                                            if (addedRef.Contains(v)) { notFound = false; break; }
                                            else if (name == v)    //寻找位置并添加引用
                                            {
                                                sb.Insert(0, $"/// <reference path=\"{v}.d.ts\" /> " + System.Environment.NewLine);
                                                addedRef.Add(v);
                                                WriteLine($"添加类{v}引用到类{cla.ClassName}成功！");
                                                notFound = false; break;
                                            }
                                        }
                                        if (notFound)
                                        {
                                            WriteLine($"未找到类型{v}的定义！");
                                            sp[i] = "any";
                                        }
                                        break;
                                }
                            }
                        }
                        return string.Join("|", sp);
                    }
                    foreach (var func in cla.AllFunc)
                    {
                        AppendLine("/**");
                        if (!string.IsNullOrWhiteSpace(func.Description))
                        {
                            foreach (string line in func.Description.Replace("\r", "").Split('\n'))
                            {
                                AppendLine(" * " + line);
                            }
                        }
                        var paramsList = new System.Collections.Generic.List<string>();
                        foreach (var p in func.Params)
                        {
                            var pd = DecodeType(p.ParamType);
                            paramsList.Add($"{p.ParamName}{(p.Optional ? "?" : "")}:{pd}");
                            bool firstLine = true;
                            foreach (string line in p.Description.Replace("\r", "").Split('\n'))
                            {
                                if (firstLine)
                                {
                                    firstLine = false;
                                    AppendLine($" * @param {p.ParamName} {line}");
                                }
                                else
                                {
                                    AppendLine(" * " + line);
                                }
                            }
                        }
                        var returnType = DecodeType(func.ReturnType);
                        if (string.IsNullOrWhiteSpace(returnType)) { returnType = "void"; }
                        if (returnType!="void")
                        {
                            bool firstLine = true;
                            foreach (string line in func.ReturnDescription.Replace("\r", "").Split('\n'))
                            {
                                if (firstLine)
                                {
                                    firstLine = false;
                                    AppendLine($" * @returns {line}");
                                }
                                else
                                {
                                    AppendLine(" * " + line);
                                }
                            }
                        }
                        AppendLine(" */");
                        AppendLine($"{(func.IsStatic ? "static " : "")}{func.FuncName}({string.Join(",", paramsList)}):{returnType}");
                    }
                    sb.AppendLine("}");//类  结束
                    File.WriteAllText(Path.Combine(dir, cla.ClassName + ".d.ts"), sb.ToString());
                }
            }
            #endregion
            //入口文件
            var indexPath = Path.Combine(targetPath, "index.d.ts");
        }
        internal void WriteLine(object x)
        {
            System.Console.WriteLine(x);
        }
    }
}
