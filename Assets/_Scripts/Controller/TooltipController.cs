using TMPro;
using UnityEngine;

public class TooltipController : MonoBehaviour
{
    [SerializeField] RectTransform _canvasRectTransform;
    [SerializeField] RectTransform _backgroundRectTransform;
    [SerializeField] RectTransform _rectTransform;
    [SerializeField] TextMeshProUGUI _textMeshPro;

    public static TooltipController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        Instance = this;
    }

    void SetText(string text)
    {
        _textMeshPro.SetText(text);
        _textMeshPro.ForceMeshUpdate();

        Vector2 textSize = _textMeshPro.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8, 8);
        _backgroundRectTransform.sizeDelta = textSize + paddingSize;
        HideTooltip();
    }

    private void Update()
    {
        Vector2 anchoredPosition = Input.mousePosition / _canvasRectTransform.localScale.x;
        if (anchoredPosition.x + _backgroundRectTransform.rect.width > _canvasRectTransform.rect.width)
        {
            anchoredPosition.x = _canvasRectTransform.rect.width - _backgroundRectTransform.rect.width;
        }
        if (anchoredPosition.y + _backgroundRectTransform.rect.height > _canvasRectTransform.rect.height)
        {
            anchoredPosition.y = _canvasRectTransform.rect.height - _backgroundRectTransform.rect.height;
        }
        _rectTransform.anchoredPosition = anchoredPosition;
    }

    public static void ShowTooltip_Static(string txt)
    {
        Instance.ShowTooltip(txt);
    }

    private void ShowTooltip(string txt)
    {
        gameObject.SetActive(true);
        SetText(txt);
    }
    public static void HideTooltip_Static()
    {
        Instance.HideTooltip();
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
