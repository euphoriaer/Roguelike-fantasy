using Sirenix.OdinInspector;
using Slate;
using UnityEngine;

[Name("播放特效")]
[Attachable(typeof(EffectTrack))]
public class EffectClip : CutsceneClip<Animator>, IDirectable
{
    [LabelText("特效Prefab")]
    public GameObject Obj;

    [LabelText("特效是否跟随人物")]
    public bool isFollow=false;

    [Button("刷新", 40)]
    public override void Refresh()
    {
        Debug.Log("特效刷新了");
    }
    
    public override float length
    {
        get { return _length; }
        set { _length = value; }
    }

    private GameObject FxObj;
    private Animator anim;
    private ParticleSystem particle;
    [HideInInspector]
    [SerializeField] private float _length = 1 / 30f;

    protected override void OnEnter()
    {
        Debug.Log("进入Fx enter");
        FxObj= GameObject.Instantiate(Obj, actor.transform.position,Quaternion.identity);
        anim = FxObj.GetComponent<Animator>();
        particle = FxObj.GetComponent<ParticleSystem>();
    }

    protected override void OnExit()
    {
        Debug.Log("销毁Fx ");
        //error chen 提高任务,实现一个环绕特效Cutscene 
        GameObject.Destroy(FxObj);
        anim = null;
        particle = null;
        base.OnExit();

    }

    protected override void OnUpdate(float time)
    {
        //自己研究
        base.OnUpdate(time);
        //判断是粒子特效 还是动画特效
        
        if (anim!=null)
        {
            anim.Update(time);
        }

        if (particle!=null)
        {
            particle.Play();
        }

        if (isFollow)
        {
            FxObj.transform.position = actor.transform.position;
        }

    }
}