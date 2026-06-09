using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayerCodeGenerator.Enums;
using BusinessLayerCodeGenerator.Utility;
using  BusinessLayerCodeGenerator.Enums;

namespace BusinessLayerCodeGenerator.Business
{
    public class GenerateProjectsFiles
    {

        private static string BuildSln(
        string projectName,
        string projectGuid,
        string solutionGuid,
        enProjectType type)
        {
            string projTypeGuid = "";
            switch (type)
            {
                case enProjectType.WindowsForms:
                    projTypeGuid = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";
                    break;
                case enProjectType.Console:
                    projTypeGuid = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";
                    break;
                case enProjectType.ClassLibrary:
                    projTypeGuid = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));

            }


            return $@"
                Microsoft Visual Studio Solution File, Format Version 12.00
                # Visual Studio Version 17
                VisualStudioVersion = 17.0.31903.59
                MinimumVisualStudioVersion = 10.0.40219.1
                Project(""{projTypeGuid}"") = ""{projectName}"", ""{projectName}\{projectName}.csproj"", ""{projectGuid}""
                EndProject
                Global
                	GlobalSection(SolutionConfigurationPlatforms) = preSolution
                		Debug|Any CPU = Debug|Any CPU
                		Release|Any CPU = Release|Any CPU
                	EndGlobalSection
                	GlobalSection(ProjectConfigurationPlatforms) = postSolution
                		{projectGuid}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                		{projectGuid}.Debug|Any CPU.Build.0 = Debug|Any CPU
                		{projectGuid}.Release|Any CPU.ActiveCfg = Release|Any CPU
                		{projectGuid}.Release|Any CPU.Build.0 = Release|Any CPU
                	EndGlobalSection
                	GlobalSection(ExtensibilityGlobals) = postSolution
                		SolutionGuid = {solutionGuid}
                	EndGlobalSection
                EndGlobal
                ";
        }

        private static string BuildCsprojFramework(
            string projectName,
            string projectGuid,
            enProjectType type)
                {
                    string outputType = "";
                    string extraRefs = "";
                    string extraItems = "";
        
                    switch (type)
                    {
                        case enProjectType.WindowsForms:
                            outputType = "WinExe";
        
                            extraRefs =
                @"    <Reference Include=""System.Drawing"" />
            <Reference Include=""System.Windows.Forms"" />
        ";
        
                            extraItems =
                @"  <ItemGroup>
            <Compile Include=""Form1.cs"">
              <SubType>Form</SubType>
            </Compile>
            <Compile Include=""Form1.Designer.cs"">
              <DependentUpon>Form1.cs</DependentUpon>
            </Compile>
            <Compile Include=""Program.cs"" />
            <Compile Include=""Properties\AssemblyInfo.cs"" />
          </ItemGroup>
          <ItemGroup>
            <EmbeddedResource Include=""Properties\Resources.resx"">
              <Generator>ResXFileCodeGenerator</Generator>
              <LastGenOutput>Resources.Designer.cs</LastGenOutput>
            </EmbeddedResource>
          </ItemGroup>";
                            break;
        
                        case enProjectType.Console:
                            outputType = "Exe";
        
                            extraItems =
                @"  <ItemGroup>
            <Compile Include=""Program.cs"" />
            <Compile Include=""Properties\AssemblyInfo.cs"" />
          </ItemGroup>";
                            break;
        
                        case enProjectType.ClassLibrary:
                            outputType = "Library";
        
                            extraItems =
                @"  <ItemGroup>
            <Compile Include=""Class1.cs"" />
            <Compile Include=""Properties\AssemblyInfo.cs"" />
          </ItemGroup>";
                            break;
                    }
        
                    return $@"<?xml version=""1.0"" encoding=""utf-8""?>
        <Project ToolsVersion=""15.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
        
          <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props""
                  Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
        
          <PropertyGroup>
            <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
            <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
            <ProjectGuid>{projectGuid}</ProjectGuid>
            <OutputType>{outputType}</OutputType>
            <RootNamespace>{projectName}</RootNamespace>
            <AssemblyName>{projectName}</AssemblyName>
            <TargetFrameworkVersion>v4.8</TargetFrameworkVersion>
            <FileAlignment>512</FileAlignment>
            <AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>
            <Deterministic>true</Deterministic>
          </PropertyGroup>
        
          <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
            <DebugSymbols>true</DebugSymbols>
            <DebugType>full</DebugType>
            <Optimize>false</Optimize>
            <OutputPath>bin\Debug\</OutputPath>
            <DefineConstants>DEBUG;TRACE</DefineConstants>
          </PropertyGroup>
        
          <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
            <DebugType>pdbonly</DebugType>
            <Optimize>true</Optimize>
            <OutputPath>bin\Release\</OutputPath>
            <DefineConstants>TRACE</DefineConstants>
          </PropertyGroup>
        
          <ItemGroup>
            <Reference Include=""System"" />
            <Reference Include=""System.Configuration"" />
            <Reference Include=""System.Core"" />
            <Reference Include=""System.Xml.Linq"" />
            <Reference Include=""System.Data"" />
            <Reference Include=""System.Xml"" />
        {extraRefs}
          </ItemGroup>
        
        {extraItems}
        
          <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
        
        </Project>";
        }

        private static string BuildCsprojCore(enProjectType type)
        {
            string outputType = "";
            string extraProps = "";
            string extraPkg = "";

            switch (type)
            {
                case enProjectType.WindowsForms:
                    outputType = "WinExe";
                    extraProps = "    <UseWindowsForms>true</UseWindowsForms>\r\n";
                    break;

                case enProjectType.Console:
                    outputType = "Exe";
                    break;

                case enProjectType.ClassLibrary:
                    outputType = "Library";
                    break;
            }

            return $@"<Project Sdk=""Microsoft.NET.Sdk"">

          <PropertyGroup>
            <OutputType>{outputType}</OutputType>
            <TargetFramework>net8.0-windows</TargetFramework>
            <Nullable>enable</Nullable>
            <ImplicitUsings>enable</ImplicitUsings>
        {extraProps}  </PropertyGroup>
        
          <ItemGroup>
            <PackageReference Include=""Microsoft.Extensions.Configuration.Json"" Version=""8.0.0"" />
        {extraPkg}  </ItemGroup>
        
        </Project>";
        }

        private static string BuildProgramCs(
    string projectName,
    enTargetPlatform framework,
    enProjectType projectType)
        {
            if (projectType == enProjectType.WindowsForms)
            {
                if (framework == enTargetPlatform.NetFramework)
                {
                    return $@"using System;
            using System.Windows.Forms;
            
            namespace {projectName}
            {{
                static class Program
                {{
                    [STAThread]
                    static void Main()
                    {{
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form1());
                    }}
                }}
            }}";
                            }
                            else
                            {
                                return $@"using System;
            using System.Windows.Forms;
            
            namespace {projectName};
            
            static class Program
            {{
                [STAThread]
                static void Main()
                {{
                    ApplicationConfiguration.Initialize();
                    Application.Run(new Form1());
                }}
            }}";
                            }
                        }
                        else if (projectType == enProjectType.Console)
                        {
                            if (framework == enTargetPlatform.NetFramework)
                            {
                                return $@"using System;
            using System.Configuration;
            
            namespace {projectName}
            {{
                class Program
                {{
                    static void Main(string[] args)
                    {{
                        string connStr = ConfigurationManager
                            .ConnectionStrings[""DefaultConnection""]
                            .ConnectionString;
            
                        Console.WriteLine(""Connection: "" + connStr);
                        Console.ReadLine();
                    }}
                }}
            }}";
                            }
                            else
                            {
                                return @"using Microsoft.Extensions.Configuration;
            
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(""appsettings.json"", optional: false)
                .Build();
            
            string connStr = config.GetConnectionString(""DefaultConnection"")!;
            
            Console.WriteLine($""Connection: {connStr}"");";
                }
            }

            // Class Library has no Program.cs
            return string.Empty;
        }

        private static string BuildForm1Cs(
    string projectName,
    enTargetPlatform framework)
        {
            if (framework == enTargetPlatform.NetFramework)
            {
                return $@"using System;
        using System.Windows.Forms;
        
        namespace {projectName}
        {{
            public partial class Form1 : Form
            {{
                public Form1()
                {{
                    InitializeComponent();
                }}
            }}
        }}";
                    }
        
                    return $@"namespace {projectName};
        
        public partial class Form1 : Form
        {{
            public Form1()
            {{
                InitializeComponent();
            }}
        }}";
        }

        private static string BuildForm1DesignerCs(
    string projectName,
    enTargetPlatform framework)
        {
            if (framework == enTargetPlatform.NetFramework)
            {
                return $@"namespace {projectName}
        {{
            partial class Form1
            {{
                private System.ComponentModel.IContainer components = null;
        
                protected override void Dispose(bool disposing)
                {{
                    if (disposing && (components != null))
                        components.Dispose();
        
                    base.Dispose(disposing);
                }}
        
                #region Windows Form Designer generated code
        
                private void InitializeComponent()
                {{
                    this.SuspendLayout();
        
                    this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                    this.ClientSize = new System.Drawing.Size(800, 450);
                    this.Name = ""Form1"";
                    this.Text = ""Form1"";
        
                    this.ResumeLayout(false);
                }}
        
                #endregion
            }}
        }}";
                    }
        
                    return $@"namespace {projectName};
        
        partial class Form1
        {{
            private System.ComponentModel.IContainer components = null;
        
            protected override void Dispose(bool disposing)
            {{
                if (disposing && (components != null))
                    components.Dispose();
        
                base.Dispose(disposing);
            }}
        
            #region Windows Form Designer generated code
        
            private void InitializeComponent()
            {{
                this.SuspendLayout();
        
                this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(800, 450);
                this.Name = ""Form1"";
                this.Text = ""Form1"";
        
                this.ResumeLayout(false);
            }}
        
            #endregion
        }}";
        }

        private static string BuildClass1Cs(
    string projectName,
    enTargetPlatform framework)
        {
            if (framework == enTargetPlatform.NetFramework)
            {
                return $@"namespace {projectName}
        {{
            public class Class1
            {{
            }}
        }}";
                    }
        
                    return $@"namespace {projectName};
        
        public class Class1
        {{
        }}";
        }

        private static string BuildAssemblyInfo(string projectName)
        {
            return $@"using System.Reflection;
            using System.Runtime.InteropServices;
            
            [assembly: AssemblyTitle(""{projectName}"")]
            [assembly: AssemblyDescription("""")]
            [assembly: AssemblyConfiguration("""")]
            [assembly: AssemblyCompany("""")]
            [assembly: AssemblyProduct(""{projectName}"")]
            [assembly: AssemblyCopyright(""Copyright © 2024"")]
            [assembly: AssemblyTrademark("""")]
            [assembly: AssemblyCulture("""")]
            [assembly: ComVisible(false)]
            [assembly: AssemblyVersion(""1.0.0.0"")]
            [assembly: AssemblyFileVersion(""1.0.0.0"")]";
        }

        private static string BuildAppConfig(string connectionString)
        {
            return $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
            <configuration>
              <startup>
                <supportedRuntime version=""v4.0"" sku="".NETFramework,Version=v4.8"" />
              </startup>
              <connectionStrings>
                <add name=""DefaultConnection""
                     connectionString=""{connectionString}""
                     providerName=""System.Data.SqlClient"" />
              </connectionStrings>
            </configuration>";
        }

        private static string BuildAppSettingsJson(string connectionString)
        {
            return $@"{{
              ""ConnectionStrings"": {{
                ""DefaultConnection"": ""{connectionString}""
              }},
              ""Logging"": {{
                ""LogLevel"": {{
                  ""Default"": ""Information"",
                  ""Microsoft"": ""Warning""
                }}
              }}
            }}";
        }

public static bool Start(
    string outputPath,
    string projectName,
    enTargetPlatform framework,
    enProjectType projectType,
    string connectionString)
    {
        // ── Paths ────────────────────────────────
        string slnDir = Path.Combine(outputPath, projectName);
        string projDir = Path.Combine(slnDir, projectName);
        string propsDir = Path.Combine(projDir, "Properties");

        // ── GUIDs ────────────────────────────────
        string projectGuid = Utility.Utility.NewGuidString();
        string solutionGuid = Utility.Utility.NewGuidString();

        // ── 1. Folder structure ──────────────────
        Utility.Utility.EnsureDir(slnDir);
        Utility.Utility.EnsureDir(projDir);
            Utility.Utility.EnsureDir(propsDir);

        // ── 2. .sln ──────────────────────────────
        string slnPath = Path.Combine(slnDir, $"{projectName}.sln");

            Utility.Utility.WriteTextFile(
            slnPath,
            BuildSln(
                projectName,
                projectGuid,
                solutionGuid,
                projectType));

        // ── 3. .csproj ───────────────────────────
        string csprojPath = Path.Combine(
            projDir,
            $"{projectName}.csproj");

        if (framework == enTargetPlatform.NetFramework)
        {
            Utility.Utility.WriteTextFile(
                csprojPath,
                BuildCsprojFramework(
                    projectName,
                    projectGuid,
                    projectType));
        }
        else
        {
            Utility.Utility.WriteTextFile(
                csprojPath,
                BuildCsprojCore(projectType));
        }

        // ── 4. Source files ──────────────────────
        switch (projectType)
        {
            case enProjectType.WindowsForms:

                Utility.Utility.WriteTextFile(
                    Path.Combine(projDir, "Program.cs"),
                    BuildProgramCs(
                        projectName,
                        framework,
                        projectType));

                Utility.Utility.WriteTextFile(
                    Path.Combine(projDir, "Form1.cs"),
                    BuildForm1Cs(
                        projectName,
                        framework));

                Utility.Utility.WriteTextFile(
                    Path.Combine(projDir, "Form1.Designer.cs"),
                    BuildForm1DesignerCs(
                        projectName,
                        framework));

                break;

            case enProjectType.Console:

                Utility.Utility.WriteTextFile(
                    Path.Combine(projDir, "Program.cs"),
                    BuildProgramCs(
                        projectName,
                        framework,
                        projectType));

                break;

            case enProjectType.ClassLibrary:

                Utility.Utility.WriteTextFile(
                    Path.Combine(projDir, "Class1.cs"),
                    BuildClass1Cs(
                        projectName,
                        framework));

                break;
        }

        // ── 5. Config file ───────────────────────
        if (framework == enTargetPlatform.NetFramework)
        {
            Utility.Utility.WriteTextFile(
                Path.Combine(projDir, "App.config"),
                BuildAppConfig(connectionString));

            Utility.Utility.WriteTextFile(
                Path.Combine(propsDir, "AssemblyInfo.cs"),
                BuildAssemblyInfo(projectName));
        }
        else
        {
            Utility.Utility.WriteTextFile(
                Path.Combine(projDir, "appsettings.json"),
                BuildAppSettingsJson(connectionString));
        }

        // ── 6. Open Solution ─────────────────────
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = slnPath,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[WARN] Failed to open solution: {ex.Message}");
        }

        Console.WriteLine(
            $"[OK] Project generated at: {slnDir}");

        return true;
    }
    
    }
}
