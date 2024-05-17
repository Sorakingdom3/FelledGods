using UnityEngine;

public class Line : MonoBehaviour
{

    public static void DrawLines(GameObject linePrefab, NodeDisplay parent, NodeDisplay child)
    {
        var go = Instantiate(linePrefab, parent.transform);

        var transform = go.GetComponent<RectTransform>();
        transform.up = child.GetComponent<RectTransform>().position - parent.GetComponent<RectTransform>().position;
        transform.localPosition = Vector3.zero + transform.up * 50;
        transform.sizeDelta = new Vector2(transform.sizeDelta.x, Vector2.Distance(child.GetComponent<RectTransform>().position, transform.position) - 50);
    }
}
