using Sirenix.OdinInspector;
using Slate;
using System;
using System.Collections.Generic;
using UnityEngine;

[Name("伤害盒子")]
[Attachable(typeof(HurtTrack))]
public class HurtClip : CutsceneClip<Transform>, IDirectable
{
    [LabelText("伤害")]
    public int hurt;

    [LabelText("碰撞框")]
    public List<Shape> colliders; //todo 绘制时需要 根据当前Time 显示/隐藏

    public override float length
    {
        get => m_length;
        set => m_length = value;
    }

    private float m_length = 1;//todo 长度显示有Bug

    private List<BoxCollider> m_colliders = new List<BoxCollider>();
    private Action<Collision> triggerAction;

    public override void Refresh()
    {
    }

    protected override void OnEnter()
    {
        //error 等待物理系统重构
        //if (!Application.isPlaying)
        //{
        //    return;
        //}
        //foreach (var collider in colliders)
        //{
        //    BoxCollider boxCollider = actor.AddComponent<BoxCollider>();
        //    boxCollider.center = collider.size;
        //    boxCollider.size = collider.size;
        //    boxCollider.isTrigger = true;
        //    m_colliders.Add(boxCollider);
        //}
        ////需要设置碰撞事件
        //triggerAction = (Collision collider) =>
        // {
        //     Debug.Log("碰撞到了" + collider.gameObject.name);
        // };
        //actor.GetComponent<Property>().CollisionAction += triggerAction;
    }

    protected override void OnExit()
    {
        //if (!Application.isPlayer)
        //{
        //    return;
        //}
        //actor.GetComponent<Property>().CollisionAction -= triggerAction;
        //foreach (var item in m_colliders)
        //{
        //    Destroy(item);
        //}
    }

#if UNITY_EDITOR  //逻辑绘制

    protected override void OnDrawGizmosSelected()
    {
        if (Application.isPlaying) return;
        Gizmos.color = Color.red;

        foreach (var collider in colliders)
        {
            //绘制collider 到屏幕上

            switch (collider.Type)
            {
                //根据形状不同去绘制

                case Shape.ShapeType.Box:
                    //绘制在 演员所在位置
                    var boxCenter = actor.transform.position + new Vector3(collider.offset.x, collider.offset.y, collider.offset.z) +
                                           new Vector3(0, collider.size.y * 0.5f, 0);
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

    [System.Serializable]
    public class Shape
    {
        public enum ShapeType
        {
            Box//, Circle
        }

        [LabelText("碰撞盒类型")]
        public ShapeType Type;

        [LabelText("尺寸/角度,半径,高度")]
        public Vector3 size = new Vector3();

        [LabelText("偏移")]
        public Vector3 offset = new Vector3();
    }
}