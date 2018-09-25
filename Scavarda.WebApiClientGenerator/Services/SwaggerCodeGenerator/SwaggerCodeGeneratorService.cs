using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Scavarda.WebApiClientGenerator.Services.Abstractions.WebApiClientGenerator;

namespace Scavarda.WebApiClientGenerator.Services.SwaggerCodeGenerator
{
    public class SwaggerCodeGeneratorService : IWebApiClientGeneratorService
    {
        private const string ConfigOptionsFilename = "config-swagger.json";
        private readonly SwaggerCodeGeneratorServiceOptions _options;

        /// <summary>
        /// SwaggerCodeGeneratorService constructor
        /// </summary>
        public SwaggerCodeGeneratorService()
        {
        }

        /// <summary>
        /// SwaggerCodeGeneratorService constructor
        /// </summary>
        public SwaggerCodeGeneratorService(SwaggerCodeGeneratorServiceOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Build package from source code
        /// </summary>
        /// <param name="inputFile">OpenApi Document (swagger.json) full path</param>
        /// <param name="outputDir">Target directory for compiled package (nupkg, etc..)</param>
        /// <param name="language">Client implementation language</param>
        /// <param name="packageName">Package name</param>
        /// <param name="packageVersion">Package version</param>
        public FileInfo BuildPackage(string inputFile, string outputDir, WebApiClientGeneratorLanguage language, string packageName, string packageVersion = null)
        {
            GenerateClientSourceCode(inputFile, outputDir, language, packageName, packageVersion);

            BuildPackage(language, outputDir, packageName, outputDir);

            return null;
        }

        /// <summary>
        /// Build package from source code
        /// </summary>
        /// <param name="language">Client implementation language</param>
        /// <param name="sourceCodeDir">Source code directory</param>
        /// <param name="packageName">Package name</param>
        /// <param name="outputDir">Target directory for compiled package (nupkg, etc..)</param>
        public FileInfo BuildPackage(WebApiClientGeneratorLanguage language, string sourceCodeDir, string packageName, string outputDir)
        {
            switch (language)
            {
                case WebApiClientGeneratorLanguage.DotNet4:
                    {
                        Directory.CreateDirectory(outputDir);
                        string targetDll = Path.Combine(outputDir, packageName + ".dll");

                        string data = File.ReadAllText(Path.Combine(sourceCodeDir, "build.bat"));
                        data = data.Replace(@"Microsoft.NET\Framework\v3.5", @"Microsoft.NET\Framework\v4.0.30319");
                        data = data.Replace($"/out:bin\\{packageName}.dll", $"/out:\"{targetDll}\"");
                        File.WriteAllText(Path.Combine(sourceCodeDir, "build.bat"), data);

                        Process process = new Process()
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                WorkingDirectory = Path.Combine(sourceCodeDir),
                                FileName = Path.Combine(sourceCodeDir, "build.bat"),
                            }
                        };
                        process.Start();
                        process.WaitForExit();

                        return new FileInfo(targetDll);
                    }
                case WebApiClientGeneratorLanguage.DotNet45:
                    {
                        Directory.CreateDirectory(outputDir);
                        string targetDll = Path.Combine(outputDir, packageName + ".dll");

                        string data = File.ReadAllText(Path.Combine(sourceCodeDir, "build.bat"));
                        data = data.Replace($"/out:bin\\{packageName}.dll", $"/out:\"{targetDll}\"");
                        File.WriteAllText(Path.Combine(sourceCodeDir, "build.bat"), data);

                        Process process = new Process()
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                WorkingDirectory = Path.Combine(sourceCodeDir),
                                FileName = Path.Combine(sourceCodeDir, "build.bat"),
                            }
                        };
                        process.Start();
                        process.WaitForExit();

                        return new FileInfo(targetDll);
                    }
                case WebApiClientGeneratorLanguage.NetCoreStandard2:
                    string projectDir = Path.Combine(sourceCodeDir, "src", packageName);

                    var arguments = new List<string> {
                        $"pack",
                        $"-o \"{outputDir}\"",
                    };

                    Process p = new Process()
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            WorkingDirectory = projectDir,
                            FileName = _options.DotNetCommandPath,
                            Arguments = string.Join(" ", arguments),
                        }
                    };

                    Console.WriteLine(_options.DotNetCommandPath + " " + string.Join(" ", arguments));

                    p.Start();
                    p.WaitForExit();
                    break;
                case WebApiClientGeneratorLanguage.Php:
                    //{
                    //    string src = Path.Combine(sourceCodeDir, "SwaggerClient-php");
                    //    //Planet.Core.IO.Directory.RecursiveDelete(output, true);
                    //    Directory.Move(src, outputDir);
                    //}
                    break;
                case WebApiClientGeneratorLanguage.Java:
                    break;
                default:
                    break;
            }

            //Planet.Core.IO.Directory.RecursiveDelete(tempCodeFolder, true);
            return null;
        }

        /// <summary>
        /// Generate client source code
        /// </summary>
        /// <param name="inputFile">OpenApi Document (swagger.json) full path</param>
        /// <param name="outputDir">Target directory for generated source code<param>
        /// <param name="language">Client implementation language</param>
        /// <param name="packageName">Package name</param>
        /// <param name="packageVersion">Package version</param>
        public void GenerateClientSourceCode(string inputFile, string outputDir, WebApiClientGeneratorLanguage language, string packageName, string packageVersion = null)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string tempCodeFolder = outputDir ?? Path.Combine(currentDirectory, "temp");
            string jarPath = Path.Combine(currentDirectory, _options.SwaggerCodeGeneratorJarPath);

            Directory.CreateDirectory(tempCodeFolder);

            #region Generate code for client
            var languageValue = GetLanguage(language);
            var configOptions = GetConfigOptions(language, packageName, packageVersion);
            var extraOptions = GetExtraOptions(language);

            var arguments = new List<string> {
                $"-jar \"{jarPath}\" generate",
                $"-i \"{inputFile}\"",
                $"-o \"{tempCodeFolder}\"",
                $"-l {languageValue}",
            };

            if (configOptions != null)
            {
                string configOptionsData = JsonConvert.SerializeObject(configOptions);
                string configOptionsPath = Path.Combine(currentDirectory, ConfigOptionsFilename);
                File.WriteAllText(configOptionsPath, configOptionsData);
                arguments.Add($"-c \"{configOptionsPath}\"");
            }

            if (extraOptions != string.Empty)
            {
                arguments.Add(extraOptions);
            }

            Process p = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = currentDirectory,
                    FileName = _options.JavaCommandPath,
                    Arguments = string.Join(" ", arguments),
                }
            };

            Console.WriteLine(_options.JavaCommandPath + " " + string.Join(" ", arguments));

            p.Start();
            p.WaitForExit();

            if (configOptions != null)
            {
                File.Delete(Path.Combine(currentDirectory, ConfigOptionsFilename));
            }

            switch (language)
            {
                case WebApiClientGeneratorLanguage.NetCoreStandard2:
                    FixSourceCodeForNetCore2(outputDir, packageName);
                    break;
            }
            #endregion
        }

        private SwaggerCodeGeneratorConfigOptions? GetConfigOptions(WebApiClientGeneratorLanguage language, string packageName, string packageVersion = null)
        {
            Nullable<SwaggerCodeGeneratorConfigOptions> configOptions = null;

            switch (language)
            {
                case WebApiClientGeneratorLanguage.DotNet4:
                    configOptions = new SwaggerCodeGeneratorConfigOptions
                    {
                        PackageName = packageName,
                        TargetFramework = "v3.5",
                    };
                    break;
                case WebApiClientGeneratorLanguage.DotNet45:
                    configOptions = new SwaggerCodeGeneratorConfigOptions
                    {
                        PackageName = packageName,
                        ModelPropertyNaming = "PascalCase",
                        TargetFramework = "4.5",
                    };
                    break;
                case WebApiClientGeneratorLanguage.NetCoreStandard2:
                    configOptions = new SwaggerCodeGeneratorConfigOptions
                    {
                        ModelPropertyNaming = "PascalCase",
                        NetCoreProjectFile = true,
                        PackageName = packageName,
                        PackageVersion = packageVersion,
                        TargetFramework = "5.0",
                    };
                    break;
                case WebApiClientGeneratorLanguage.TypescriptAngular:
                    configOptions = new SwaggerCodeGeneratorConfigOptions
                    {
                        PackageName = packageName,
                    };
                    break;
                case WebApiClientGeneratorLanguage.Php:
                    configOptions = new SwaggerCodeGeneratorConfigOptions
                    {
                        GitUserId = "planet",
                        GitRepoId = "php-tallorno-data-api-client",
                        InvokerPackage = packageName.Replace(".", "\\"),
                    };
                    break;
                case WebApiClientGeneratorLanguage.Java:
                    break;
                default:
                    break;
            }

            return configOptions;
        }

        private string GetExtraOptions(WebApiClientGeneratorLanguage language)
        {
            string extraOptions = string.Empty;

            switch (language)
            {
                case WebApiClientGeneratorLanguage.DotNet4:
                case WebApiClientGeneratorLanguage.DotNet45:
                case WebApiClientGeneratorLanguage.NetCoreStandard2:
                    break;
                case WebApiClientGeneratorLanguage.Php:
                    break;
                case WebApiClientGeneratorLanguage.Java:
                    break;
                case WebApiClientGeneratorLanguage.TypescriptAngular:
                    break;
                default:
                    break;
            }
            return extraOptions;
        }

        private string GetLanguage(WebApiClientGeneratorLanguage language)
        {
            string languageValue = string.Empty;

            switch (language)
            {
                case WebApiClientGeneratorLanguage.DotNet4:
                case WebApiClientGeneratorLanguage.DotNet45:
                case WebApiClientGeneratorLanguage.NetCoreStandard2:
                    languageValue = "csharp";
                    break;
                case WebApiClientGeneratorLanguage.Php:
                    languageValue = "php";
                    break;
                case WebApiClientGeneratorLanguage.Java:
                    languageValue = "java";
                    break;
                case WebApiClientGeneratorLanguage.TypescriptAngular:
                    languageValue = "typescript-angular";
                    break;
                default:
                    languageValue = "all";
                    break;
            }
            return languageValue;
        }

        private void FixSourceCodeForNetCore2(string outputDir, string packageName)
        {
            #region CSProject
            string csprojFilePath = Path.Combine(outputDir, "src", packageName, packageName + ".csproj");

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(csprojFilePath);

            var node = xmlDocument.DocumentElement.SelectSingleNode("PropertyGroup/TargetFramework");
            if (node != null)
            {
                if (node.InnerText == "net45")
                    node.InnerText = "netstandard2.0";
            }

            var restSharpPackageReferenceNode = xmlDocument.DocumentElement.SelectSingleNode("ItemGroup/PackageReference[@Include='RestSharp']");
            if (restSharpPackageReferenceNode != null)
            {
                XmlNode itemGroupNode = restSharpPackageReferenceNode.ParentNode;

                restSharpPackageReferenceNode.Attributes["Version"].Value = "106.4.1";

                var newtonsoftNode = itemGroupNode.SelectSingleNode("PackageReference[@Include='Newtonsoft.Json']");
                if (newtonsoftNode != null)
                    newtonsoftNode.Attributes["Version"].Value = "11.0.2";

                var jsonSubTypesNode = itemGroupNode.SelectSingleNode("PackageReference[@Include='JsonSubTypes']");
                if (jsonSubTypesNode != null)
                    jsonSubTypesNode.Attributes["Version"].Value = "1.5.0";


                var msCSharpNode = xmlDocument.CreateElement("PackageReference");
                msCSharpNode.SetAttribute("Include", "Microsoft.CSharp");
                msCSharpNode.SetAttribute("Version", "4.5.0");
                itemGroupNode.AppendChild(msCSharpNode);

                var cmpModelAnnotationsNode = xmlDocument.CreateElement("PackageReference");
                cmpModelAnnotationsNode.SetAttribute("Include", "System.ComponentModel.Annotations");
                cmpModelAnnotationsNode.SetAttribute("Version", "4.5.0");
                itemGroupNode.AppendChild(cmpModelAnnotationsNode);
            }

            var secondItemGroupNode = xmlDocument.DocumentElement.SelectSingleNode("ItemGroup/Reference[@Include='System']")?.ParentNode;
            if (secondItemGroupNode != null)
            {
                xmlDocument.DocumentElement.RemoveChild(secondItemGroupNode);
            }

            xmlDocument.Save(csprojFilePath);
            #endregion

            #region Client\ApiClient.cs
            string csFilePath = Path.Combine(outputDir, "src", packageName, "Client", "ApiClient.cs");
            string csSourceCode = File.ReadAllText(csFilePath);

            string find = "request.AddFile(param.Value.Name, param.Value.Writer, param.Value.FileName, param.Value.ContentType);";
            string repl = "request.AddFile(param.Value.Name, param.Value.Writer, param.Value.FileName, param.Value.ContentLength, param.Value.ContentType);";
            csSourceCode = csSourceCode.Replace(find, repl);

            File.WriteAllText(csFilePath, csSourceCode);
            #endregion
        }
    }
}
