using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MapManager : MonoBehaviour
{
    [SerializeField] GameObject _nodePrefab;
    [SerializeField] GameObject _linePrefab;
    [SerializeField] Transform _mapContainer;
    [SerializeField] List<List<NodeDisplay>> _nodeDisplayMap;
    [SerializeField] Scrollbar _scroll;
    List<List<Node>> _map;
    public void DrawMap(List<List<Node>> map)
    {
        _map = map;
        _nodeDisplayMap = new List<List<NodeDisplay>>();
        for (int i = 0; i < map.Count; i++)
        {
            _nodeDisplayMap.Add(new List<NodeDisplay>());
            for (int j = 0; j < map[i].Count; j++)
            {
                if (map[i][j] == null)
                {
                    _nodeDisplayMap[i].Add(null);
                    continue;
                }
                var go = Instantiate(_nodePrefab, _mapContainer);
                go.transform.localPosition = map[i][j].Position;
                var display = go.GetComponent<NodeDisplay>();
                display.Setup(this, map[i][j]);
                _nodeDisplayMap[i].Add(display);
            }
        }


        for (int i = 0; i < _nodeDisplayMap.Count; i++)
        {
            for (int j = 0; j < _nodeDisplayMap[i].Count; j++)
            {
                if (_nodeDisplayMap[i][j] != null && _nodeDisplayMap[i][j].Node.Children.Count != 0)
                {
                    foreach (var node in _nodeDisplayMap[i][j].Node.Children)
                    {
                        Line.DrawLines(_linePrefab, _nodeDisplayMap[i][j], _nodeDisplayMap[node.Row][node.Column]);
                    }
                }
            }
        }

        _scroll.value = 0;
        var room = GameManager.Instance.GetCurrentRoom();
        for (int i = 0; i < room.Children.Count; i++)
        {
            var child = room.Children[i];
            _nodeDisplayMap[child.Row][child.Column].Open();
        }
    }

    public void RunNode(int row, int column)
    {
        foreach (var room in _nodeDisplayMap[row])
        {
            if (room != null)
                room.Close();
        }
        GameManager.Instance.VisitFloor(row, column);
    }

    public void ProceedFrom(Node node)
    {
        foreach (var child in node.Children)
        {
            _nodeDisplayMap[child.Row][child.Column].Open();
        }
    }

    public void ClearMap()
    {
        foreach (var floor in _nodeDisplayMap)
        {
            foreach (var room in floor)
            {
                if (room != null)
                    Destroy(room.gameObject);
            }
        }
    }

    public void ResetMap()
    {
        foreach (var floor in _nodeDisplayMap)
        {
            foreach (var node in floor)
            {
                if (node != null)
                    Destroy(node.gameObject);
            }
        }
        _nodeDisplayMap.Clear();
    }
}
