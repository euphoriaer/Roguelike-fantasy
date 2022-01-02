using Slate.ActionClips;
using System;
using UnityEngine;


    [Serializable]
    public abstract class CutsceneClip<T> : ActorActionClip, ClipRefresh where T : Component
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

    public interface ClipRefresh
    {
        public void Refresh();
    }
