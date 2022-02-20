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
    private PlayableGraph playableGraph;
    public bool useRootMotion = true;
    private bool useBakedRootMotion;

    [SerializeField, HideInInspector]
    public bool isRootMotionPreBaked;
    [SerializeField, HideInInspector]
    private List<Vector3> rmPositions;
    [SerializeField, HideInInspector]
    private List<Quaternion> rmRotations;

    private Animator _animator;
    public Animator animator
    {
        get
        {
            if (_animator == null || _animator.gameObject != actor.gameObject)
            {
                _animator = actor.GetComponentInChildren<Animator>();
            }
            return _animator;
        }
    }
    protected override void OnCreate()
    {
        base.OnCreate();

        this.name = "模型动画轨道";
    }
    const int ROOTMOTION_FRAMERATE = 30;

    protected override void OnEnter()
    {
        //CreateAndPlayTree();
        //if (useRootMotion)
        //{
        //    BakeRootMotion();
        //}
        base.OnEnter();
    }
    void BakeRootMotion()
    {

        if (isRootMotionPreBaked)
        {
            animator.applyRootMotion = false;
            useBakedRootMotion = true;
            return;
        }

        var rb = animator.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.MovePosition(animator.transform.localPosition);
            rb.MoveRotation(animator.transform.localRotation);
        }

        useBakedRootMotion = false;
        animator.applyRootMotion = true;
        rmPositions = new List<Vector3>();
        rmRotations = new List<Quaternion>();
        var tempActiveClips = 0;
        var updateInterval = (1f / ROOTMOTION_FRAMERATE);
        for (var time = startTime - updateInterval; time <= endTime + updateInterval; time += updateInterval)
        {

            //EvaluateTrackClips(time, time - updateInterval, ref tempActiveClips);

            if (tempActiveClips > 0)
            {
                playableGraph.Evaluate(updateInterval);
            }

            //apparently animator automatically sets rigidbody pos/rot if attached on same go when evaluated.
            //thus we read pos/rot from rigidbody in such cases.
            var pos = rb != null ? rb.position : animator.transform.localPosition;
            var rot = rb != null ? rb.rotation : animator.transform.localRotation;
            rmPositions.Add(pos);
            rmRotations.Add(rot);
        }
        animator.applyRootMotion = false;
        useBakedRootMotion = true;
    }
    private void CreateAndPlayTree()
    {
        playableGraph = PlayableGraph.Create();
    }

    protected override void OnUpdate(float time, float previousTime)
    {
        //if (useRootMotion && useBakedRootMotion)
        //{
        //    ApplyBakedRootMotion(time);
        //}
        base.OnUpdate(time, previousTime);
    }

    void ApplyBakedRootMotion(float time)
    {
        var frame = Mathf.FloorToInt(time * ROOTMOTION_FRAMERATE);
        var nextFrame = frame + 1;
        nextFrame = nextFrame < rmPositions.Count ? nextFrame : rmPositions.Count - 1;

        var tNow = frame * (1f / ROOTMOTION_FRAMERATE);
        var tNext = nextFrame * (1f / ROOTMOTION_FRAMERATE);

        var posNow = rmPositions[frame];
        var posNext = rmPositions[nextFrame];
        var pos = Vector3.Lerp(posNow, posNext, Mathf.InverseLerp(tNow, tNext, time));
        animator.transform.localPosition = pos;

        var rotNow = rmRotations[frame];
        var rotNext = rmRotations[nextFrame];
        var rot = Quaternion.Lerp(rotNow, rotNext, Mathf.InverseLerp(tNow, tNext, time));
        animator.transform.localRotation = rot;
    }

    protected override void OnExit()
    {
        //if (useRootMotion)
        //{
        //    ApplyBakedRootMotion(endTime - startTime);
        //}
        base.OnExit();
    }
}

