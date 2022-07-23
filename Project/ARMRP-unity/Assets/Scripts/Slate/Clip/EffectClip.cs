using Battle;
using Sirenix.OdinInspector;
using Slate;
using UnityEngine;

[Name("播放特效")]
[Attachable(typeof(EffectTrack))]
public class EffectClip : CutsceneClip<Animator>, IDirectable
{
    //轨道控制粒子播放时间
    [LabelText("特效Prefab")]
    public GameObject Obj;

    [LabelText("特效是否跟随人物")]
    public bool isFollow=false;
    [LabelText("偏移")]
    public Vector3 Offset=Vector3.zero;
    [LabelText("旋转")]
    public Vector3 Rotation=Vector3.zero;

    [Button("刷新", 40)]
    public override void Refresh()
    {
        Debug.Log("特效刷新了");
        if (actor!=null)
        {
            particle = actor.GetComponent<ParticleSystem>();
            _length = particle.duration;
        }
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
        //Debug.Log("进入Fx enter");
        FxObj = GameObject.Instantiate(Obj, actor.transform.position, Quaternion.identity, actor.transform);
        FxObj.transform.forward = actor.transform.forward;
        FxObj.transform.localPosition += Offset;
        FxObj.transform.localRotation=Quaternion.Euler(Rotation);
        anim = FxObj.GetComponent<Animator>();
        particle = FxObj.GetComponent<ParticleSystem>();
        particle.Play(true);
    }

    protected override void OnExit()
    {
        
        GameObject.Destroy(FxObj);
        anim = null;
        particle = null;
        base.OnExit();

    }

    protected override void OnUpdate(float time)
    {
        //自己研究
      
        //判断是粒子特效 还是动画特效
        
        if (anim!=null)
        {
            anim.Update(time);
        }

        if (particle!=null)
        {
            particle.Simulate(time,true);
        }

        if (isFollow)
        {
            
        }

    }
}
