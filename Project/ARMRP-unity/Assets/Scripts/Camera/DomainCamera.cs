using UnityEngine;

namespace MyEditor
{
    public class DomainCamera : MonoBehaviour
    {
        #region Field

        [SerializeField] private Vector2 _xRange = new Vector2(-8.5f, 11.3f);
        [SerializeField] private Vector2 _zRange = new Vector2(-56f, -12.4f);

        private Transform _trans;
        private Transform _target;
        private Camera _camera;
        private float _cameraSize;

        public Transform Target
        {
            get { return _target; }
            set
            {
                _target = value;
            }
        }

        public Vector2 XRange
        { get { return _xRange; } set { _xRange = value; } }
        public Vector2 ZRange
        { get { return _zRange; } set { _zRange = value; } }

        public float CameraSize
        { get { return _cameraSize; } }

        #endregion Field

        #region Public

        public void InitCameraPos(Vector3 targetPos)
        {
            if (_trans == null) _trans = transform;
            _trans.position = ValidTargetPos(targetPos);
            UnityEngine.Debug.LogWarning($"<size=15> ---->初始化相机的位置：_trans.position={_trans.position}  targetPos={targetPos}</size>");
        }

        public void Update()
        {
            if (null == _target) return;

            //CriAudioManager.Instance.Update(_trans.position);
        }

        public void SetCameraSize(float size)
        {
            _cameraSize = size;
            if (null == _camera) _camera = GetComponent<Camera>();
            _camera.orthographicSize = size;
        }

        private Vector3 ValidTargetPos(Vector3 targetPos)
        {
            if (_trans == null) _trans = transform;
            var pos = targetPos - 50 * _trans.forward;
            pos.x = Mathf.Clamp(pos.x, _xRange.x, _xRange.y);
            pos.z = Mathf.Clamp(pos.z, _zRange.x, _zRange.y);
            return pos;
        }

        #endregion Public

        #region Unity Event

        public static int ROLELAYER;
        public static int ROLE2LAYER;
        public static int DEFAULTLAYER;

        protected void Awake()
        {
            _trans = transform;
            _camera = GetComponent<Camera>();
            _cameraSize = _camera.orthographicSize;
            ROLELAYER = LayerMask.NameToLayer("Role");
            ROLE2LAYER = LayerMask.NameToLayer("Role2");
            DEFAULTLAYER = LayerMask.NameToLayer("Default");
        }

        public Camera GetMainCamera()
        {
            return _camera;
        }

        protected void LateUpdate()
        {
            if (null == _target) return;

            var pos = ValidTargetPos(_target.position);
            _trans.position = Vector3.Lerp(_trans.position, pos, 8 * Time.deltaTime);
        }

        #endregion Unity Event
    }
}