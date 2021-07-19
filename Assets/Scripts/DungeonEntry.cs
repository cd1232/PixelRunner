using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonEntry : MonoBehaviour
{
	[SerializeField]
	private Sprite m_tickImage;

	[SerializeField]
	private Sprite m_timerImage;

	[SerializeField]
	private Image m_heroImage;

	[SerializeField]
	private Image m_heroStatus;

	private HeroInDungeon m_hero;
	private bool m_isComplete = false;

	public Action<HeroInDungeon, DungeonEntry> OnPopup;

	public void Setup(HeroInDungeon hero)
	{
		m_hero = hero;
		m_heroStatus.sprite = m_timerImage;
	}

	public void SetComplete()
	{
		m_isComplete = true;
		m_heroStatus.sprite = m_tickImage;
		GetComponent<Button>().interactable = true;
		GetComponent<Button>().onClick.AddListener(OnButtonClick);
	}

	void OnButtonClick()
	{
		if (m_isComplete)
		{
			OnPopup?.Invoke(m_hero, this);
		}
	}

	public HeroInDungeon GetHero()
	{
		return m_hero;
	}
}
