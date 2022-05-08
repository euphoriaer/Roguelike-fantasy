using Battle;
using UI;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{

    //加载资源管理。

    //加载UI

    //场景加载/卸载
    public void Start()
    {
        UISystem uISystem = new UISystem();
        uISystem.CreatePanel<BottomPanel>("");
    }
}