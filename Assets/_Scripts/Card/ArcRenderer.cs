using System.Collections.Generic;
using UnityEngine;

public class ArcRenderer : MonoBehaviour
{

    public GameObject ArrowPrefab;
    public GameObject DotPrefab;
    public int PoolSize = 50;
    List<GameObject> DotPool = new List<GameObject>();
    GameObject ArrowInstance;

    public float spacing = 50;
    public float arrowAngleAdjustment = 0;
    public int dotsToSkip = 1;
    private Vector3 arrowDirection;

    private void Start()
    {
        ArrowInstance = Instantiate(ArrowPrefab, transform);
        ArrowInstance.transform.localPosition = Vector3.zero;
        IntializeDotPool(PoolSize);
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0;

        Vector3 startPosition = transform.position;
        Vector3 midPosition = CalculateMiddlePoint(startPosition, mousePosition);

        UpdateArc(startPosition, midPosition, mousePosition);
        PositionAndRotateArrow(mousePosition);

    }
    private void IntializeDotPool(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            var dot = Instantiate(DotPrefab, Vector3.zero, Quaternion.identity, transform);
            dot.SetActive(false);
            DotPool.Add(dot);
        }
    }

    private Vector3 CalculateMiddlePoint(Vector3 start, Vector3 end)
    {
        Vector3 middlePoint = (start + end) / 2;
        float arcHeight = Vector3.Distance(start, end) / 3f;
        middlePoint.y += arcHeight;
        return middlePoint;
    }
    private void UpdateArc(Vector3 start, Vector3 mid, Vector3 end)
    {
        int numDots = Mathf.CeilToInt(Vector3.Distance(start, end) / spacing);
        for (int i = 0; i < numDots && i < DotPool.Count; i++)
        {
            float t = i / (float)numDots;
            t = Mathf.Clamp(t, 0f, 1f);
            Vector3 position = QuadraticBezierPoint(start, mid, end, t);

            if (i != numDots - dotsToSkip)
            {
                DotPool[i].transform.position = position;
                DotPool[i].SetActive(true);
            }
            if (i == numDots - (dotsToSkip + 1) && i - dotsToSkip + 1 >= 0)
            {
                arrowDirection = DotPool[i].transform.position;
            }
        }
        for (int i = numDots - dotsToSkip; i < DotPool.Count; i++)
        {
            if (i > 0)
            {
                DotPool[i].SetActive(false);
            }
        }
    }
    private Vector3 QuadraticBezierPoint(Vector3 start, Vector3 control, Vector3 end, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 point = uu * start;
        point += 2 * u * t * control;
        point += tt * end;
        return point;
    }
    private void PositionAndRotateArrow(Vector3 position)
    {
        ArrowInstance.transform.position = position;
        Vector3 direction = arrowDirection - position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += arrowAngleAdjustment;
        ArrowInstance.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
