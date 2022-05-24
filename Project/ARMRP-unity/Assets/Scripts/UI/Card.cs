using Battle.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card
{
    public bool isEnable = false;
    private GameObject cardObj;

    private Image icon;
    private TextMeshProUGUI text;
    private Button btn;
    
    private float lerpSpeed = 2f;

    private float LerpTime = 0;
    private int[] Text = new int[5];
    private float y;

    public Card(int cardId, GameObject parent)
    {
        //读取Json,获取ID;
        cardObj = GameObject.Instantiate(UIManager.Instate.CreateGizmos("Card"), parent.transform);
        icon= cardObj.transform.GetChild(1).GetComponent<Image>();
        text = cardObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        btn= cardObj.transform.GetChild(3).GetComponent<Button>();
            
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