using UnityEngine;
using System.Collections;

public class Ghost : MonoBehaviour
{
    Shape m_ghostShape = null;
    bool m_hitBottom = false;
    public Color m_color = new Color(1f, 1f, 1f, 0.2f);

    public void DrawGhost(Shape originalShape, Board gameBoard)
    {
        m_ghostShape = Instantiate(originalShape, originalShape.transform.postiion, originalShape.transform.rotation) as Shape;
        m_ghostShape.gameObject.name = "GhostShape";

        SpriteRenderer[] allRenderers = m_ghostShape.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer r in allRenderers)
        {
            r.color = m_color;
        }

        m_hitBottom = false;

        while (!m_hitBottom)
        {
            m_ghostShape.MoveDown();
            if (!gameBoard.IsValidPosition(m_ghostShape))
            {
                m_ghostShape.MoveUp();
                m_hitBottom = true;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
