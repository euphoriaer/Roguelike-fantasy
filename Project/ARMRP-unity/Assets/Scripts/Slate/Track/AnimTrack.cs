using Slate;
using UnityEngine;

[Name("模型动画轨道")]
[Icon(typeof(Animator))]
[Attachable(typeof(ActorGroup))]
public class AnimTrack : CutsceneTrack
{
    protected override void OnCreate()
    {
        base.OnCreate();

        this.name = "模型动画轨道";
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