using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour
{
    public Transform m_emptySprite;
    public int m_height = 30;
    public int m_width = 10;

    public int m_header = 8;

    Transform[,] m_grid;

    void Awake()
    {
        m_grid = new Transform[m_width, m_height];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DrawEmptyCells();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DrawEmptyCells()
    {
        if (m_emptySprite != null)
        {
            // center board offsets
            float offsetX = -(m_width - 1) / 2f;
            float offsetY = -(m_height - 1) / 2f;

            offsetY += m_height / 12f;
            
            for (int y = 0; y < m_height - m_header; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    Vector3 position = new Vector3(x + offsetX, y + offsetY, 0);
                    Transform clone;

                    clone = Instantiate(m_emptySprite, position, Quaternion.identity) as Transform;
                    clone.name = $"Board Space (x={x}, y={y})";
                    clone.transform.parent = transform; // hide in hierarchy
                }
            }
        }
        else
        {
            Debug.Log("WARNING! Please assign the emptySprite object!");
        }
    }
}
