using System.Collections;
using System.Collections.Generic;
using Slate;
using UnityEngine;
using UnityEngine.Playables;

[Name("模型动画轨道")]
[Icon(typeof(Animator))]
[Attachable(typeof(ActorGroup))]
public class AnimTrack : CutsceneTrack
{
    private PlayableGraph graph;

    protected override void OnCreate()
    {
        base.OnCreate();

        this.name = "模型动画轨道";
    }

    protected override void OnEnter()
    {
        CreateAndPlayTree();
        base.OnEnter();
    }

    private void CreateAndPlayTree()
    {
        graph = PlayableGraph.Create();
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

