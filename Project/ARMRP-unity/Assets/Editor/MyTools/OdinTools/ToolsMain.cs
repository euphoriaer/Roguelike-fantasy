using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;

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
        tree.DefaultMenuStyle.SetHeight(80);
        tree.DefaultMenuStyle.IconSize = 80;
        return tree;
    }
}