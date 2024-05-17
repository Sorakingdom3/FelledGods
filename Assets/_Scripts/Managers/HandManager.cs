using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public float m_FanSpread;
    public float m_HorizontalSpacing;
    public float m_VerticalSpacing = 100f;
    [SerializeField] Transform HandContainer;
    [SerializeField] GameObject CardPrefab;
    [SerializeField] List<GameObject> Hand;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void DrawCard(CardData data)
    {
        var card = Instantiate(CardPrefab, HandContainer);
        card.GetComponent<CardDisplay>().Setup(data);
        Hand.Add(card);
        UpdateVisuals();
    }

    private void Update()
    {
        //UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        if (Hand.Count == 1)
        {

            Hand[0].transform.localPosition = Vector3.zero;
            Hand[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        for (int i = 0; i < Hand.Count; i++)
        {
            float rotationAngle = (m_FanSpread * (i - (Hand.Count - 1) / 2f));
            Hand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            float horizontalOffset = (m_HorizontalSpacing * (i - (Hand.Count - 1) / 2f));
            float normalizedPosition = (2f * i / (Hand.Count - 1) - 1f);
            float verticalOffset = Mathf.Max(0.01f, m_VerticalSpacing * (1 - normalizedPosition * normalizedPosition));
            Hand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);

        }
    }
}
