using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Slate;

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
}