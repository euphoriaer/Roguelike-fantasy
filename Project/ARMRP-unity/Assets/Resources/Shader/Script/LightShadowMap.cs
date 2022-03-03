using System.Collections.Generic;
using UnityEngine;

namespace ArtTest
{
    [ExecuteAlways]
    public class LightShadowMap : MonoBehaviour
    {
        #region Field

        [SerializeField] Material _depthMaterial = null;
        [SerializeField] int _renderTextureSize = 1024;

        RenderTexture _renderTexture;
        GameObject[] _renderObjects;

        Camera _camera;
        List<Renderer> _renderers = new List<Renderer>();

        #endregion


        #region Unity Event

        private void Awake()
        {
            _camera = GetComponent<Camera>();

            DepthObject[] depthObjects = FindObjectsOfType<DepthObject>();
            _renderObjects = new GameObject[depthObjects.Length];
            for (int i = 0; i < depthObjects.Length; ++i)
            {
                _renderObjects[i] = depthObjects[i].gameObject;
            }
        }

        private void Start()
        {
            SetRender(_renderObjects, _renderTextureSize);
        }

        void OnDestroy()
        {
            DestorySelf();
        }

        void Update()
        {
            if (_renderers.Count == 0) return;

            Matrix4x4 matVP = GL.GetGPUProjectionMatrix(_camera.projectionMatrix, true) * _camera.worldToCameraMatrix;
            foreach (var renderer in _renderers)
            {
                if (renderer == null) continue;

                foreach (var mat in renderer.sharedMaterials)
                {
                    mat.SetMatrix("_ShadowMatrix", matVP);
                    mat.SetTexture("_ShadowTexture", _renderTexture);
                }
            }
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (_depthMaterial != null)
            {
                Graphics.Blit(source, destination, _depthMaterial);
            }
            else
            {
                Graphics.Blit(source, destination);
            }
        }

        #endregion


        #region Function

        void SetRender(GameObject[] renderObjects, int size)
        {
            DestorySelf();

            if (_camera != null)
            {
                _camera.GetComponent<Camera>().enabled = true;
                _camera.depthTextureMode = DepthTextureMode.Depth;

                if (_renderTexture == null)
                {
                    _renderTexture = new RenderTexture(size, size, 0, RenderTextureFormat.ARGB32);
                    _renderTexture.name = "shadowTexture" + GetInstanceID();
                    _renderTexture.isPowerOfTwo = true;
                    _renderTexture.hideFlags = HideFlags.DontSave;
                }
                _camera.targetTexture = _renderTexture;
            }

            if (null != renderObjects)
            {
                foreach (var renderObj in renderObjects)
                {
                    if (null == renderObj) continue;

                    var renderers = renderObj.GetComponentsInChildren<Renderer>();
                    _renderers.AddRange(renderers);
                }
            }
        }

        void DestorySelf()
        {
            _renderers.Clear();

            if (_renderTexture != null)
            {
                //Destroy(_renderTexture);
                _renderTexture = null;
            }

            if (_camera != null) _camera.enabled = false;
        }

        #endregion
    }
}