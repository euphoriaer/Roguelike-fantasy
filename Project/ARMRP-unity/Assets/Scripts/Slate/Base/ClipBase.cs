using System;
using Slate.ActionClips;
using UnityEngine;

namespace Assets.Scripts.Slate.Base
{
    [Serializable]
    public class ClipBase<T> : ActorActionClip where T : Component
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

        public override bool isValid
        {
            get { return actor != null && base.isValid; }
        }
    }
}