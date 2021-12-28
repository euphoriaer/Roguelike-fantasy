using System.Collections;
using System.Collections.Generic;
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

}

