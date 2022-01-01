using Assets.Scripts.Slate.Base;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Slate
{
    public partial class CutsceneEditor : EditorWindow
    {
        /// <summary>
        /// 自定义按键功能
        /// </summary>
        private void CustomButton()
        {
            #region 自定义功能

            if (Prefs.timeStepMode != Prefs.TimeStepMode.Frames)
            {
                Prefs.timeStepMode = Prefs.TimeStepMode.Frames;
                Prefs.frameRate = 60; //TODO:因为默认是30，所以只有赋值60和默认值不同才能触发相关的操作
                Prefs.frameRate = 30;
            }

            //GUI.color = Color.red;
            if (GUILayout.Button("保存文件", EditorStyles.toolbarButton, GUILayout.Width(60)))
            {
                PropertyModification[] info = PrefabUtility.GetPropertyModifications(cutscene);
                PrefabUtility.ApplyPrefabInstance(cutscene.gameObject, InteractionMode.AutomatedAction);
                if (cutscene.transform.childCount > 0)
                {
                    cutscene.transform.GetChild(0).gameObject.hideFlags = HideFlags.HideInHierarchy;
                }
            }

            if (GUILayout.Button("刷新", EditorStyles.toolbarButton, GUILayout.Width(60)))
            {
                CutsceneEditorHelper.Refresh(cutscene);
            }
            //GUI.color = Color.white;

            #endregion 自定义功能
        }
    }

    public static class CutsceneEditorHelper
    {
        public static void Refresh(Cutscene cutscene)
        {
            //通过cutscene 对象找到所有的Clips，调用带有ClipRefresh 接口的函数
            foreach (var group in cutscene.groups)
            {
                foreach (var track in group.tracks)
                {
                    var clips = track.clips.ToList();

                    for (int i = 0; i < clips.Count; i++)
                    {
                        var curClip = clips[i];
                        var curPlayAnimClip = curClip as ClipRefresh;
                        curPlayAnimClip.Refresh();
                    }
                }
            }
        }
    }
}