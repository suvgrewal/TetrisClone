using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    // reference to board
    Board m_gameBoard;

    // reference to spawner
    Spawner m_spawner;

    // currently active shape
    Shape m_activeShape;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        //m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        m_gameBoard = GameObject.FindObjectOfType<Board>();
        m_spawner = GameObject.FindObjectOfType<Spawner>();

        if (m_spawner)
        {
            if (m_activeShape == null)
            {
                m_activeShape = m_spawner.SpawnShape();
            }

            Vector3 offset = new Vector3(0.5f, 0.0f, 0.0f); // 0.5 offset in x for proper alignment
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position) + offset;
        }

        if (!m_gameBoard)
        {
            Debug.LogWarning("WARNING! There is no game board defined!");
        }
        if (!m_spawner)
        {
            Debug.LogWarning("WARNIGN! There is no spawner defined!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // do not run if there is not a gameboard or spawner
        if (!m_gameBoard) || !m_spawner)
        {
            
        }
    }
}
