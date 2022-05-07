using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Slate;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

[LabelWidth(100)]
public class ToolsSettings : GlobalConfig<ToolsSettings>
//动画资源自动分类处理 fbx animationClip Avatar 自动分类，所有资源自动处理型配置
{
    [LabelWidth(30)]
    [BoxGroup("AnimClip")]
    [HorizontalGroup("AnimClip/AnimationClip")]
    [FolderPath]
    public string Anim;

    [LabelWidth(30)]
    [BoxGroup("AnimClip")]
    [HorizontalGroup("AnimClip/AnimationClip", width: 80)]
    public string Mark;

    [LabelWidth(75)]
    [FolderPath]
    [BoxGroup("AnimationClip")]
    [LabelText("AvatarPath")]
    public string Avatarfolder;

    [LabelWidth(50)]
    [FolderPath]
    [BoxGroup("Fbx")]
    [LabelText("FBXPath")]
    public string FBXfolder;

    //[Space(50)] todo 如有必要
    //[OnValueChanged("MonoBehaviourExecuteChange")]
    //[LabelText("脚本执行顺序")]
    //public List<Component> MonoBehaviourExecute;

    //private void MonoBehaviourExecuteChange()
    //{
    //    Debug.Log("脚本执行顺序修改");
    //    //UnityEditor.
    //}

}