using Battle.Resource;
using LitJson;
using UnityEngine;

namespace Battle
{
    public static class JsonConfig
    {
        /// <summary>
        /// 需通过键值对 再提取，Key必须是 string
        /// </summary>
        /// <param name="JsonName"></param>
        /// <returns></returns>
        public static JsonData Read(string JsonName)
        {
            string path = "Data/Json/" + JsonName;
            var jsonText = ResourceManager.Instate.Load<TextAsset>(path).text;
            var jsonData = JsonMapper.ToObject(jsonText);
            return jsonData;
        }

        /// <summary>
        /// 直接读取到实体，不需要再解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="JsonName"></param>
        /// <returns></returns>
        public static T Read<T>(string JsonName)
        {
            string path = "Data/Json/" + JsonName;
            var jsonText = ResourceManager.Instate.Load<TextAsset>(path).text;
            var jsonData = JsonMapper.ToObject<T>(jsonText);
            return jsonData;
        }
    }
}