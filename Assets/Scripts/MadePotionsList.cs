using System.Collections;
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
	private int m_maxMadePotions = 3;

	private HorizontalLayoutGroup m_horizontalLayoutGroup;

	private List<MadePotion> m_madePotions = new List<MadePotion>();


	private void Awake()
	{
		m_horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
	}

	public bool AddPotion(Potion potion)
	{
		if (m_madePotions.Count < m_maxMadePotions)
		{ 
			GameObject madePotion = Instantiate(m_madePotionPrefab, m_horizontalLayoutGroup.transform);
			madePotion.GetComponent<MadePotion>().SetupMadePotion(potion, m_canvas, this);
			m_madePotions.Add(madePotion.GetComponent<MadePotion>());
			return true;
		}

		return false;
	}

	public void RemovePotion(MadePotion potion)
	{
		m_madePotions.Remove(potion);
	}
}
