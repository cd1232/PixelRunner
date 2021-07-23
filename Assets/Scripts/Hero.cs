using System;
using UnityEngine;

[Serializable]
public enum WeaponType
{
	EmptyHand,
	Dagger,
	Sword
}

[Serializable]
public enum ArmorType
{
	NakedDisplay,
	LightArmor,
	HeavyArmor
}

[Serializable]
public class HeroStats
{
	public int m_currentHP = 0;
	public int m_maxHP = 0;
	public WeaponType m_weaponType;
	public ArmorType m_armorType;
}


[Serializable]
public class Hero
{
	[SerializeField]
	public HeroStats m_heroStats;
	[SerializeField]
	public Potion m_wantedPotion;
	[SerializeField]
	public Potion m_createdPotion;

	public Sprite m_heroSprite;

	public int m_selectedFloor = 0;
}
