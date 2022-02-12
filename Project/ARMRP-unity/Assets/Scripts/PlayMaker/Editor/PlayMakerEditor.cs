using Assets.Scripts.PlayMaker.Action;
using HutongGames.PlayMaker;
using HutongGames.PlayMakerEditor;
using Sirenix.OdinInspector.Editor;

[CustomActionEditor(typeof(PlayCutsceneRole))]
public class PlayMakerEditor : CustomActionEditor
{
    PropertyTree tree;
    public override void OnEnable()
    {
        tree = PropertyTree.Create(target);
    }


    public override bool OnGUI()
    {
        tree.Draw(false);
        return true;
    }
}