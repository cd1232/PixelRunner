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
		FinalStatSettings finalStatSettings = GameManager.GetInstance().GetStatSettings();

		if (potion.m_healingIngredient)
		{
			m_healingStrength.text = potion.m_healingIngredient.m_name;
		}
		else
		{
			m_healingStrength.text = finalStatSettings.m_noHealHint;

		}

		if (potion.m_buffIngredient)
		{
			m_buffType.text = potion.m_buffIngredient.name;
		}
		else
		{
			m_buffType.text = finalStatSettings.m_noBuffHint;

		}

		if (potion.m_colorIngredient)
		{
			m_color.text = potion.m_colorIngredient.name;
		}
		else
		{
			m_color.text = finalStatSettings.m_noColorHint;
		}

	}

	public void ResetPotionText()
	{
		m_healingStrength.text = "";
		m_buffType.text = "";
		m_color.text = "";
	}
}
