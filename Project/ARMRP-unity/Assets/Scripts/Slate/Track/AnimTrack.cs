using System.Collections;
using System.Collections.Generic;
using Slate;
using UnityEngine;

[Name("ģ�Ͷ������")]
[Icon(typeof(Animator))]
[Attachable(typeof(ActorGroup))]
public class AnimTrack : CutsceneTrack
{
  
    protected override void OnCreate()
    {
        base.OnCreate();

        this.name = "ģ�Ͷ������";
    }

}

