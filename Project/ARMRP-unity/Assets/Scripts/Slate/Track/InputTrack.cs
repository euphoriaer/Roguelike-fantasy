using Slate;
using UnityEngine;

[Name("按键响应轨道")]
[Attachable(typeof(ActorGroup))]
public class InputTrack : CutsceneTrack
{
    
    
    protected override void OnCreate()
    {
        base.OnCreate();

        this.name = "按键响应轨道";
    }

    protected override void OnEnter()
    {
        base.OnEnter();
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}
