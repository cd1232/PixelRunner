using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MadePotionsList : MonoBehaviour
{
	[SerializeField]
	private GameObject m_madePotionPrefab;

	[SerializeField]
	private Canvas m_canvas;

	[SerializeField]
	private int m_maxMadePotions = 1;

	private MadePotion m_madePotion;

	private void Awake()
	{
	}

	public bool AddPotion(Potion potion)
	{
		if (m_madePotion)
		{
			Destroy(m_madePotion.gameObject);
		}

		GameObject madePotion = Instantiate(m_madePotionPrefab, transform);
		madePotion.GetComponent<MadePotion>().SetupMadePotion(potion, m_canvas, this);

		m_madePotion = madePotion.GetComponent<MadePotion>();
		return true;
	}

	public void RemovePotion(MadePotion potion)
	{
		m_madePotion = null;
	}
}
