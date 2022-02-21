using Slate;
using UnityEngine;

[Name("材质轨道")]
[Attachable(typeof(ActorGroup))]
public class MaterialTrack : CutsceneTrack
{
    protected override void OnCreate()
    {
        base.OnCreate();

        this.name = "材质轨道";
    }

    protected override void OnEnter()
    {
        base.OnEnter();
    }

    protected override void OnUpdate(float time, float previousTime)
    {
        base.OnUpdate(time, previousTime);
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}