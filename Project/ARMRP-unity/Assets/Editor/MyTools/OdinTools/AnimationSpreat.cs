using System.IO;
using UnityEditor;
using UnityEngine;

public class AnimationSpreat : AssetPostprocessor
{
    private void OnPreprocessModel()
    {
        if (assetPath.Contains(ToolsSettings.Instance.Mark))
        {
            ModelImporter modelImporter = assetImporter as ModelImporter;
            modelImporter.materialImportMode = ModelImporterMaterialImportMode.None;
            //动画禁止材质导入
            modelImporter.materialImportMode = ModelImporterMaterialImportMode.None;
            //设置为Humannoid
            modelImporter.animationType = ModelImporterAnimationType.Human;
            //创建Avata
            modelImporter.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
        }
    }

    private void OnPostprocessModel(GameObject g)
    {
        string name = g.name;
        Debug.Log("动画资源分离" + assetPath + "   ");
        EditorApplication.delayCall += () =>
        {
            if (assetPath.Contains(ToolsSettings.Instance.Mark))
            {
                
              
                //copy 动画
                var assets = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
                //处理过的资源不带标记
                name = name.Split(ToolsSettings.Instance.Mark)[1];
                //copy Avata
                foreach (var obj in assets)
                {
                    Debug.Log("FBX中的资源" + obj.name);

                    if (obj is AnimationClip)
                    {
                        var newClip = UnityEngine.Object.Instantiate(obj);
                        AssetDatabase.CreateAsset(newClip, ToolsSettings.Instance.Anim + "/" + name + ".anim");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }

                    if (obj is Avatar)
                    {
                        var newAvatar = UnityEngine.Object.Instantiate(obj);
                        AssetDatabase.CreateAsset(newAvatar, ToolsSettings.Instance.Avatarfolder + "/" + name + ".asset");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }

                //move fbx
                //error 检测目标目录是否有同名资源  如果有终止操作并报错
                if (assetPath != ToolsSettings.Instance.FBXfolder)
                {
                    Debug.Log("移动资源" + assetPath);
                    //去除标记，防止对资源多次操作
                    string path = Path.GetFileName(assetPath).Replace(ToolsSettings.Instance.Mark,"");
                    AssetDatabase.MoveAsset(assetPath, ToolsSettings.Instance.FBXfolder + "/" + path);
                }
            }
            else
            {
                Debug.Log("导入模型，创建控制器" + assetPath);
            }
        };
    }
}
