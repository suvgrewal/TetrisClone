using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    // reference to board
    Board m_gameBoard;

    // reference to spawner
    Spawner m_spawner;

    // reference to sound manager
    SoundManager m_soundManager;

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

    public GameObject m_gameOverPanel;

    bool m_gameOver = false;

    public IconToggle m_rotIconToggle;

    bool m_clockwise = true;

    public bool m_isPaused = false;

    public GameObject m_pausePanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //m_gameBoard      = GameObject.FindWithTag("Board").GetComponent<Board>();
        //m_spawner        = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        //m_soundManager   = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();

        m_gameBoard    = GameObject.FindObjectOfType<Board>();
        m_spawner      = GameObject.FindObjectOfType<Spawner>();
        m_soundManager = GameObject.FindObjectOfType<SoundManager>();
        
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
        if (!m_soundManager)
        {
            Debug.LogWarning("WARNING! There is no sound manager defined!");
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

        if (m_pausePanel)
        {
            m_pausePanel.SetActive(false);
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
                PlaySound(m_soundManager.m_errorSound);
            }
            else
            {
                PlaySound(m_soundManager.m_moveSound);
            }
        }

        else if (Input.GetButton("MoveLeft") && (Time.time > m_timeToNextKeyLeftRight) || Input.GetButtonDown("MoveLeft"))
        {
            m_activeShape.MoveLeft();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveRight();
                PlaySound(m_soundManager.m_errorSound);
            }
            else
            {
                PlaySound(m_soundManager.m_moveSound);
            }
        }
        
        else if (Input.GetButtonDown("Rotate") && (Time.time > m_timeToNextKeyRotate))
        {
            m_activeShape.RotateClockwise(m_clockwise);
            m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.RotateClockwise(!m_clockwise);
                PlaySound(m_soundManager.m_errorSound);
            }
            else
            {
                PlaySound(m_soundManager.m_moveSound);
            }
        }

        else if (Input.GetButton("MoveDown") && (Time.time > m_timeToNextKeyDown) || Input.GetButtonDown("MoveDown"))
        {
            m_activeShape.MoveDown();
            m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveUp();
                PlaySound(m_soundManager.m_errorSound);
            }
            else
            {
                PlaySound(m_soundManager.m_moveSound, 1.0f);
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

        else if(Input.GetButtonDown("ToggleRot"))
        {
            ToggleRotDirection();
        }

        else if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }

        else if (Input.GetButtonDown("Restart"))
        {
            Restart();
        }
    }

    float ClampRepeatRate   (float value, float lowerBound, float upperBound)
    {
        return Mathf.Clamp(value, lowerBound, upperBound);
    }

    void LandShape()
    {
        m_activeShape.MoveUp();
        m_gameBoard.StoreShapeInGrid(m_activeShape);

        // grab next shape
        m_activeShape = m_spawner.SpawnShape();
        
        m_timeToNextKeyLeftRight = Time.time;
        m_timeToNextKeyDown = Time.time;
        m_timeToNextKeyRotate = Time.time;

        m_gameBoard.ClearAllRows();

        PlaySound(m_soundManager.m_dropSound);

        if (m_gameBoard.m_completedRows > 0)
        {
            if (m_gameBoard.m_completedRows > 1)
            {
                AudioClip randomVocal = m_soundManager.GetRandomClip(m_soundManager.m_vocalClips);
                PlaySound(randomVocal);
            }

            PlaySound(m_soundManager.m_clearRowSound);
        }
    }

    void GameOver()
    {
        m_activeShape.MoveUp();


        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(true);
        }

        PlaySound(m_soundManager.m_gameOverSound, 5f);
        PlaySound(m_soundManager.m_gameOverVocalClip, 5f);

        m_gameOver = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Application.LoadLevel(Application.loadedLevel); // reloading already loaded level
    }

    // Update is called once per frame
    void Update()
    {
        // do not run if there is not a gameboard or spawner
        if (!m_gameBoard || !m_spawner || !m_activeShape || m_gameOver || !m_soundManager)
        {
            return;
        }

        PlayerInput();
    }

    void PlaySound(AudioClip clip, float volMultiplier = 0.5f)
    {
        if (clip && m_soundManager.m_fxEnabled)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(m_soundManager.m_fxVolume * volMultiplier, 0.05f, 1f));
        }
    }

    public void ToggleRotDirection()
    {
        m_clockwise = !m_clockwise;

        if (m_rotIconToggle)
        {
            m_rotIconToggle.ToggleIcon(m_clockwise);
        }
    }

    public void TogglePause()
    {
        if (m_gameOver)
        {
            return;
        }

        m_isPaused = !m_isPaused;

        if (m_pausePanel)
        {
            m_pausePanel.SetActive(m_isPaused);

            if (m_soundManager)
            {
                m_soundManager.m_musicSource.volume = (m_isPaused) ? m_soundManager.m_musicVolume * 0.25f : m_soundManager.m_musicVolume;
            }

            Time.timeScale = (m_isPaused) ? 0 : 1;
        }
    }
}
