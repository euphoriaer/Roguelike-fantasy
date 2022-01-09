using System.Collections;
using HutongGames.PlayMaker;
using Slate;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[ActionCategory("Cutscene")]
public class PlayCutsceneAction : FsmStateAction
{
    private static AnimationClip _beforeAnimationClip;
    private static AnimationClip _afterAnimationClip;

    [UnityEngine.Tooltip("Cutscene名字")]
    public string CutsceneName;

    public string CutsceneClipName;

    //todo 按比例过渡，按固定时间过渡选项
    //[LabelText("Cutscene动作过渡")]
    [Header("Game Settings")]
    public float TransTime;

    private Cutscene _cutscene;

    private PlayAnimPlayable playClip;
    private AnimationClipPlayable playableClip1;
    private AnimationClipPlayable playableClip2;
    private AnimationMixerPlayable mixerPlayable;
    private PlayableGraph _playableGraph = PlayableGraph.Create();
    private float _weight = 0;

    private float time;
    public IEnumerator FinishMix()
    {
        Debug.Log("开始融合");
        yield return new WaitUntil((() =>
        {
            Debug.Log("融合进度"+ Mathf.Abs(_weight - 1));
            return Mathf.Abs(_weight - 1) < 0.01f;
        }));
        Debug.Log("完成融合");
        _cutscene.Play();
    }

    public override void Reset()
    {
        base.Reset();
    }

    public override void OnEnter()
    {
        time = 0;
        CutsceneInstate();
        if (_beforeAnimationClip == null || _afterAnimationClip == null)
        {//没有动画需要过度
            Debug.Log("直接播放");
            _cutscene.Play();
        }
        else
        {
            MixAnimationInit(_beforeAnimationClip, _afterAnimationClip);
            StartCoroutine(FinishMix());
        }
       
    }

    private void GetCutsceneClip()
    {
    }

    private void CutsceneInstate()
    {
        _cutscene = CutsceneHelper.Instate(this.Owner, CutsceneName);
        playClip = CutsceneHelper.GetCutsceneClip<PlayAnimPlayable>(_cutscene, CutsceneClipName);
        _afterAnimationClip = playClip.animationClip;
    }

    public void MixAnimationInit(AnimationClip before, AnimationClip after)
    {
        var Animator = Fsm.GameObject.GetComponent<Animator>();
        var playableOutput = AnimationPlayableOutput.Create(_playableGraph, "Animation", Animator);

        mixerPlayable = AnimationMixerPlayable.Create(_playableGraph, 2);
        // 将剪辑包裹在可播放项中

        playableClip1 = AnimationClipPlayable.Create(_playableGraph, before);
        playableClip2 = AnimationClipPlayable.Create(_playableGraph, after);

        // 将可播放项连接到输出

        _playableGraph.Connect(playableClip1, 0, mixerPlayable, 0);

        _playableGraph.Connect(playableClip2, 0, mixerPlayable, 1);

        playableOutput.SetSourcePlayable(mixerPlayable);
    }

    public override void OnFixedUpdate()
    {
        if (_beforeAnimationClip == null || _afterAnimationClip == null)
        {//没有动画需要过度
        }
        else if (Mathf.Abs(_weight - 1) > 0.01f)
        {
            time += Time.fixedDeltaTime;
            _weight = Mathf.Lerp(0, 1, time / TransTime);//t 是0,1的平滑过度，按时间从0到1
            mixerPlayable.SetInputWeight(0, 1.0f - _weight);

            mixerPlayable.SetInputWeight(1, _weight);
            _playableGraph.Evaluate(time);
        }
    }

    public override void OnUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Fsm.SendEventToFsmOnGameObject(this.Owner, this.Fsm.Name, "IdleToRun");
            Finish();
        }
    }

    public override void OnExit()
    {
        _beforeAnimationClip = _afterAnimationClip;
        _cutscene.Stop();
        base.OnExit();
    }
}