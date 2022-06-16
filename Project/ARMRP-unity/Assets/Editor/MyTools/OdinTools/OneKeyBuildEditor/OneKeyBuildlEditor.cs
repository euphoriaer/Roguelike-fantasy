using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

[TypeInfoBox("<size=20>一键打包</size>")]
public class OneKeyBuildlEditor : GlobalConfig<OneKeyBuildlEditor>
{
    private const int addressableBundlesDirOrder = 3;
    private const int AddressableButtonOrder = 2;
    private const int Addressables地址管理Order = 3;
    private const int PlayerSetting打包设置Order = 4;
    private const int PlayerSetting打包结果Order = 5;
    private const string HFS = "HFS";
    private string androidPath;
    private string winPath;
#pragma warning disable CS0414
    private bool isBuild = false;
#pragma warning restore CS0414

    [TitleGroup("PlayerSetting 打包设置", null, TitleAlignments.Left, true, true, false, PlayerSetting打包设置Order)]
    public BuildConfig buildConfig;

    private List<string> m_activeScene = new List<string>();

    [ShowInInspector]
    [TitleGroup("PlayerSetting 打包设置", null, TitleAlignments.Left, true, true, false, PlayerSetting打包设置Order)]
    public List<string> activeScenes
    {
        get
        {
            var tempScenes = new List<string>();
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            foreach (var item in scenes)
            {
                if (item.enabled)
                {
                    tempScenes.Add(item.path);
                }
            }
            return tempScenes;
        }
    }

    private bool isBuilding;

    [TitleGroup("PlayerSetting 打包设置")]
    public AndroidSdkVersions androidSDK_Version
    {
        get
        {
            return buildConfig.androidSDK_Version;
        }
        set
        {
            if (buildConfig != null)
            {
                buildConfig.androidSDK_Version = value;
            }
        }
    }

    [TitleGroup("PlayerSetting 打包设置"), ShowInInspector]
    [FolderPath(AbsolutePath = true)]
    public string buildPath
    {
        get
        {
            if (buildConfig == null)
            {
                return "";
            }
            return buildConfig.buildPath;
        }
        set
        {
            if (buildConfig != null)
            {
                buildConfig.buildPath = value;
            }
            EditorPrefs.SetString("OdinBuild.BuildPath", value);
        }
    }

    [TitleGroup("PlayerSetting 打包设置"), ShowInInspector]
    [ReadOnly]
    public BuildTargetGroup buildTarget
    {
        get
        {
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                return BuildTargetGroup.Android;
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                return BuildTargetGroup.iOS;
            }
            else
            {
                return BuildTargetGroup.Standalone;
            }
        }
    }

    [TitleGroup("PlayerSetting 打包设置"), ShowInInspector]
    public string companyName
    {
        get
        {
            return PlayerSettings.companyName;
        }
        set
        {
            PlayerSettings.companyName = value;
        }
    }

    [TitleGroup("PlayerSetting 打包设置"), ShowInInspector]
    public string productName
    {
        get
        {
            return PlayerSettings.productName;
        }
        set
        {
            PlayerSettings.productName = value;
        }
    }

    [ReadOnly]
    [TitleGroup("PlayerSetting 打包设置"), ShowInInspector]
    [InfoBox("将要打包的版本号（不可修改，默认自增）")]
    public string version
    {
        get
        {
            return PlayerSettings.bundleVersion;
        }
        set
        {
            PlayerSettings.bundleVersion = value;
        }
    }

    [TitleGroup("PlayerSetting 打包设置"), ShowInInspector]
    public int versionIncrease
    {
        get
        {
            if (buildConfig == null)
            {
                return 0;
            }
            return buildConfig.versionIncrease;
        }
        set
        {
            if (buildConfig != null)
            {
                buildConfig.versionIncrease = value;
            }
        }
    }

    [TitleGroup("PlayerSetting 打包设置"), ShowInInspector]
    public ScriptingImplementation scriptingBacked
    {
        get
        {
            ScriptingImplementation ScriptingBackend = PlayerSettings.GetScriptingBackend(buildTarget);
            return ScriptingBackend;
        }
        set
        {
            PlayerSettings.SetScriptingBackend(buildTarget, value);
        }
    }

    /// <summary>
    /// 打包
    /// </summary>
    [TitleGroup("PlayerSetting 打包设置")]
    [Button(90), GUIColor(0.4f, 0.8f, 1f)]
    public void BuildPackage()
    {
        //this.isBuild = !this.isBuild;
        PlayerSettings.companyName = companyName;
        PlayerSettings.productName = productName;
        PlayerSettings.bundleVersion = version;

        PlayerSettings.Android.targetSdkVersion = androidSDK_Version;

        #region 开始打包

        isBuilding = BuildPipeline.isBuildingPlayer;

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = activeScenes.ToArray();
        //打包目标路径
        androidPath = buildPath + @"\" + buildTarget.ToString() + @"\" + productName + "_" + version + ".apk";
        winPath = buildPath + @"\" + buildTarget.ToString() + @"\" + productName + "_" + version + ".exe";
        if (buildTarget == BuildTargetGroup.Android)
        {
            buildPlayerOptions.locationPathName = androidPath;
        }
        else
        {
            buildPlayerOptions.locationPathName = winPath;
        }
        //打包目标平台
        if (buildTarget.ToString() == "Standalone")
        {
            buildPlayerOptions.target = BuildTarget.StandaloneWindows;
        }
        else
        {
            buildPlayerOptions.target = (BuildTarget)Enum.Parse(typeof(BuildTarget), buildTarget.ToString());
        }

        buildPlayerOptions.options = BuildOptions.None;
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.platform);
            Debug.Log("Build succeeded: " + summary.totalSize + "bytes");
            Debug.Log("Build succeeded: " + summary.totalTime + "s");

            SucessBuild(report);
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
            FailBuild(report);
        }
        isBuilding = BuildPipeline.isBuildingPlayer;

        #endregion 开始打包
    }

    

    /// <summary>
    /// 打包失败
    /// </summary>
    /// <param name="report"></param>
    private void FailBuild(BuildReport report)
    {
        buildResult = "打包失败，请看Console信息";
    }

    /// <summary>
    /// 打包成功
    /// </summary>
    /// <param name="report"></param>
    private void SucessBuild(BuildReport report)
    {
        BuildSummary summary = report.summary;
        string size = "检测打包大小失败";
        if (File.Exists(androidPath))
        {
            FileInfo apk = new FileInfo(androidPath);
            size = " " + (apk.Length / (1024.00 * 1024.00)).ToString("f2") + "MB";
        }
        else
        {
            long m_size = 0;
            GetDirSizeByPath((buildPath + @"\" + buildTarget.ToString()).Replace(@"/", @"\"), ref m_size);
            size = " " + (m_size / (1024.00 * 1024.00)).ToString("f2") + "MB";
        }

        string time = " " + summary.totalTime + "s";
        buildResult = "打包成功: " + summary.outputPath + "\n" +
            "安装后大小: " + size + "\n" +
            "打包时长: " + time + "\n";

        string[] versionsNum = PlayerSettings.bundleVersion.Split('.');
        int tempInt = int.Parse(versionsNum[2]) + versionIncrease;
        versionsNum[2] = tempInt.ToString();
        var tempVersionsNum = String.Join(".", versionsNum);

        PlayerSettings.bundleVersion = tempVersionsNum;
        EditorUtility.OpenWithDefaultApp(buildPath.Replace(@"/", @"\"));
    }

    [TitleGroup("PlayerSetting 打包结果", null, TitleAlignments.Left, true, true, false, PlayerSetting打包结果Order)]
    [ReadOnly]
    [MultiLineProperty(3), ShowInInspector]
    public string buildResult
    {
        get
        {
            return EditorPrefs.GetString("OdinBuild.buildResult");
        }
        set
        {
            EditorPrefs.SetString("OdinBuild.buildResult", value);
        }
    }

    /// <summary>
    /// 获取文件夹的大小
    /// </summary>
    /// <param name="dir">文件夹目录</param>
    /// <param name="dirSize">返回文件夹大小，传递引用</param>
    private static void GetDirSizeByPath(string dir, ref long dirSize)
    {
        try
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dir);

            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            FileInfo[] files = dirInfo.GetFiles();

            foreach (var item in dirs)
            {
                GetDirSizeByPath(item.FullName, ref dirSize);
            }

            foreach (var item in files)
            {
                dirSize += item.Length;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("获取文件大小失败" + ex.Message);
        }
    }
}