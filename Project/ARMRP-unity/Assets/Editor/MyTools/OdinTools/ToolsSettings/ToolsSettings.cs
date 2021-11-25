using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ToolsSettings : GlobalConfig<ToolsSettings>
{
    //������Դ�Զ����ദ�� fbx animationClip Avatar �Զ����࣬������Դ�Զ�����������
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
