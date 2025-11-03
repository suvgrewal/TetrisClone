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

    float m_dropInterval = 0.3f;

    float m_timeToDrop = 0f;

    //float m_timeToNextKey = 0f;
    //[Range(0.02f, 1.0f)]
    //public float m_keyRepeatRate = 0.25f;

    float lowerTimeBound = 0.02f;
    float upperTimeBound = 1.0f;

    float m_timeToNextKeyLeftRight = 0f;
    [Range(0.02f, 1.0f)]
    public float m_keyRepeatRateLeftRight = 0.25f;

    float m_timeToNextKeyDown = 0f;
    [Range(0.02f, 1.0f)]
    public float m_keyRepeatRateDown = 0.02f;

    float m_timeToNextKeyRotate = 0f;
    [Range(0.02f, 1.0f)]
    public float m_keyRepeatRateRotate = 0.20f;

    bool m_gameOver = false;

    public GameObject m_gameOverPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        //m_spawner   = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        m_gameBoard = GameObject.FindObjectOfType<Board>();
        m_spawner   = GameObject.FindObjectOfType<Spawner>();

        m_keyRepeatRateLeftRight = ClampRepeatRate(m_keyRepeatRateLeftRight, lowerTimeBound, upperTimeBound);
        m_keyRepeatRateDown      = ClampRepeatRate(m_keyRepeatRateDown, lowerTimeBound, upperTimeBound);
        m_keyRepeatRateRotate    = ClampRepeatRate(m_keyRepeatRateRotate, lowerTimeBound, upperTimeBound);

        m_timeToNextKeyLeftRight = Time.time;
        m_timeToNextKeyDown      = Time.time;
        m_timeToNextKeyRotate    = Time.time;
        
        if (!m_gameBoard)
        {
            Debug.LogWarning("WARNING! There is no game board defined!");
        }
        if (!m_spawner)
        {
            Debug.LogWarning("WARNING! There is no spawner defined!");
        }
        else
        {
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);


            if (m_activeShape == null)
            {
                m_activeShape = m_spawner.SpawnShape();
            }

        }

        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(false);
        }
    }

    void PlayerInput()
    {
        if (Input.GetButton("MoveRight") && (Time.time > m_timeToNextKeyLeftRight) || Input.GetButtonDown("MoveRight"))
        {
            m_activeShape.MoveRight();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveLeft();
            }
        }

        else if (Input.GetButton("MoveLeft") && (Time.time > m_timeToNextKeyLeftRight) || Input.GetButtonDown("MoveLeft"))
        {
            m_activeShape.MoveLeft();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveRight();
            }
        }
        
        else if (Input.GetButtonDown("Rotate") && (Time.time > m_timeToNextKeyRotate))
        {
            m_activeShape.RotateRight();
            m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.RotateLeft();
            }
        }

        else if (Input.GetButton("MoveDown") && (Time.time > m_timeToNextKeyDown) || Input.GetButtonDown("MoveDown"))
        {
            m_activeShape.MoveDown();
            m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveUp();
            }
        }

        else if (Input.GetButton("MoveDown") && (Time.time > m_timeToNextKeyDown) || (Time.time > m_timeToDrop))
        {
            m_timeToDrop = Time.time + m_dropInterval;
            m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;

            m_activeShape.MoveDown();

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                if (m_gameBoard.IsOverLimit(m_activeShape))
                {
                    GameOver();
                }
                else
                {
                    LandShape();
                }                    
            }
        }
    }

    float ClampRepeatRate   (float value, float lowerBound, float upperBound)
    {
        return Mathf.Clamp(value, lowerBound, upperBound);
    }

    void LandShape()
    {
        // grab next shape
        m_timeToNextKeyLeftRight = Time.time;
        m_timeToNextKeyDown = Time.time;
        m_timeToNextKeyRotate = Time.time;

        m_activeShape.MoveUp();
        m_gameBoard.StoreShapeInGrid(m_activeShape);
        m_activeShape = m_spawner.SpawnShape();

        m_gameBoard.ClearAllRows();
    }

    void GameOver()
    {
        m_activeShape.MoveUp();
        m_gameOver = true;
        Debug.LogWarning(m_activeShape.name + " is over the limit");


        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(true);
        }
    }

    public void Restart()
    {
        Debug.Log("Restarted");
        Application.LoadLevel(Application.loadedLevel); // reloading already loaded level
    }

    // Update is called once per frame
    void Update()
    {
        // do not run if there is not a gameboard or spawner
        if (!m_gameBoard || !m_spawner || !m_activeShape || m_gameOver)
        {
            return;
        }

        PlayerInput();
    }
}
