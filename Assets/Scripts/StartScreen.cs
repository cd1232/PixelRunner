using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
	[SerializeField]
	private GameObject m_creditsScreen;

	[SerializeField]
	private GameObject m_optionsScreen;

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

	public void ShowOptions()
	{
		m_optionsScreen.SetActive(true);
	}

	public void HideOptions()
	{
		m_optionsScreen.SetActive(false);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
