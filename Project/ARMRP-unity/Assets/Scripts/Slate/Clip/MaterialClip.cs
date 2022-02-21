using Sirenix.OdinInspector;
using Slate;
using UnityEngine;

[Name("材质改变")]
[Attachable(typeof(MaterialTrack))]
public class MaterialClip : CutsceneClip<Transform>, IDirectable
{
    public Material mat;

    public override void Refresh()
    {
        
    }

    protected override void OnExit()
    {
        //将Mat材质 换到目标身上
        //ActorComponent.gameObject 目标Gameobject 自己搜换材质
    }
}

