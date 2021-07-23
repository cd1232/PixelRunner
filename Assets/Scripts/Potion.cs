using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

[Serializable]
public enum HealingStrength
{
	None,
	Weak,
	Medium,
	Strong
};

[Serializable]
public enum PotionColor
{
	Transparent,
	Red,
	Green,
	Blue
};

[Serializable]
public enum BuffType
{
	Nothing,
	Speed,
	Damage,
	IronSkin
};

public class Potion
{
	public Potion(bool isNothing)
	{
		if (isNothing)
		{
			Reset();
		}
	}

	public Potion()
	{
		m_healingStrength = (HealingStrength)Random.Range(0, 4);
		m_potionColor = (PotionColor)Random.Range(0, 4);
		m_buffType = (BuffType)Random.Range(0, 4);
	}

	public Potion(HealingStrength healingStrength, PotionColor potionColor, BuffType buffType)
	{
		m_healingStrength = healingStrength;
		m_potionColor = potionColor;
		m_buffType = buffType;
	}

	public static int GetNumMatchingIngredidents(Potion p1, Potion p2)
	{
		int numMatching = 0;
		numMatching += p1.m_healingStrength == p2.m_healingStrength ? 1 : 0;
		numMatching += p1.m_buffType == p2.m_buffType ? 1 : 0;
		numMatching += p1.m_potionColor == p2.m_potionColor ? 1 : 0;

		return numMatching;
	}

	private void Reset()
	{
		m_healingStrength = HealingStrength.None;
		m_potionColor = PotionColor.Transparent;
		m_buffType = BuffType.Nothing;
	}


	// TODO use ingredients instead
	public HealingIngredient healingIngredient;
	public BuffIngredient buffIngredient;
	public ColorIngredient colorIngredient;

	public HealingStrength m_healingStrength;
	public PotionColor m_potionColor;
	public BuffType m_buffType;
}
