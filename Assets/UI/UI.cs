using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
	[SerializeField]
	private PotionTextSettings m_potionTextSettings;

	[SerializeField]
	private TextMeshProUGUI m_text;

	[SerializeField]
	private TextMeshProUGUI m_currentPotionText;

	[SerializeField]
	private TextMeshProUGUI m_announcementText;

	private enum PotionMakingStage
	{
		Waiting,
		FirstStage,
		SecondStage,
		ThirdStage
	}

	private PotionMakingStage m_potionMakingStage;

	private HealPower m_currentHealPower = HealPower.Invalid;
	private PotionColor m_currentPotionColor = PotionColor.Invalid;
	private int m_currentSpeed;

	private void Awake()
	{
	}

	void OnSubmitPotion()
	{
		if (m_potionMakingStage == PotionMakingStage.Waiting)
			return;

		SwitchToNextStage();		
	}

	void OnSwitch()
	{
		if (m_potionMakingStage == PotionMakingStage.Waiting)
			return;

		switch (m_potionMakingStage)
		{
			case PotionMakingStage.FirstStage:
				m_currentHealPower++;
				break;
			case PotionMakingStage.SecondStage:
				m_currentPotionColor++;
				break;
			case PotionMakingStage.ThirdStage:
				m_currentSpeed++;
				break;
		}

		UpdatePotionText();
	}

	void OnReset()
	{
		if (m_potionMakingStage == PotionMakingStage.Waiting)
			return;

		switch (m_potionMakingStage)
		{
			case PotionMakingStage.FirstStage:
				m_currentHealPower = 0;
				break;
			case PotionMakingStage.SecondStage:
				m_currentPotionColor = 0;
				break;
			case PotionMakingStage.ThirdStage:
				m_currentSpeed = 0;
				break;
		}

		UpdatePotionText();
	}

	private void UpdatePotionText()
	{
		switch (m_potionMakingStage)
		{
			case PotionMakingStage.FirstStage:
				m_currentPotionText.text = "Potion Strength: " + GetPotionStrengthText();
				break;
			case PotionMakingStage.SecondStage:
				m_currentPotionText.text = "Potion Color: " + GetPotionColorText();
				break;
			case PotionMakingStage.ThirdStage:
				m_currentPotionText.text = "Potion Speed: " + (m_currentSpeed > 3 ? "Invalid" : m_currentSpeed.ToString());
				break;
		}
	}

	private string GetPotionStrengthText()
	{
		if ((int)m_currentHealPower >= 3)
		{
			return "Invalid";
		}

		return m_currentHealPower.ToString();
	}

	private string GetPotionColorText()
	{
		if ((int)m_currentPotionColor >= 3)
		{
			return "Invalid";
		}

		return m_currentPotionColor.ToString();
	}

	private string GetSpeedText()
	{
		string speedText = "Invalid";
		foreach (var speed in m_potionTextSettings.m_speedText)
		{
			if (speed.speed == m_currentSpeed)
				speedText = speed.text;
		}

		return speedText;
	}

	public void SwitchToNextStage()
	{
		if (m_potionMakingStage == PotionMakingStage.ThirdStage)
		{
			m_potionMakingStage = PotionMakingStage.Waiting;
			StartCoroutine(DisplayPotionMade());
			GameManager.GetInstance().PotionCompleted();
			ResetCurrentPotion();
			m_currentPotionText.text = "Waiting...";
		}
		else
		{
			m_potionMakingStage++;
			m_announcementText.text = "";
		}

		UpdatePotionText();
	}

	void ResetCurrentPotion()
	{
		m_currentHealPower = HealPower.Invalid;
		m_currentPotionColor = PotionColor.Invalid;
		m_currentSpeed = 0;
	}

	IEnumerator DisplayPotionMade()
	{
		string speedText = "";
		foreach (var speed in m_potionTextSettings.m_speedText)
		{
			if (speed.speed == m_currentSpeed)
				speedText = speed.text;
		}

		m_announcementText.text = "You made a " + m_currentHealPower + " " + m_currentPotionColor + " that was " + GetSpeedText();
		yield return new WaitForSeconds(3);
		m_announcementText.text = "";
	}

	//void UpdateDefaultPotionText()
	//{
	//	switch (m_potionMakingStage)
	//	{
	//		case PotionMakingStage.Waiting:
	//			m_currentPotionText.text = "Waiting...";
	//			break;
	//		case PotionMakingStage.FirstStage:
	//			m_currentPotionText.text = "Potion Strength: None";
	//			break;
	//		case PotionMakingStage.SecondStage:
	//			m_currentPotionText.text = "Potion Color: None";
	//			break;
	//		case PotionMakingStage.ThirdStage:
	//			m_currentPotionText.text = "Potion Speed: None";
	//			break;
	//	}
	//}

	public void OnCustomerReachedDesk(CustomerController customer)
	{
		Potion wantedPotion = customer.GetWantedPotion();

		//string healPowerText = "";
		//string potionColorText = "";
		string speedText = "";

		//foreach (var healPower in m_potionTextSettings.m_healPowerText)
		//{
		//	if (healPower.healPower == wantedPotion.m_healPower)
		//		healPowerText = healPower.text;
		//}

		//foreach (var potionColor in m_potionTextSettings.m_potionColorText)
		//{
		//	if (potionColor.color == wantedPotion.m_potionColor)
		//		potionColorText = potionColor.text;
		//}

		foreach (var speed in m_potionTextSettings.m_speedText)
		{
			if (speed.speed == wantedPotion.m_speed)
				speedText = speed.text;
		}

		string textToDisplay = m_potionTextSettings.m_startText + " " + wantedPotion.m_healPower + " " + wantedPotion.m_potionColor + " potion that hits me " + speedText;

		m_text.text = textToDisplay;

		SwitchToNextStage();
	}

}
