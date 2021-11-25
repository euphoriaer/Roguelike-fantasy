using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ToolsSettings : GlobalConfig<ToolsSettings>
{
    //动画资源自动分类处理 fbx animationClip Avatar 自动分类，所有资源自动处理型配置
    [HorizontalGroup]
    [LabelText("AnimationClipPath")]
    public string AnimationClipfolder;

    [HorizontalGroup]
    [LabelText("AvatarPath")]
    public string Avatarfolder;

    [HorizontalGroup]
    [LabelText("FBXPath")]
    public string FBXfolder;


}
