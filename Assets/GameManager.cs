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
	public bool m_shouldDelete = false;
}

public class GameManager : MonoBehaviour
{
	//#region Singleton
	//public static GameManager GetInstance()
	//{
	//	return Instance;
	//}

	//private static GameManager Instance;
	//#endregion

	[SerializeField]
	private float m_dungeonCountdown = 3.0f;

	public Action<Hero> OnDisplayHero;
	public Action OnHideHero;
	public Action<KeyValuePair<HeroInDungeon, float>> OnAddHeroToDungeon;
	public Action<HeroInDungeon> OnHeroFinishedDungeon;

	private List<Hero> m_heroes = new List<Hero>();
	private List<KeyValuePair<HeroInDungeon, float>> m_dungeonHeroes = new List<KeyValuePair<HeroInDungeon, float>>();

	[SerializeField]
	private Hero m_currentHero;

	private int m_currentMoney = 50;
	private Potion m_chosenPotion;

	void Awake()
	{
		//if (Instance == null)
		//	Instance = this;
		//else
		//	Destroy(gameObject);
	}

	private void Start()
	{
		m_chosenPotion = new Potion();
		DisplayHero();
	}

	void DisplayHero()
	{
		Hero newHero = GenerateNewHero();
		m_heroes.Add(newHero);
		m_currentHero = newHero;
		OnDisplayHero?.Invoke(newHero);
	}

	Hero GenerateNewHero()
	{
		Debug.Log("Generating new hero");
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

	public int GetCurrentMoney()
	{
		return m_currentMoney;
	}

	public Potion GetMadePotion()
	{
		return m_chosenPotion;
	}

	public void SetCreatedPotion(HealingStrength strength, BuffType buffType, PotionColor potionColor)
	{
		m_chosenPotion.m_healingStrength = strength;
		m_chosenPotion.m_buffType = buffType;
		m_chosenPotion.m_potionColor = potionColor;
	}

	public void SendHeroToDungeon()
	{
		m_currentHero.m_createdPotion = m_chosenPotion;

		HeroInDungeon heroInDungeon = new HeroInDungeon(m_currentHero);
		m_dungeonHeroes.Add(new KeyValuePair<HeroInDungeon, float>(heroInDungeon, m_dungeonCountdown));
		OnAddHeroToDungeon?.Invoke(new KeyValuePair<HeroInDungeon, float>(heroInDungeon, m_dungeonCountdown));

		m_currentHero = null;
		OnHideHero?.Invoke();
		m_heroes.Remove(m_currentHero);
		DisplayHero();
	}

	private void Update()
	{
		for (int i = 0; i < m_dungeonHeroes.Count; ++i)
		{
			if (m_dungeonHeroes[i].Value > 0.0f)
			{
				m_dungeonHeroes[i] = new KeyValuePair<HeroInDungeon, float>(m_dungeonHeroes[i].Key, m_dungeonHeroes[i].Value - Time.deltaTime);

			}

			if (m_dungeonHeroes[i].Value <= 0.0f)
			{
				OnHeroFinishedDungeon?.Invoke(m_dungeonHeroes[i].Key);
				m_dungeonHeroes[i].Key.m_shouldDelete = true;
			}
		}

		if (m_dungeonHeroes.Count > 0)
		{
			m_dungeonHeroes.RemoveAll(hero => hero.Key.m_shouldDelete == true);
		}
	}	

}
