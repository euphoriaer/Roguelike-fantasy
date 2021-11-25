using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

[TypeInfoBox("<size=20>һ�����</size>")]
public class OneKeyBuildlEditor : GlobalConfig<OneKeyBuildlEditor>
{
    private const int addressableBundlesDirOrder = 3;
    private const int AddressableButtonOrder = 2;
    private const int Addressables��ַ����Order = 3;
    private const int PlayerSetting�������Order = 4;
    private const int PlayerSetting������Order = 5;
    private const string HFS = "HFS";
    private string androidPath;
    private string winPath;
#pragma warning disable CS0414
    private bool isBuild = false;
#pragma warning restore CS0414

    [TitleGroup("PlayerSetting �������", null, TitleAlignments.Left, true, true, false, PlayerSetting�������Order)]
    public BuildConfig buildConfig;

    private List<string> m_activeScene = new List<string>();

    [ShowInInspector]
    [TitleGroup("PlayerSetting �������", null, TitleAlignments.Left, true, true, false, PlayerSetting�������Order)]
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

    [TitleGroup("PlayerSetting �������")]
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

    [TitleGroup("PlayerSetting �������"), ShowInInspector]
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

    [TitleGroup("PlayerSetting �������"), ShowInInspector]
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

    [TitleGroup("PlayerSetting �������"), ShowInInspector]
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

    [TitleGroup("PlayerSetting �������"), ShowInInspector]
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
    [TitleGroup("PlayerSetting �������"), ShowInInspector]
    [InfoBox("��Ҫ����İ汾�ţ������޸ģ�Ĭ��������")]
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

    [TitleGroup("PlayerSetting �������"), ShowInInspector]
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

    [TitleGroup("PlayerSetting �������"), ShowInInspector]
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
    /// ���
    /// </summary>
    [TitleGroup("PlayerSetting �������")]
    [Button(90), GUIColor(0.4f, 0.8f, 1f)]
    public void BuildPackage()
    {
        //this.isBuild = !this.isBuild;
        PlayerSettings.companyName = companyName;
        PlayerSettings.productName = productName;
        PlayerSettings.bundleVersion = version;

        PlayerSettings.Android.targetSdkVersion = androidSDK_Version;

        #region ��ʼ���

        isBuilding = BuildPipeline.isBuildingPlayer;

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = activeScenes.ToArray();
        //���Ŀ��·��
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
        //���Ŀ��ƽ̨
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

        #endregion ��ʼ���
    }

    

    /// <summary>
    /// ���ʧ��
    /// </summary>
    /// <param name="report"></param>
    private void FailBuild(BuildReport report)
    {
        buildResult = "���ʧ�ܣ��뿴Console��Ϣ";
    }

    /// <summary>
    /// ����ɹ�
    /// </summary>
    /// <param name="report"></param>
    private void SucessBuild(BuildReport report)
    {
        BuildSummary summary = report.summary;
        string size = "�������Сʧ��";
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
        buildResult = "����ɹ�: " + summary.outputPath + "\n" +
            "��װ���С: " + size + "\n" +
            "���ʱ��: " + time + "\n";

        string[] versionsNum = PlayerSettings.bundleVersion.Split('.');
        int tempInt = int.Parse(versionsNum[2]) + versionIncrease;
        versionsNum[2] = tempInt.ToString();
        var tempVersionsNum = String.Join(".", versionsNum);

        PlayerSettings.bundleVersion = tempVersionsNum;
        EditorUtility.OpenWithDefaultApp(buildPath.Replace(@"/", @"\"));
    }

    [TitleGroup("PlayerSetting ������", null, TitleAlignments.Left, true, true, false, PlayerSetting������Order)]
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
    /// ��ȡ�ļ��еĴ�С
    /// </summary>
    /// <param name="dir">�ļ���Ŀ¼</param>
    /// <param name="dirSize">�����ļ��д�С����������</param>
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
            Console.WriteLine("��ȡ�ļ���Сʧ��" + ex.Message);
        }
    }
}