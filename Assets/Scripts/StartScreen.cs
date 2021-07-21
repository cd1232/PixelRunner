using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
	[SerializeField]
	private GameObject m_creditsScreen;

    public void StartGame()
	{
		SceneManager.LoadScene("main");
	}

	public void ShowCredits()
	{
		m_creditsScreen.SetActive(true);
	}

	public void HideCredits()
	{
		m_creditsScreen.SetActive(false);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
