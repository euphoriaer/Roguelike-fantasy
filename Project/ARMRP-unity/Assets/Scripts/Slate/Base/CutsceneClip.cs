using Slate;
using Slate.ActionClips;
using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public abstract class CutsceneClip<T> : CutsceneClipBase, IRefresh, IClipRefresh where T : Component
{
    private T _actorComponent;

    public T ActorComponent
    {
        get
        {
            if (_actorComponent != null && _actorComponent.gameObject == base.actor)
            {
                return _actorComponent;
            }

            return _actorComponent = base.actor != null ? base.actor.GetComponent<T>() : null;
        }
    }

    public abstract void Refresh();

    public override bool isValid
    {
        get { return actor != null && base.isValid; }
    }

}

public class CutsceneClipBase:ActorActionClip
{
    [LabelText("Clip片段名")]
    public string CutsceneClipName;
}

public interface IRefresh
{
    public void Refresh();
}

public interface IClipRefresh
{
    public void Refresh();
}