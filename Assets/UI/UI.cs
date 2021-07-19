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
	private BetResultPopup m_popUp;

	[SerializeField]
	private Button m_nextButton;

	[SerializeField]
	private SoldPotionStatusUI m_soldPotionStatus;

	[SerializeField]
	private GameManager gameManager;

	private List<Toggle> m_allToggles = new List<Toggle>();

	private Hero m_currentlyDisplayedHero;

	private List<DungeonEntry> m_dungeonEntries = new List<DungeonEntry>();

	private void Start()
	{
		gameManager.OnDisplayHero += OnDisplayHero;
		gameManager.OnHideHero += OnHideHero;
		gameManager.OnAddHeroToDungeon += OnAddHeroToDungeon;
		gameManager.OnHeroFinishedDungeon += OnHeroFinishedInDungeon;

		m_currentMoney.text = "$" + gameManager.GetCurrentMoney();

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
		if (gameManager.GetMadePotion() == null)
		{
			Debug.Log("Made potion is null??");
		}

		m_soldPotionStatus.SetSoldPotionText(gameManager.GetMadePotion());

		m_potionScreen.SetActive(false);
		m_betScreen.SetActive(true);

		m_nextButton.onClick.RemoveListener(SwitchToBetScreen);
		m_nextButton.onClick.AddListener(FinishBetScreen);
		m_nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Next Hero";		
	}

	public void FinishBetScreen()
	{
		gameManager.SendHeroToDungeon();

		foreach(Toggle t in m_allToggles)
		{
			t.isOn = false;
		}

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
		m_popUp.OnReceiveButtonPressed += ReceivePayment;
		m_popUp.ShowInfo(heroInDungeon);
	}

	void ReceivePayment()
	{
		//TODO receive payment
		m_popUp.gameObject.SetActive(false);
		m_popUp.OnReceiveButtonPressed -= ReceivePayment;
	}

	void OnToggleChanged(bool bNewValue)
	{
		if (bNewValue)
		{
			List<int> newCombinations = new List<int>();

			int strength = -1;
			int buffType = -1;
			int color = -1;

			for (int i = 0; i < 3; ++i)
			{
				if (m_allToggles[i].isOn)
				{
					strength = i;
				}
			}

			for (int i = 3; i < 6; ++i)
			{
				if (m_allToggles[i].isOn)
				{
					buffType = i - 3;
				}
			}

			for (int i = 6; i < m_allToggles.Count; ++i)
			{
				if (m_allToggles[i].isOn)
				{
					color = i - 6;
				}
			}

			gameManager.SetCreatedPotion((HealingStrength)strength, (BuffType)buffType, (PotionColor)color);
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