using LitJson;
using System.Threading.Tasks;

namespace Battle
{
    /// <summary>
    /// errod 需要Json 生成时自动生成（同步字段增加/修改）
    /// </summary>
    public class CardConfig
    {

        public const string CONFIG_NAME = "card";

        public static JsonData GetCard(int id)
        {
            //尝试直接通过实体读取

            var buffJson = JsonConfig.Read(CONFIG_NAME)[id.ToString()];
            return buffJson;
        }

        /// <summary>
        /// 名称 string
        /// </summary>
        public readonly static string ID = "ID";
        public readonly static string IconPath = "IconPath";
        public readonly static string Description = "Description";
        public readonly static string CardType = "CardType";
        public readonly static string BuffID = "BuffID";
    }
}