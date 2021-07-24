using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BaseModifier
{
	public int modifier = 0;
	public int finalWord = -1;
}

[Serializable]
public class HealthModifier : BaseModifier
{
	public int min = 0;
	public int max = 0;
}

[Serializable]
public enum ItemModifierState
{
	UseWeapon,
	UseArmor,
	UseBoth
}

[Serializable]
public class ItemModifier : BaseModifier
{
	public WeaponType weaponType;
	public ArmorType armorType;
	public ItemModifierState itemModifierState;
}

[Serializable]
public class BuffModifier : BaseModifier
{
	public BuffIngredient buffIngredient;
}

[Serializable]
public class ItemAndBuffModifier : BaseModifier
{
	public BuffIngredient buffIngredient;
	public WeaponType weaponType;
	public ArmorType armorType;
	public ItemModifierState itemModifierState;
}

[Serializable]
public class BetReward
{
	public int difference = 0;
	public float multiplier = 1.0f;
}

[Serializable]
public class FinalModifierDeathFloor
{
	public int min = 0;
	public int max = 0;
	public int deathFloor = 0;
}

[CreateAssetMenu(fileName = "finalStatSettings", menuName = "ScriptableObjects/FinalStatSettings", order = 2)]
public class FinalStatSettings : ScriptableObject
{
	public List<string> m_finalWords = new List<string>();
	public List<HealthModifier> m_healthModifiers = new List<HealthModifier>();
	public List<ItemModifier> m_ItemModifiers = new List<ItemModifier>();
	public List<BuffModifier> m_buffModifiers = new List<BuffModifier>();
	public List<ItemAndBuffModifier> m_itemAndBuffModifiers = new List<ItemAndBuffModifier>();

	public List<BetReward> m_betRewards = new List<BetReward>();

	public string m_noHealHint = "No Healing";
	public string m_noBuffHint = "No Buff";
	public string m_noColorHint = "Transparent";

	public List<int> m_heroMaxHPOptions = new List<int>();
	public List<FinalModifierDeathFloor> m_finalModifierDeathFloors = new List<FinalModifierDeathFloor>();
}
