using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

public class ToolsMain : OdinMenuEditorWindow
{
    [MenuItem("Tools/我的工具箱")]
    private static void OpenWindow()
    {
        var window = GetWindow<ToolsMain>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 500);
    }
    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();
        tree.Add("设置", ToolsSettings.Instance, EditorIcons.SettingsCog);
        tree.Add("一键打包工具", OneKeyBuildlEditor.Instance, EditorIcons.SmartPhone);
        return tree;
    }
  
}
