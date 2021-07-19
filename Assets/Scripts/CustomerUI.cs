using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_customerComments;

	[SerializeField]
	private TextMeshProUGUI m_hp;

	[SerializeField]
	private TextMeshProUGUI m_items;

	//[SerializeField]
	//private TextMeshProUGUI m_color;

	[SerializeField]
	private Image m_customerIcon;

	private Potion m_wantedPotion;

	public void SetCustomer(Hero hero)
	{
		if (hero != null)
		{
			m_wantedPotion = hero.m_wantedPotion;
			HeroStats stats = hero.m_heroStats;

			string textToDisplay = "I need a " + m_wantedPotion.m_healingStrength + " healing potion that gives me " + m_wantedPotion.m_buffType + ". Could you make it " + m_wantedPotion.m_potionColor + "?";
			m_customerComments.text = textToDisplay;

			m_hp.text = "HP: " + stats.m_currentHP + "/" + stats.m_maxHP;
			m_items.text = "Items: " + stats.m_weaponType + " + " + stats.m_armorType;
		}
		else
		{
			m_customerComments.text = "";
			m_hp.text = "";
			m_items.text = "";
		}
		
	}
}
