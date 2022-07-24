using System;
using Sirenix.OdinInspector;
using Slate;
using UnityEngine;

[Serializable]
public class HitBox
{
    [HideInInspector] public GameObject Source;
    [HideInInspector] public GameObject Target;

    public enum PlayCutsceneTarget
    {
        [LabelText("自身")] Self = 0,
        [LabelText("被击者")] Behit = 1,
    }

    [LabelText("播放位置")] 
    public PlayCutsceneTarget FxCutsceneTarget;

    [LabelText("特效Cutscene")]
    public Cutscene FxCutscene;
}
