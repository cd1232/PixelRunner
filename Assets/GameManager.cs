using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HeroInDungeon
{
	public HeroInDungeon(Hero hero)
	{
		m_hero = hero;
	}

	public Hero m_hero;
	public int m_bidAmount = 0;
	public int m_placeOfDeath = 0;
	public string m_finalWords = "";
	public bool m_hasDungeonBeenBeaten = false;
	public bool m_haveResultsBeenCalculated = false;
	public bool m_shouldDelete = false;

	public float m_MoneyWon = 0;
	public float m_rewardMultiplier = 1.0f;
}

public class GameManager : MonoBehaviour
{
	#region Singleton
	public static GameManager GetInstance()
	{
		return Instance;
	}

	private static GameManager Instance;
	#endregion

	[SerializeField]
	private float m_dungeonCountdown = 3.0f;

	[SerializeField]
	private int m_rightIngredientPayment = 5;

	[SerializeField]
	private float m_startingMoney = 50.0f;

	[SerializeField]
	private float m_winningAmount = 200.0f;

	[SerializeField]
	private List<Sprite> m_lightArmorHeroSprites = new List<Sprite>();

	[SerializeField]
	private List<Sprite> m_heavyArmorHeroSprites = new List<Sprite>();

	[SerializeField]
	private FinalStatSettings finalStatSettings;

	[SerializeField]
	private TutorialPlayer m_tutorialPlayer;

	[Space]

	public Action<Hero> OnDisplayHero;
	public Action OnHideHero;
	public Action<KeyValuePair<HeroInDungeon, float>> OnAddHeroToDungeon;
	public Action<HeroInDungeon> OnHeroFinishedDungeon;
	public Action<float> OnMoneyChanged;
	public Action OnMoneyAdded;
	public Action OnGameEnded;
	public Action OnPotionGiven;

	private List<Hero> m_heroes = new List<Hero>();
	private List<KeyValuePair<HeroInDungeon, float>> m_dungeonHeroes = new List<KeyValuePair<HeroInDungeon, float>>();

	[SerializeField]
	private Hero m_currentHero;

	private float m_currentMoney;
	private float m_highestEarnings;
	private Potion m_chosenPotion;

	private int m_bidAmount = 0;

	private float m_timeElapsed = 0.0f;
	private bool m_hasGameStarted = false;
	private bool m_hasGameEnded = false;

	private bool m_shouldPlayTutorial = true;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public void PlayAgain()
	{		
		StartGame();
	}

	public void StartGame()
	{
		//if (m_shouldPlayTutorial)
		//{
		//	m_tutorialPlayer.Play();
		//}
		//else
		//{
		//	m_tutorialPlayer.RemoveTutorialEntries();
		//}

		// if tutorial is on
		// delay start of game and first go through the tutorial
		m_hasGameEnded = false;
		m_dungeonHeroes.Clear();
		m_heroes.Clear();
		m_currentHero = null;
		m_bidAmount = 0;
		m_timeElapsed = 0.0f;
		m_currentMoney = m_startingMoney;
		m_highestEarnings = m_startingMoney;
		OnMoneyChanged?.Invoke(m_currentMoney);
		m_hasGameStarted = true;
		m_chosenPotion = new Potion(true);
		DisplayHero();
	}

	void DisplayHero()
	{
		m_bidAmount = 0;
		m_chosenPotion = new Potion(true);

		Hero newHero = GenerateNewHero();
		m_heroes.Add(newHero);
		m_currentHero = newHero;
		OnDisplayHero?.Invoke(newHero);
	}

	public void OnTutorialChanged(bool newValue)
	{
		m_shouldPlayTutorial = newValue;
	}

	Hero GenerateNewHero()
	{
		Hero newHero = new Hero();
		HeroStats newHeroStats = new HeroStats();

		newHeroStats.m_armorType = (ArmorType) Random.Range(0, 3);
		newHeroStats.m_weaponType = (WeaponType) Random.Range(0, 3);

		if (newHeroStats.m_armorType == ArmorType.HeavyArmor)
		{
			newHero.m_heroSprite = m_heavyArmorHeroSprites[Random.Range(0, m_heavyArmorHeroSprites.Count)];
		}
		else
		{
			newHero.m_heroSprite = m_lightArmorHeroSprites[Random.Range(0, m_lightArmorHeroSprites.Count)];
		}

		int heroMaxHP = 4;
		int heroMaxHPLevel = Random.Range(0, 3);
		switch (heroMaxHPLevel)
		{
			case 0:
				heroMaxHP = 8;
				break;
			case 1:
				heroMaxHP = 10;
				break;
			case 2:
				heroMaxHP = 12;
				break;
		}

		newHeroStats.m_maxHP = heroMaxHP;
		newHeroStats.m_currentHP = Random.Range(1, heroMaxHP + 1);

		newHero.m_heroStats = newHeroStats;
		newHero.m_wantedPotion = new Potion();

		return newHero;
	}

	public void SetBidAmount(int amount)
	{
		m_bidAmount = amount;
	}

	public void SetFloorDeathGuess(int floor)
	{
		m_currentHero.m_selectedFloor = floor;
	}

	public float GetCurrentMoney()
	{
		return m_currentMoney;
	}

	public float GetTimeElapsed()
	{
		return m_timeElapsed;
	}

	public Potion GetMadePotion()
	{
		return m_chosenPotion;
	}

	public float GetHighestEarnings()
	{
		return m_highestEarnings;
	}

	public float GetWinningAmount()
	{
		return m_winningAmount;
	}

	public void AddMoney(float addedAmount)
	{
		float previousMoney = m_currentMoney;

		m_currentMoney += addedAmount;

		if (m_currentMoney > previousMoney)
		{
			OnMoneyAdded?.Invoke();
		}
		OnMoneyChanged?.Invoke(m_currentMoney);

		if (m_currentMoney > m_highestEarnings)
		{
			m_highestEarnings = m_currentMoney;
		}

		if (m_currentMoney <= 0.0f || m_currentMoney >= m_winningAmount)
		{
			OnGameEnded?.Invoke();
			m_hasGameEnded = true;
		}
	}

	public void SetCreatedPotion(HealingStrength strength, BuffType buffType, PotionColor potionColor)
	{
		m_chosenPotion.m_healingStrength = strength;
		m_chosenPotion.m_buffType = buffType;
		m_chosenPotion.m_potionColor = potionColor;

		m_currentHero.m_createdPotion = m_chosenPotion;
		// Probably display both of these separately
		int moneyGained = Potion.GetNumMatchingIngredidents(m_currentHero.m_createdPotion, m_currentHero.m_wantedPotion) * m_rightIngredientPayment;


		AddMoney(moneyGained);
		OnMoneyChanged?.Invoke(m_currentMoney);

		OnPotionGiven?.Invoke();
	}


	public void SendHeroToDungeon()
	{	
		HeroInDungeon heroInDungeon = new HeroInDungeon(m_currentHero);
		m_dungeonHeroes.Add(new KeyValuePair<HeroInDungeon, float>(heroInDungeon, m_dungeonCountdown));
		OnAddHeroToDungeon?.Invoke(new KeyValuePair<HeroInDungeon, float>(heroInDungeon, m_dungeonCountdown));
		heroInDungeon.m_bidAmount = m_bidAmount;

		// Subtract bid amount from money
		AddMoney(-m_bidAmount);

		OnMoneyChanged?.Invoke(m_currentMoney);

		m_currentHero = null;
		OnHideHero?.Invoke();
		m_heroes.Remove(m_currentHero);
		DisplayHero();
	}

	public void CalculateHeroResults(HeroInDungeon heroInDungeon)
	{
		float heroHPCalc = heroInDungeon.m_hero.m_heroStats.m_currentHP;
		int maxHeroHP = heroInDungeon.m_hero.m_heroStats.m_maxHP;

		switch (heroInDungeon.m_hero.m_createdPotion.m_healingStrength)
		{
			case HealingStrength.Weak:
				heroHPCalc += 0.25f * maxHeroHP;
				break;
			case HealingStrength.Medium:
				heroHPCalc += 0.50f * maxHeroHP;
				break;
			case HealingStrength.Strong:
				heroHPCalc += maxHeroHP;
				break;
		}

		List<BaseModifier> baseModifiers = new List<BaseModifier>();

		HealthModifier healthModifier = finalStatSettings.m_healthModifiers.Find(modifier => heroHPCalc >= modifier.min && heroHPCalc <= modifier.max);
		BuffModifier buffModifier = finalStatSettings.m_buffModifiers.Find(modifier => modifier.buffType == heroInDungeon.m_hero.m_createdPotion.m_buffType);

		Debug.Log("Health Modifier: HPCalc is between " + healthModifier.min + " and " + healthModifier.max + " and value is " + healthModifier.modifier);

		Debug.Log("Buff Modifier: Type is " + buffModifier.buffType + " and value is " + buffModifier.modifier);

		baseModifiers.Add(healthModifier);
		baseModifiers.Add(buffModifier);


		List<ItemModifier> itemModifiers = new List<ItemModifier>();

		List<ItemModifier> allItemModifiers = finalStatSettings.m_ItemModifiers;

		WeaponType weaponType = heroInDungeon.m_hero.m_heroStats.m_weaponType;
		ArmorType armorType = heroInDungeon.m_hero.m_heroStats.m_armorType;
		foreach (var currentItemModifier in allItemModifiers)
		{
			if ((weaponType == currentItemModifier.weaponType && armorType == currentItemModifier.armorType && currentItemModifier.itemModifierState == ItemModifierState.UseBoth) ||
				(weaponType == currentItemModifier.weaponType && currentItemModifier.itemModifierState == ItemModifierState.UseWeapon) ||
				(armorType == currentItemModifier.armorType && currentItemModifier.itemModifierState == ItemModifierState.UseArmor))
			{
				itemModifiers.Add(currentItemModifier);

				string debug = "Item Modifier: ";
				switch (currentItemModifier.itemModifierState)
				{
					case ItemModifierState.UseWeapon:
						debug += currentItemModifier.weaponType;
						break;
					case ItemModifierState.UseArmor:
						debug += currentItemModifier.armorType;
						break;
					case ItemModifierState.UseBoth:
						debug += currentItemModifier.weaponType + " with " + currentItemModifier.armorType;
						break;
				}

				debug += " and value is " + currentItemModifier.modifier;
				Debug.Log(debug);
			}
		}

		baseModifiers.AddRange(itemModifiers);

		List<ItemAndBuffModifier> itemAndBuffModifiers = new List<ItemAndBuffModifier>();
		List<ItemAndBuffModifier> allItemAndBuffModifiers = finalStatSettings.m_itemAndBuffModifiers;

		BuffType buffType = heroInDungeon.m_hero.m_createdPotion.m_buffType;

		foreach (var currentItemAndBuffModifier in allItemAndBuffModifiers)
		{
			if (buffType == currentItemAndBuffModifier.buffType)
			{
				if ((armorType == currentItemAndBuffModifier.armorType && currentItemAndBuffModifier.itemModifierState == ItemModifierState.UseArmor) ||
					(weaponType == currentItemAndBuffModifier.weaponType && currentItemAndBuffModifier.itemModifierState == ItemModifierState.UseWeapon))
				{
					itemAndBuffModifiers.Add(currentItemAndBuffModifier);


					string debug = "Item and Buff Modifier: ";
					if (currentItemAndBuffModifier.itemModifierState == ItemModifierState.UseArmor)
						debug += currentItemAndBuffModifier.armorType;
					else
						debug += currentItemAndBuffModifier.weaponType;

					debug += " with " + currentItemAndBuffModifier.buffType + " and value is " + currentItemAndBuffModifier.modifier;
					Debug.Log(debug);
				}
			}
		}

		baseModifiers.AddRange(itemAndBuffModifiers);

		int finalModifier = 0;
		string finalWords = finalStatSettings.m_finalWords[finalStatSettings.m_finalWords.Count - 1];
		int highestModifier = baseModifiers[0].modifier;

		Debug.Log("All modifiers:");
		foreach (var modifier in baseModifiers)
		{
			Debug.Log("Modifier value is: " + modifier.modifier);
			finalModifier += modifier.modifier;

			if (Math.Abs(modifier.modifier) > highestModifier && modifier.finalWord >= 0)
			{
				highestModifier = modifier.modifier;
				finalWords = finalStatSettings.m_finalWords[modifier.finalWord];
			}
		}
		Debug.Log("Final Modifier: " + finalModifier);

		if (finalModifier < 5)
		{
			heroInDungeon.m_placeOfDeath = 1;
		}
		else if (finalModifier == 5)
		{
			heroInDungeon.m_placeOfDeath = 2;
		}
		else if (finalModifier == 6)
		{
			heroInDungeon.m_placeOfDeath = 3;
		}
		else if (finalModifier == 7)
		{
			heroInDungeon.m_placeOfDeath = 4;
		}
		else if (finalModifier < 10)
		{
			heroInDungeon.m_placeOfDeath = 5;
		}

		if (finalModifier > 9)
		{
			heroInDungeon.m_hasDungeonBeenBeaten = true;
			heroInDungeon.m_MoneyWon = -(m_currentMoney / 2);
		}
		else
		{
			heroInDungeon.m_finalWords = finalWords;

			int placeOfDeath = heroInDungeon.m_placeOfDeath;
			int guessedFloor = heroInDungeon.m_hero.m_selectedFloor;

			BetReward foundBetReward = finalStatSettings.m_betRewards.Find(betReward => Math.Abs(placeOfDeath - guessedFloor) == betReward.difference);

			if (foundBetReward != null)
			{
				heroInDungeon.m_rewardMultiplier = foundBetReward.multiplier;
				heroInDungeon.m_MoneyWon = heroInDungeon.m_bidAmount * heroInDungeon.m_rewardMultiplier;
			}
			else
			{
				heroInDungeon.m_rewardMultiplier = 0.0f;
				heroInDungeon.m_MoneyWon = 0.0f;
				// No reward
			}
		}

		heroInDungeon.m_haveResultsBeenCalculated = true;
	}

	public void AddPaymentForHero(HeroInDungeon heroInDungeon)
	{
		KeyValuePair<HeroInDungeon, float> dungeonHero = m_dungeonHeroes.Find(hero => hero.Key == heroInDungeon);

		AddMoney(heroInDungeon.m_MoneyWon);
		OnMoneyChanged?.Invoke(m_currentMoney);

		dungeonHero.Key.m_shouldDelete = true;
	}

	private void Update()
	{
		if (m_hasGameStarted)
		{
			if (!m_hasGameEnded)
			{
				m_timeElapsed += Time.deltaTime;	

				for (int i = 0; i < m_dungeonHeroes.Count; ++i)
				{
					if (m_dungeonHeroes[i].Value > 0.0f)
					{
						m_dungeonHeroes[i] = new KeyValuePair<HeroInDungeon, float>(m_dungeonHeroes[i].Key, m_dungeonHeroes[i].Value - Time.deltaTime);

					}

					if (m_dungeonHeroes[i].Value <= 0.0f)
					{
						if (!m_dungeonHeroes[i].Key.m_haveResultsBeenCalculated)
						{
							CalculateHeroResults(m_dungeonHeroes[i].Key);
							OnHeroFinishedDungeon?.Invoke(m_dungeonHeroes[i].Key);
						}
					}
				}

				if (m_dungeonHeroes.Count > 0)
				{
					m_dungeonHeroes.RemoveAll(hero => hero.Key.m_shouldDelete == true);
				}
			}
		}
	}	

}
