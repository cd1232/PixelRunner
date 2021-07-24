using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_heading;

	[SerializeField]
	private TextMeshProUGUI m_subHeading;

	[SerializeField]
	private TextMeshProUGUI m_highestEarnings;

	[SerializeField]
	private TextMeshProUGUI m_timer;

	[SerializeField]
	private Button m_playAgain;

	public Action OnPlayAgain;

	public void Start()
	{
		m_playAgain.onClick.AddListener(OnPlayAgainPressed);
	}

	void OnPlayAgainPressed()
	{
		gameObject.SetActive(false);
		GameManager.GetInstance().PlayAgain();
	}

	public void ShowEndScreen()
	{
		GameManager gameManager = GameManager.GetInstance();

		float highestEarnings = gameManager.GetHighestEarnings();

		if (highestEarnings >= gameManager.GetWinningAmount())
		{
			m_heading.text = "Victory!!";
			m_subHeading.text = "Congratulations! You have earned your way out of the dungeon";
			m_highestEarnings.text = "";

		}
		else
		{
			m_heading.text = "Game Over";
			m_highestEarnings.text = "Highest Earnings: $" + highestEarnings.ToString("F2") + "/$" + gameManager.GetWinningAmount().ToString("F2");
			m_subHeading.text = "You've ran out of money and been replaced";
		}

		float timeElapsed = gameManager.GetTimeElapsed();
		int minutes = 0;
		int seconds = 0;

		while (timeElapsed >= 60)
		{
			minutes++;
			timeElapsed -= 60;
		}

		seconds = (int)timeElapsed;

		m_timer.text = "Elapsed Time: " + minutes + " mins " + seconds + " secs";
	}
}
