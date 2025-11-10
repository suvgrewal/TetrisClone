using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ScoreManager : MonoBehaviour {
	// number of string elements to put in score values
	int scoreElements = 5;

	//our score
	int m_score = 0;

	// the number of lines we need to get to the next level
	int m_lines;

	// our current level
	public int m_level = 1;

	// base number of lines needed to clear a level
	public int m_linesPerLevel = 5;

	// text component for our Lines UI
	public TextMeshProUGUI m_linesText;

	// text component for our Level UI
	public TextMeshProUGUI m_levelText;

	// text component for our Score UI
	public TextMeshProUGUI m_scoreText;

	// text component for High Score UI
	public TextMeshProUGUI m_highScoreText;

	// minimum number of lines we can clear if we do indeed clear any lines
	const int m_minLines = 1;

	// maximum number of lines we can clear if we do indeed clear any lines
	const int m_maxLines = 4;

	// whether user did level up
	public bool didLevelUp = false;

	// levelUp visual FX to play
	public ParticlePlayer m_levelUpFx;

	// public getter for current score
	public int Score => m_score;

    // integer value of high score
    int m_highScore = 0;

    // add this public setter
    public void SetHighScore(int value)
    {
        m_highScore = value;
        UpdateUIText();
    }


    // update the user interface
    void UpdateUIText()
	{
		if (m_linesText)
		{
			m_linesText.text = m_lines.ToString();
		}

		if (m_levelText)
		{
			m_levelText.text = m_level.ToString();
		}

		if (m_scoreText)
		{
			m_scoreText.text = PadZero(m_score, scoreElements);
		}

		if (m_highScoreText)
		{
			m_highScoreText.text = PadZero(m_highScore, scoreElements);
		}
	}

	// handle scoring
	public void ScoreLines(int n)
	{
		// flag to GameController that we leveled up
		didLevelUp = false;

		// clamp this between 1 and 4 lines
		n = Mathf.Clamp(n,m_minLines,m_maxLines);

		// adds to our score depending on how many lines we clear
		switch (n)
		{
			case 1:
				m_score += 40 * m_level;
				break;
			case 2:
				m_score += 100 * m_level;
				break;
			case 3:
				m_score += 300 * m_level;
				break;
			case 4:
				m_score += 1200 * m_level;
				break;
		}

		// reduce our current number of lines needed for the next level
		m_lines -= n;

		// if we finished our lines, then level up
		if (m_lines <= 0)
		{
			LevelUp();
		}

		// update the user interface
		UpdateUIText();
	}

	// start our level and lines -- in the future we might start at a different level than 1 for increased difficulty
	public void Reset()
	{
		m_level = 1;
		m_lines = m_linesPerLevel * m_level;
		UpdateUIText();
	}

	// increments our level
	public void LevelUp()
	{
		m_level++;
		m_lines = m_linesPerLevel* m_level;
		didLevelUp = true;

		if (m_levelUpFx)
		{
			m_levelUpFx.Play();
		}
	}

	void Start () 
	{
		Reset();
    }

	// returns a string padded to a certain number of places
	string PadZero(int n,int padDigits)
	{
		string nStr = n.ToString();

		while (nStr.Length < padDigits)
		{
			nStr = "0" + nStr;
		}
		return nStr;
	}



}
