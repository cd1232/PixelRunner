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
		m_healingStrength.text = potion.m_healingIngredient.m_name;
		m_buffType.text = potion.m_buffIngredient.name;
		m_color.text = potion.m_colorIngredient.name;
	}

	public void ResetPotionText()
	{
		m_healingStrength.text = "";
		m_buffType.text = "";
		m_color.text = "";
	}
}
