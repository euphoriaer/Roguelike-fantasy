using Slate;

[Name("输入轨道")]
[Attachable(typeof(ActorGroup))]
public class InputTrack : CutsceneTrack
{
    protected override void OnCreate()
    {
        base.OnCreate();

        this.name = "输入轨道";
    }
}