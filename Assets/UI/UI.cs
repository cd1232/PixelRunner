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

	[SerializeField]
	private GameObject m_dungeonListPanel;

	[SerializeField]
	private GameObject m_dungeonEntryUIPrefab;

	private List<KeyValuePair<DungeonEntry, float>> m_dungeonEntries = new List<KeyValuePair<DungeonEntry, float>>();
	private List<CustomerController> m_dungeonCustomerList = new List<CustomerController>();

	private enum PotionMakingStage
	{
		Waiting,
		FirstStage,
		SecondStage,
		ThirdStage,
		FourthStage // Guess what floor they will die on
	}

	private PotionMakingStage m_potionMakingStage;

	private HealPower m_currentHealPower = HealPower.Invalid;
	private PotionColor m_currentPotionColor = PotionColor.Invalid;
	private int m_currentSpeed = 0;
	private int m_currentFloor = 0;

	private void Start()
	{
		GameManager.GetInstance().OnNewDungeonCustomer += OnNewDungeonCustomer;
	}

	void OnNewDungeonCustomer(GameObject newCustomer)
	{
		GameObject dungeonEntryObject = Instantiate(m_dungeonEntryUIPrefab, m_dungeonListPanel.transform);
		DungeonEntry dungeonEntry = dungeonEntryObject.GetComponent<DungeonEntry>();

		m_dungeonCustomerList.Add(newCustomer.GetComponent<CustomerController>());
		m_dungeonEntries.Add(new KeyValuePair<DungeonEntry, float>(dungeonEntry, 10.0f));

		dungeonEntry.SetInfo(null, "Status: Alive", "Floor Guess: " + newCustomer.GetComponent<CustomerController>().m_floorDeathGuess);
	}

	private void Update()
	{
		for (int i = 0; i < m_dungeonEntries.Count; ++i)
		{
			if (m_dungeonEntries[i].Value > 0.0f)
			{
				m_dungeonEntries[i] = new KeyValuePair<DungeonEntry, float>(m_dungeonEntries[i].Key, m_dungeonEntries[i].Value - Time.deltaTime);
			}


		}

		// Need to remove entries on ui prefab
		//m_dungeonEntries.RemoveAll(dungeonEntry => dungeonEntry.Value <= 0.0f);
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
			case PotionMakingStage.FourthStage:
				m_currentFloor++;
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
			case PotionMakingStage.FourthStage:
				m_currentFloor = 0;
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
			case PotionMakingStage.FourthStage:
				m_currentPotionText.text = "What floor will they die on? " + (m_currentFloor <= 10 ? m_currentFloor.ToString() : "Invalid");
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
		if (m_potionMakingStage == PotionMakingStage.FourthStage)
		{
			m_potionMakingStage = PotionMakingStage.Waiting;
			StartCoroutine(DisplayPotionMade());
			GameManager.GetInstance().PotionCompleted(new Potion(m_currentHealPower, m_currentPotionColor, m_currentSpeed), m_currentFloor);
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
		m_currentFloor = 0;
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

	public void OnCustomerReachedDesk(CustomerController customer)
	{
		Potion wantedPotion = customer.GetWantedPotion();
		string speedText = "";

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
