using Sirenix.OdinInspector;
using Slate;
using UnityEngine;

[Name("播放特效")]
[Attachable(typeof(EffectTrack))]
public class EffectClip : CutsceneClip<Animator>, IDirectable
{
    [Button("刷新", 40)]
    public override void Refresh()
    {
        Debug.Log("特效刷新了");
    }

    protected override void OnEnter()
    {
       //明天直播实现我们刚才那个特效
    }

    protected override void OnExit()
    {
        //提高任务,实现一个环绕特效Cutscene 
        base.OnExit();
    }

    protected override void OnUpdate(float time)
    {
        //自己研究
        base.OnUpdate(time);
    }
}