using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
	[SerializeField]
	private Customer m_customer;

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

	private Hero m_currentlyDisplayedHero;

	private HeroInDungeon m_heroPopup;

	private AudioSource m_buttonClickSource;

	private List<DungeonEntry> m_dungeonEntries = new List<DungeonEntry>();

	private void Awake()
	{
		m_buttonClickSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		GameManager gameManager = GameManager.GetInstance();

		gameManager.OnDisplayHero += OnDisplayHero;
		gameManager.OnHideHero += OnHideHero;
		gameManager.OnAddHeroToDungeon += OnAddHeroToDungeon;
		gameManager.OnHeroFinishedDungeon += OnHeroFinishedInDungeon;
		gameManager.OnGameEnded += OnGameEnded;
		gameManager.OnPotionGiven += SwitchToBetScreen;
		m_nextButton.onClick.AddListener(FinishBetScreen);
	}

	public void PlayButtonSound()
	{
		m_buttonClickSource.Play();
	}

	public void SwitchToBetScreen()
	{
		m_soldPotionStatus.SetSoldPotionText(GameManager.GetInstance().GetMadePotion());

		m_potionScreen.SetActive(false);
		m_betScreen.SetActive(true);
		m_nextButton.gameObject.SetActive(true);
	}

	public void FinishBetScreen()
	{
		GameManager.GetInstance().SendHeroToDungeon();

		m_biddingPanel.Reset();
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

	void OnDisplayHero(Hero hero)
	{
		m_currentlyDisplayedHero = hero;
		m_customer.SetCustomer(hero);
	}

	void OnHideHero()
	{
		m_customer.SetCustomer(null);
	}
}