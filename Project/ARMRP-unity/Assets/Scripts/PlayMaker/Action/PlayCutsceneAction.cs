﻿using HutongGames.PlayMaker;
using Slate;
using System.Collections;
using UnityEngine;

[ActionCategory("Cutscene")]
public class PlayCutsceneAction : FsmStateAction
{
    [UnityEngine.Tooltip("Cutscene名字")]
    public string CutsceneName;

    //todo 按比例过渡，按固定时间过渡选项
    //[LabelText("Cutscene动作过渡")]
    [Header("Game Settings")]
    public float TransTime;

    private Cutscene _cutscene;

    public override void Reset()
    {
        base.Reset();
    }

    public override void OnEnter()
    {
        StartCoroutine(AnimatotBleedCrossFade(CutsceneName, TransTime));
    }

    private IEnumerator AnimatotBleedCrossFade(string name, float normalizedTime)
    {
        var Animator = Fsm.GameObject.GetComponent<Animator>();

        Animator.CrossFade(name, normalizedTime);
        _cutscene = CutsceneHelper.Instate(this.Owner, CutsceneName);
        if (Animator.GetCurrentAnimatorClipInfo(0).Length <= 0)
        {
            yield return 0;
        }
        else
        {
            var clipName = Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

            yield return new WaitWhile((() =>
            {
                var clipName = Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                Debug.Log("混合中");
                return clipName != name;
            }));
            Debug.Log("混合完成");
        }
        _cutscene.Play();
    }

    private IEnumerator AnimatotBleedCrossFadeFixed(string name, float FixedTime)
    {
        var Animator = Fsm.GameObject.GetComponent<Animator>();
        Animator.CrossFadeInFixedTime(name, FixedTime,0,0,0);
        _cutscene = CutsceneHelper.Instate(this.Owner, CutsceneName);
        Debug.Log("混合中");
        yield return new WaitForSeconds(FixedTime);
        Debug.Log("混合完成");


        _cutscene.Play();
    }

    public override void OnUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            //error 检测是否切换状态，或者在Cutscene 加入输入轨道
            Fsm.SendEventToFsmOnGameObject(this.Owner, this.Fsm.Name, "IdleToRun");
            Finish();
        }
    }

    public override void OnExit()
    {
        _cutscene.Stop();
        base.OnExit();
    }
}