using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BetResultPopup : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_heroHP;

	[SerializeField]
	private TextMeshProUGUI m_items;

	[SerializeField]
	private TextMeshProUGUI m_potionStrength;

	[SerializeField]
	private TextMeshProUGUI m_potionBuff;

	[SerializeField]
	private TextMeshProUGUI m_placeOfDeath;

	[SerializeField]
	private TextMeshProUGUI m_finalWords;

	[SerializeField]
	private TextMeshProUGUI m_floorBet;

	[SerializeField]
	private TextMeshProUGUI m_betAmount;

	[SerializeField]
	private TextMeshProUGUI m_amountWon;

	[SerializeField]
	private Button m_receiveButton;

	public Action OnReceiveButtonPressed;

	public void Start()
	{
		m_receiveButton.onClick.AddListener(OnClick);
	}

	void OnClick()
	{
		OnReceiveButtonPressed?.Invoke();
	}

	public void ShowInfo(HeroInDungeon heroInDungeon)
	{
		HeroStats heroStats = heroInDungeon.m_hero.m_heroStats;
		Potion createdPotion = heroInDungeon.m_hero.m_createdPotion;


		m_heroHP.text = "HP: " + heroStats.m_currentHP + "/" + heroStats.m_maxHP; ;
		m_items.text = "Items: " + heroStats.m_weaponType + " with " + heroStats.m_armorType;

		m_potionStrength.text = "Healing Strength: " + createdPotion.m_healingStrength;
		m_potionBuff.text = "Buff Type: " + createdPotion.m_buffType;
		
		// TODO
		// game manager should determine place of death when finishing a dungeon adventure, along with final words.


		gameObject.SetActive(true);
	}
}
