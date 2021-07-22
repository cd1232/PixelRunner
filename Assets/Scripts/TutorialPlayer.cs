using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPlayer : MonoBehaviour
{
	[SerializeField]
	private GameObject m_commentWindow;

	[SerializeField]
	private GameObject m_ingredientsWindow;

	[SerializeField]
	private GameObject m_potionMakerWindow;

	[SerializeField]
	private GameObject m_bettingWindow;

	[SerializeField]
	private GameObject m_heroWindow;

	[SerializeField]
	private GameObject m_secondScreen;

	[SerializeField]
	private GameObject m_dungeonList;

	[SerializeField]
	private GameObject m_potionList;

	// Start is called before the first frame update
	void Start()
    {
    }

	public void Play()
	{
		m_commentWindow.gameObject.SetActive(true);
	}

	public void RemoveTutorialEntries()
	{
		Destroy(m_dungeonList.GetComponentInChildren<GameObject>());
		Destroy(m_potionList.GetComponentInChildren<GameObject>());
		m_secondScreen.gameObject.SetActive(false);
	}
}
