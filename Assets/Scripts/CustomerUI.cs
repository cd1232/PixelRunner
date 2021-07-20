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

	[SerializeField]
	private PotionTextSettings m_potionTextSettings;

	[SerializeField]
	private Image m_customerIcon;

	private Potion m_wantedPotion;

	public void SetCustomer(Hero hero)
	{
		if (hero != null)
		{
			m_wantedPotion = hero.m_wantedPotion;
			HeroStats stats = hero.m_heroStats;

			HealPowerText healPowerText = m_potionTextSettings.m_healPowerText.Find(healPower => healPower.healPower == m_wantedPotion.m_healingStrength);
			BuffText buffTextSetting = m_potionTextSettings.m_buffText.Find(buff => buff.buffType == m_wantedPotion.m_buffType);
			PotionColorText potionColorTextSetting = m_potionTextSettings.m_potionColorText.Find(potionColor => potionColor.color == m_wantedPotion.m_potionColor);

			string startText = m_potionTextSettings.m_startText;
			string healingStrengthText = healPowerText != null ? healPowerText.text : "no healing";
			string buffText = buffTextSetting != null ? buffTextSetting.text : "no buff";
			string potionColorText = potionColorTextSetting != null ? potionColorTextSetting.text : "no color";

			string textToDisplay = startText + healingStrengthText + " and gives me " + buffText + ". Can you make it " + potionColorText + "?";

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
