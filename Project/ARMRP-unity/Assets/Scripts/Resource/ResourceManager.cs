using UnityEngine;

namespace Battle.Resource
{
    public class ResourceManager
    {
        //资源加载、卸载中枢，可替换加载方式，

        //1.Addressable
        //2.Resource
        //3.Assetbundle
        //4.其余资源管理框架，例如 XAsset 等

        //Demo暂用Resource,方便快捷.可添加切换
        private ResourceLoadMode resourceMode;

        public ResourceLoadMode ResourceMode { get => resourceMode; set => resourceMode = value; }

        public enum ResourceLoadMode
        {
            Resource
        }

        internal static T Load<T>(string path) where T : Object
        {
           return Resources.Load<T>(path);
        }

        internal static ResourceRequest LoadAsync<T>(string path) where T : Object
        {
            return Resources.LoadAsync<T>(path);
        }

        internal static void Unload(Object asset)
        {
            Resources.UnloadAsset(asset);
        }
    }
}