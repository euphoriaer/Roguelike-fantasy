using Battle;
using Sirenix.OdinInspector;
using Slate;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityPhysics;

[Name("伤害盒子")]
[Attachable(typeof(HurtTrack))]
public class PhysicsClip : CutsceneClip<Transform>, IDirectable
{
    public bool IsNormalAttack = false;

    [Sirenix.OdinInspector.HideIf("IsNormalAttack")]
    [LabelText("伤害")]
    public int hurt = 10;

    [FormerlySerializedAs("colliders")]
    [LabelText("碰撞框")]
    public List<UnityPhysic.Shape> Shapes; //todo 绘制时需要 根据当前Time 显示/隐藏

    public bool IsFollow = false;

    public override float length
    {
        get { return _length; }
        set { _length = value; }
    }

    [HideInInspector][SerializeField] private float _length = 1f;

    private List<BoxCollider> _colliders = new List<BoxCollider>();
    private Action<Collider> _triggerAction;
    private float _time;
    private UnityPhysic _unityPhysic;

    public override void Refresh()
    {
    }

    protected override void OnEnter()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        _unityPhysic = new UnityPhysic(true, Shapes.ToArray(), actor.transform.position);
        _unityPhysic.PhysicObj.transform.SetParent(actor.transform, true);
        if (IsFollow)
        {
            _unityPhysic.PhysicObj.transform.ResetLocalCoords(false);
        }

        //需要设置碰撞事件
        _triggerAction = (Collider collider) =>
        {
            if (IsNormalAttack)
            {
                collider?.GetComponent<BehitSystem>()?.Injured(actor, collider.GetComponent<PropertySystem>().Attack);
            }
            else
            {
                collider?.GetComponent<BehitSystem>()?.Injured(actor, hurt);
            }
        };

        _unityPhysic.TriggerEnterAction += _triggerAction;
    }

    protected override void OnExit()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        _unityPhysic.Destory();
    }

#if UNITY_EDITOR //逻辑绘制

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.black;
    //     Gizmos.matrix = transform.localToWorldMatrix;
    //     Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    // }

    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (IsFollow)
        {
            Gizmos.matrix = actor.transform.localToWorldMatrix; //Gizmos 配合Collider 旋转
        }

        foreach (var collider in Shapes)
        {
            //绘制collider 到屏幕上

            switch (collider.Type)
            {
                //根据形状不同去绘制

                case UnityPhysic.Shape.ShapeType.Box:
                    //绘制在
                    Vector3 boxCenter;

                    if (IsFollow)
                    {
                        boxCenter = actor.transform.position +
                                    new Vector3(collider.offset.x - actor.transform.position.x,
                                        collider.offset.y - actor.transform.position.y,
                                        collider.offset.z - actor.transform.position.z) +
                                    new Vector3(0, collider.size.y * 0.5f, 0);
                    }
                    else
                    {
                        boxCenter = actor.transform.position +
                                     new Vector3(collider.offset.x,
                                         collider.offset.y,
                                         collider.offset.z) +
                                     new Vector3(0, collider.size.y * 0.5f, 0);
                    }

                    Gizmos.DrawWireCube(boxCenter, new Vector3(collider.size.x, collider.size.y, collider.size.z));

                    break;

                //case Shape.ShapeType.Circle:
                //    float angle = collider.size.x;
                //    float radius = collider.size.y;
                //    float height = collider.size.z;
                //    var arcCenter1 = actor.transform.position + new Vector3(collider.offset.x, collider.offset.y, collider.offset.z);

                //    GizmosHelper.DrawWireArc(arcCenter1,
                //            actor.transform.forward,
                //            radius,
                //            angle,
                //            0);
                //    if (height > 0)
                //    {
                //        var arcCenter2 = arcCenter1 + new Vector3(0, height, 0);
                //        var direction = actor.transform.forward;
                //        GizmosHelper.DrawWireArc(arcCenter2,
                //            direction,
                //            radius,
                //            angle,
                //            0);

                //        if (angle < 360f)
                //        {
                //            Gizmos.DrawLine(arcCenter1, arcCenter2);
                //        }
                //        Gizmos.DrawLine(arcCenter1 + direction * radius, arcCenter2 + direction * radius);

                //        var normal = Vector3.up;
                //        var leftDir = Quaternion.AngleAxis(-angle / 2f, normal) * direction;
                //        var rightDir = Quaternion.AngleAxis(angle / 2f, normal) * direction;
                //        var currentP = arcCenter1 + leftDir * radius;
                //        var currentP1 = arcCenter2 + leftDir * radius;
                //        Gizmos.DrawLine(currentP, currentP1);

                //        var currentP2 = arcCenter1 + rightDir * radius;
                //        var currentP3 = arcCenter2 + rightDir * radius;
                //        Gizmos.DrawLine(currentP2, currentP3);
                //    }
                //    break;

                default:
                    break;
            }
        }
    }

#endif
}