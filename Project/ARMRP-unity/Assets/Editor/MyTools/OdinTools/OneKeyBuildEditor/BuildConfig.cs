using Sirenix.OdinInspector;
using System;
using UnityEditor;

public class BuildConfig : SerializedScriptableObject
{
    public AndroidSdkVersions androidSDK_Version;
    public BuildTarget buildTarget;

    public string buildPath;

   
    public int versionIncrease;

    //public string buildResult;
}