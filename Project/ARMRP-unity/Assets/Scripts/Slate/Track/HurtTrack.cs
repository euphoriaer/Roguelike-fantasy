using Slate;
using System.Collections.Generic;

[Name("伤害轨道")]
// [Icon(typeof(Animator))]
[Attachable(typeof(ActorGroup))]
public class HurtTrack : CutsceneTrack
{

   

    protected override void OnCreate()
    {
        base.OnCreate();

        this.name = "伤害轨道";
    }

}