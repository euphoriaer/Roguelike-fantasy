using Slate;
using UnityEngine;

[Name("特效轨道")]
[Attachable(typeof(ActorGroup))]
public class EffectTrack : CutsceneTrack
{
    protected override void OnCreate()
    {
        base.OnCreate();

        this.name = "特效轨道";
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
