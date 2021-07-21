using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
	[SerializeField]
	private Button m_startGame;

	private void Start()
	{
		m_startGame.onClick.AddListener(OnStartGameClicked);
	}

	void OnStartGameClicked()
	{
		GameManager.GetInstance().StartGame();
		gameObject.SetActive(false);
	}
}
