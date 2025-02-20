using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ContentsAutoCell : MonoBehaviour
{
    public ScrollRect scv;

    public int spacing = 5;

    public void AutoSizeExpand()
    {
        scv.content.sizeDelta = new Vector2(scv.content.rect.width, scv.content.childCount * (scv.content.GetChild(0).GetComponent<RectTransform>().rect.height + spacing));
    }

    [ContextMenu("ReCelling")]
    public void AutoCelling()
    {
        if (scv == null) return;
        
        AutoSizeExpand();
        RectTransform scvRect = scv.GetComponent<RectTransform>();

        if (scv.horizontal && scv.vertical)
        {
            int countX = (int)(this.GetComponent<RectTransform>().rect.width / scv.content.GetChild(0)?.GetComponent<RectTransform>().rect.width);

            for (int i = 0; i < scv.content.childCount; i++)
            {
                RectTransform child = scv.content.GetChild(i).GetComponent<RectTransform>();
                child.localPosition = new Vector2(child.rect.width * (i % countX) + (spacing * (i + 1)), (child.rect.height * (int)(i % countX) + (spacing * ((int)(i % countX) + 1))) * -1);
            }
        }

        else if (scv.horizontal)
        {
            for (int i = 0; i < scv.content.childCount; i++)
            {
                RectTransform child = scv.content.GetChild(i).GetComponent<RectTransform>();
                child.localPosition = new Vector2((child.rect.width * i) + (spacing * (i + 1)), (scvRect.rect.height - child.rect.height) * 0.5f);
            }
        }

        else if (scv.vertical)
        {
            for (int i = 0; i < scv.content.childCount; i++)
            {
                RectTransform child = scv.content.GetChild(i).GetComponent<RectTransform>();
                child.localPosition = new Vector2((scvRect.rect.width - child.rect.width) * 0.5f, ((child.rect.height * i) + (spacing * (i + 1))) * -1);
            }
        }
    }
}
