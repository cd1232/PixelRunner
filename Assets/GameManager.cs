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
	private FinalStatSettings finalStatSettings;

	[Space]

	public Action<Hero> OnDisplayHero;
	public Action OnHideHero;
	public Action<KeyValuePair<HeroInDungeon, float>> OnAddHeroToDungeon;
	public Action<HeroInDungeon> OnHeroFinishedDungeon;
	public Action<float> OnMoneyChanged;
	public Action OnGameEnded;

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
		m_chosenPotion.Reset();

		Hero newHero = GenerateNewHero();
		m_heroes.Add(newHero);
		m_currentHero = newHero;
		OnDisplayHero?.Invoke(newHero);
	}

	Hero GenerateNewHero()
	{
		Hero newHero = new Hero();
		HeroStats newHeroStats = new HeroStats();

		newHeroStats.m_armorType = (ArmorType) Random.Range(0, 3);
		newHeroStats.m_weaponType = (WeaponType) Random.Range(0, 3);

		int heroMaxHP = 4;
		int heroMaxHPLevel = Random.Range(0, 3);
		switch (heroMaxHPLevel)
		{
			case 0:
				heroMaxHP = 4;
				break;
			case 1:
				heroMaxHP = 8;
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
		m_currentMoney += addedAmount;

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
	}

	public void PotionCreated()
	{
		m_currentHero.m_createdPotion = m_chosenPotion;
		// Probably display both of these separately
		int moneyGained = Potion.GetNumMatchingIngredidents(m_currentHero.m_createdPotion, m_currentHero.m_wantedPotion) * m_rightIngredientPayment;


		AddMoney(moneyGained);
		OnMoneyChanged?.Invoke(m_currentMoney);
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

		HealthModifier healthModifier = finalStatSettings.m_healthModifiers.Find(modifier => heroHPCalc >= modifier.min && heroHPCalc <= modifier.max);
		BuffModifier buffModifier = finalStatSettings.m_buffModifiers.Find(modifier => modifier.buffType == heroInDungeon.m_hero.m_createdPotion.m_buffType);

		Debug.Log("Health Modifier: HPCalc is between " + healthModifier.min + " and " + healthModifier.max + " and value is " + healthModifier.modifier);

		Debug.Log("Buff Modifier: Type is " + buffModifier.buffType + " and value is " + buffModifier.modifier);


		ItemModifier itemModifier = new ItemModifier();
		ItemAndBuffModifier itemAndBuffModifier = new ItemAndBuffModifier();

		List<ItemModifier> itemModifiers = finalStatSettings.m_ItemModifiers;

		WeaponType weaponType = heroInDungeon.m_hero.m_heroStats.m_weaponType;
		ArmorType armorType = heroInDungeon.m_hero.m_heroStats.m_armorType;
		foreach (var currentItemModifier in itemModifiers)
		{
			if ((weaponType == currentItemModifier.weaponType && armorType == currentItemModifier.armorType && currentItemModifier.itemModifierState == ItemModifierState.UseBoth) ||
				(weaponType == currentItemModifier.weaponType && currentItemModifier.itemModifierState == ItemModifierState.UseWeapon) ||
				(armorType == currentItemModifier.armorType && currentItemModifier.itemModifierState == ItemModifierState.UseArmor))
			{
				itemModifier = currentItemModifier;

				string debug = "Item Modifier: ";
				switch (itemModifier.itemModifierState)
				{
					case ItemModifierState.UseWeapon:
						debug += itemModifier.weaponType;
						break;
					case ItemModifierState.UseArmor:
						debug += itemModifier.armorType;
						break;
					case ItemModifierState.UseBoth:
						debug += itemModifier.weaponType + " with " + itemModifier.armorType;
						break;
				}

				debug += " and value is " + itemModifier.modifier;
				Debug.Log(debug);
			}
		}

		List<ItemAndBuffModifier> itemAndBuffModifiers = finalStatSettings.m_itemAndBuffModifiers;

		BuffType buffType = heroInDungeon.m_hero.m_createdPotion.m_buffType;

		foreach (var currentItemAndBuffModifier in itemAndBuffModifiers)
		{
			if (buffType == currentItemAndBuffModifier.buffType)
			{
				if ((armorType == currentItemAndBuffModifier.armorType && currentItemAndBuffModifier.itemModifierState == ItemModifierState.UseArmor) ||
					(weaponType == currentItemAndBuffModifier.weaponType && currentItemAndBuffModifier.itemModifierState == ItemModifierState.UseWeapon))
				{
					itemAndBuffModifier = currentItemAndBuffModifier;
					string debug = "Item and Buff Modifier: ";
					if (itemAndBuffModifier.itemModifierState == ItemModifierState.UseArmor)
						debug += itemAndBuffModifier.armorType;
					else
						debug += itemAndBuffModifier.weaponType;

					debug += " with " + itemAndBuffModifier.buffType + " and value is " + itemAndBuffModifier.modifier;
					Debug.Log(debug);
				}
			}
		}

		int finalModifier = healthModifier.modifier + itemModifier.modifier + buffModifier.modifier + itemAndBuffModifier.modifier + Random.Range(-1, 2);
		Debug.Log("Final Modifier: " + finalModifier);

		heroInDungeon.m_placeOfDeath = finalModifier > 0 ? finalModifier : 1;
		if (finalModifier > 10)
		{
			heroInDungeon.m_hasDungeonBeenBeaten = true;
			heroInDungeon.m_MoneyWon = -(m_currentMoney / 2);
		}
		else
		{
			BaseModifier finalWordModifier = healthModifier;

			if (Math.Abs(buffModifier.modifier) > Math.Abs(finalWordModifier.modifier))
			{
				finalWordModifier = buffModifier;
			}

			if (Math.Abs(itemModifier.modifier) > Math.Abs(finalWordModifier.modifier))
			{
				finalWordModifier = itemModifier;
			}

			if (Math.Abs(itemAndBuffModifier.modifier) > Math.Abs(finalWordModifier.modifier))
			{
				finalWordModifier = itemAndBuffModifier;
			}

			string finalWords = "";
			if (finalWordModifier.finalWord >= 0)
			{
				finalWords = finalStatSettings.m_finalWords[finalWordModifier.finalWord];
				heroInDungeon.m_finalWords = finalWords;
			}

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
