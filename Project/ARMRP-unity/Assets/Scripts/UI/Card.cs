using Battle.UI;
using UnityEngine;

public class Card
{
    public bool isEnable = false;
    private GameObject cardObj;
    private float lerpSpeed = 2f;

    private float LerpTime = 0;

    private float y;

    public Card(int cardId, GameObject parent)
    {
        //读取Json,获取ID;
        cardObj = GameObject.Instantiate(UIManager.Instate.CreateGizmos("Card"), parent.transform);

        //error 通过ID 读取图片路径，替换
        //读取描述
        //增加按下事件 buff
    }

    public void Hide()
    {
        cardObj.SetActive(false);
        isEnable = false;
    }

    public void Show()
    {
        cardObj.SetActive(true);
        y = cardObj.transform.rotation.eulerAngles.y;
        isEnable = true;
    }
    public void Update()
    {
        LerpTime += lerpSpeed * Time.deltaTime;
        float cury = Mathf.Lerp(y, 0, LerpTime);
        cardObj.transform.rotation = Quaternion.Euler(new Vector3(0, cury, 0));
        //cardObj.transform.rotation = Quaternion.EulerAngles(new Vector3(0, cury, 0)); 已过时不要使用
    }
}