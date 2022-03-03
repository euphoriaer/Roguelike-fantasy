using System.Collections.Generic;
using UnityEngine;

namespace ArtTest
{
    [ExecuteAlways]
    public class DepthObject : MonoBehaviour
    {
        LightShadowMap _lightShaowMap;
        LightShadowMap LightShadowMap
        {
            get
            {
                if (null == _lightShaowMap)
                {
                    _lightShaowMap = FindObjectOfType<LightShadowMap>();
                }

                return _lightShaowMap;
            }
        }

        List<Material> _mats;
        List<Material> Mats
        {
            get
            {
                if (null == _mats)
                {
                    _mats = new List<Material>();

                    var renderers = transform.GetComponentsInChildren<Renderer>();
                    foreach (var renderer in renderers)
                    {
                        _mats.AddRange(renderer.sharedMaterials);
                    }
                }

                return _mats;
            }
        }

        private void Update()
        {
            if (null == LightShadowMap) return;
            if (null == Mats) return;

            Vector3 dir = -LightShadowMap.transform.forward;
            foreach (var mat in Mats)
            {
                mat.SetVector("_LightDirection", dir);
            }
        }
    }
}