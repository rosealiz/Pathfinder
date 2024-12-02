using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    [SerializeField] Text deDisplay;
    [SerializeField] Text paraDisplay;
    [SerializeField] Text infoDisplay;

    Pathfinder<Node> map;
    Node para, de;
    Vector3[] linePath;

    void Start()
    {
        Node.OnClicked += HandleNodeClicked;

        var nodes = GameObject.FindObjectsOfType<Node>();
        var connections = new Dictionary<Node, HashSet<Node>>();
        foreach (var node in nodes)
        {
            connections.Add(node, new HashSet<Node>(node.Neighbors));
        }
        map = new Pathfinder<Node>(connections);
    }

    void HandleNodeClicked(Node node)
    {
        switch (de, para)
        {
            case (null, null):
                de = node;
                break;
            case ({ }, null):
                if (de == node) break;
                para = node;
                var path = map.FindPath(de, para);
                if (path == null) break;
                linePath = path.Select(n => n.Position).ToArray();
                line.positionCount = linePath.Length;
                line.SetPositions(linePath);
                break;
            case ({ }, { }):
                de = node;
                para = null;
                linePath = null;
                break;
        }
        line.enabled = de != null && para != null && linePath != null;
        paraDisplay.text = para?.name ?? string.Empty;
        deDisplay.text = de?.name ?? string.Empty;
        infoDisplay.text = linePath == null && de != null && para != null ? "caminho nao e possivel" : string.Empty;
    }
}
