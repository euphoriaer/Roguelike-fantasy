using Slate;
using UnityEngine;

[Name("ģ�Ͷ������")]
// [Icon(typeof(Animator))]
[Attachable(typeof(ActorGroup))]
public class AnimTrack : CutsceneTrack
{
    protected override void OnCreate()
    {
        base.OnCreate();

        this.name = "ģ�Ͷ������";
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