using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoldPotionStatusUI : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_healingStrength;

	[SerializeField]
	private TextMeshProUGUI m_buffType;

	[SerializeField]
	private TextMeshProUGUI m_color;

	public void SetSoldPotionText(Potion potion)
	{
		m_healingStrength.text = "Healing Strength: " + potion.m_healingStrength;
		m_buffType.text = "Buff Type: " + potion.m_buffType;
		m_color.text = "Color: " + potion.m_potionColor;
	}

	public void ResetPotionText()
	{
		m_healingStrength.text = "Healing Strength: None";
		m_buffType.text = "Buff Type: None";
		m_color.text = "Color: Transparent";
	}
}
