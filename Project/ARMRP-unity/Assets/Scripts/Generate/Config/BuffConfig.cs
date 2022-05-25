using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle
{
    /// <summary>
    /// errod 需要Json 生成时自动生成（同步字段增加/修改）
    /// </summary>
    public class BuffConfig
    {

        public const string CONFIG_NAME = "buff";

        public static JsonData GetBuff(int id)
        {
            //error 尝试直接通过实体读取,即生成代码时，直接生成实体类

            var buffJson = JsonConfig.Read(CONFIG_NAME)[id.ToString()];
            return buffJson;
        }

        /// <summary>
        /// 名称 string
        /// </summary>
        public readonly static string ID = "ID";
        public readonly static string NAME = "name";
        public readonly static string TIME = "Time";
        public readonly static string TYPE = "Type";
        public readonly static string EFFECT = "Effect";
    }
}
