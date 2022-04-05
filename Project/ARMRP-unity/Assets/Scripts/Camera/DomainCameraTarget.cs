using UnityEngine;

namespace Drg.Battle
{
    public class DomainCameraTarget : MonoBehaviour
    {
        #region Field

        Transform _trans;
        Transform _target;
        Vector3 _position;

        public Transform Target
        {
            get { return _target; }
            set
            {
                _target = value;
                UpdatePosByTarget();
            }
        }

        public void UpdatePosByTarget()
        {
            if (null == _target) return;

            var position = _target.position;
            _position.x = position.x;
            _position.z = position.z;
            _trans.position = _position;
        }
        #endregion


        #region Unity Event

        protected void Awake()
        {
            _trans = transform;
            _position = _trans.position;
        }

        protected void Update()
        {
            UpdatePosByTarget();
        }

        #endregion
    }
}