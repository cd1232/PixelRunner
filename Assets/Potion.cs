using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

[Serializable]
public enum HealingStrength
{
	Weak,
	Medium,
	Strong
};

[Serializable]
public enum PotionColor
{
	Red,
	Green,
	Blue
};

[Serializable]
public enum BuffType
{
	Speed,
	Damage,
	Invincible
};


public class Potion
{
	public Potion()
	{
		m_healingStrength = (HealingStrength)Random.Range(0, 3);
		m_potionColor = (PotionColor)Random.Range(0, 3);
		m_buffType = (BuffType)Random.Range(0, 2);
	}

	public Potion(HealingStrength healingStrength, PotionColor potionColor, BuffType buffType)
	{
		m_healingStrength = healingStrength;
		m_potionColor = potionColor;
		m_buffType = buffType;
	}

	public HealingStrength m_healingStrength;
	public PotionColor m_potionColor;
	public BuffType m_buffType;
}
