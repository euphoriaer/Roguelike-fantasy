using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

public class ToolsMain : OdinMenuEditorWindow
{
    [MenuItem("Tools/�ҵĹ�����")]
    private static void OpenWindow()
    {
        var window = GetWindow<ToolsMain>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 500);
    }
    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();
        tree.Add("����", ToolsSettings.Instance, EditorIcons.SettingsCog);
        tree.Add("һ���������", OneKeyBuildlEditor.Instance, EditorIcons.SmartPhone);
        return tree;
    }
  
}
