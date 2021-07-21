using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
	[SerializeField]
	private CustomerUI m_customerUI;

	[SerializeField]
	private TextMeshProUGUI m_currentMoney;

	[SerializeField]
	private GameObject m_dungeonList;

	[SerializeField]
	private GameObject m_dungeonEntryPrefab;

	[SerializeField]
	private GameObject m_potionScreen;

	[SerializeField]
	private GameObject m_betScreen;

	[SerializeField]
	private BiddingPanel m_biddingPanel;

	[SerializeField]
	private BetResultPopup m_popUp;

	[SerializeField]
	private Button m_nextButton;

	[SerializeField]
	private EndScreen m_endScreen;

	[SerializeField]
	private SoldPotionStatusUI m_soldPotionStatus;

	private List<Toggle> m_allToggles = new List<Toggle>();

	private Hero m_currentlyDisplayedHero;

	private HeroInDungeon m_heroPopup;

	private List<DungeonEntry> m_dungeonEntries = new List<DungeonEntry>();

	private void Start()
	{
		GameManager gameManager = GameManager.GetInstance();

		gameManager.OnDisplayHero += OnDisplayHero;
		gameManager.OnHideHero += OnHideHero;
		gameManager.OnAddHeroToDungeon += OnAddHeroToDungeon;
		gameManager.OnHeroFinishedDungeon += OnHeroFinishedInDungeon;
		gameManager.OnMoneyChanged += OnMoneyChanged;
		gameManager.OnGameEnded += OnGameEnded;

		m_currentMoney.text = "$" + gameManager.GetCurrentMoney().ToString("F2");

		Toggle[] foundToggles = m_potionScreen.GetComponentsInChildren<Toggle>();
		foreach(Toggle t in foundToggles)
		{
			t.onValueChanged.AddListener(OnToggleChanged);
		}

		m_nextButton.onClick.AddListener(SwitchToBetScreen);

		m_allToggles.AddRange(foundToggles);
	}

	public void SwitchToBetScreen()
	{
		m_soldPotionStatus.SetSoldPotionText(GameManager.GetInstance().GetMadePotion());

		m_potionScreen.SetActive(false);
		m_betScreen.SetActive(true);

		GameManager.GetInstance().PotionCreated();

		m_nextButton.onClick.RemoveListener(SwitchToBetScreen);
		m_nextButton.onClick.AddListener(FinishBetScreen);
		m_nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Next Hero";		
	}

	public void FinishBetScreen()
	{
		GameManager.GetInstance().SendHeroToDungeon();

		foreach(Toggle t in m_allToggles)
		{
			t.isOn = false;
		}

		m_biddingPanel.Reset();

		m_nextButton.onClick.RemoveListener(FinishBetScreen);
		m_nextButton.onClick.AddListener(SwitchToBetScreen);
		m_nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Next";
		m_potionScreen.SetActive(true);
		m_betScreen.SetActive(false);
	}

	void OnAddHeroToDungeon(KeyValuePair<HeroInDungeon, float> dungeonHero)
	{
		GameObject newDungeonEntryObject = Instantiate(m_dungeonEntryPrefab, m_dungeonList.transform);
		DungeonEntry newDungeonEntry = newDungeonEntryObject.GetComponent<DungeonEntry>();
		newDungeonEntry.OnPopup += OnPopup;
		newDungeonEntry.Setup(dungeonHero.Key);
		m_dungeonEntries.Add(newDungeonEntry);
	}

	void OnHeroFinishedInDungeon(HeroInDungeon heroInDungeon)
	{
		DungeonEntry foundEntry = m_dungeonEntries.Find(dungeonEntry => dungeonEntry.GetHero() == heroInDungeon);
		if (foundEntry != null)
		{
			foundEntry.SetComplete();
		}
	}

	void OnPopup(HeroInDungeon heroInDungeon, DungeonEntry dungeonEntry)
	{		
		dungeonEntry.OnPopup -= OnPopup;
		Destroy(dungeonEntry.gameObject);

		m_popUp.OnReceiveButtonPressed += ReceivePayment;
		m_popUp.ShowInfo(heroInDungeon);
		m_heroPopup = heroInDungeon;
	}

	void OnMoneyChanged(float newAmount)
	{
		m_currentMoney.text = "$" + newAmount.ToString("F2");
	}

	void OnGameEnded()
	{
		float money = GameManager.GetInstance().GetCurrentMoney();

		foreach (DungeonEntry dungeonEntry in m_dungeonEntries)
		{
			if (dungeonEntry != null)
			{
				Destroy(dungeonEntry.gameObject);
			}
		}

		m_dungeonEntries.Clear();
		m_currentlyDisplayedHero = null;
		m_heroPopup = null;
		m_soldPotionStatus.ResetPotionText();


		m_endScreen.ShowEndScreen();
		m_endScreen.gameObject.SetActive(true);
	}

	void ReceivePayment()
	{
		if (m_heroPopup.m_hero.m_selectedFloor > 0)
		{
			GameManager.GetInstance().AddPaymentForHero(m_heroPopup);
		}
		m_popUp.gameObject.SetActive(false);
		m_popUp.OnReceiveButtonPressed -= ReceivePayment;
		m_heroPopup = null;
	}

	public void OnStrengthChanged(HealingStrength healingStrength)
	{

	}

	void OnToggleChanged(bool bNewValue)
	{
		if (bNewValue)
		{
			// TODO change this. Toggles suck.
			List<int> newCombinations = new List<int>();

			int strength = 0;
			int buffType = 0;
			int color = 0;

			for (int i = 0; i < 3; ++i)
			{
				if (m_allToggles[i].isOn)
				{
					strength = i + 1;
				}
			}

			for (int i = 3; i < 6; ++i)
			{
				if (m_allToggles[i].isOn)
				{
					buffType = i - 2;
				}
			}

			for (int i = 6; i < m_allToggles.Count; ++i)
			{
				if (m_allToggles[i].isOn)
				{
					color = i - 5;
				}
			}

			GameManager.GetInstance().SetCreatedPotion((HealingStrength)strength, (BuffType)buffType, (PotionColor)color);
		}
	}

	void OnDisplayHero(Hero hero)
	{
		m_currentlyDisplayedHero = hero;
		m_customerUI.SetCustomer(hero);
	}

	void OnHideHero()
	{
		m_customerUI.SetCustomer(null);
	}
}