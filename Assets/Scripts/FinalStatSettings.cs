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
	public BuffType buffType;
}

[Serializable]
public class ItemAndBuffModifier : BaseModifier
{
	public BuffType buffType;
	public WeaponType weaponType;
	public ArmorType armorType;
	public ItemModifierState itemModifierState;
}

[Serializable]
public class ItemAndItemModifier : BaseModifier
{
	public WeaponType weaponType;
	public ArmorType armorType;
}

[Serializable]
public class BetReward
{
	public int difference = 0;
	public float multiplier = 1.0f;
}

[CreateAssetMenu(fileName = "finalStatSettings", menuName = "ScriptableObjects/FinalStatSettings", order = 2)]
public class FinalStatSettings : ScriptableObject
{
	public List<string> m_finalWords = new List<string>();
	public List<HealthModifier> m_healthModifiers = new List<HealthModifier>();
	public List<ItemModifier> m_ItemModifiers = new List<ItemModifier>();
	public List<BuffModifier> m_buffModifiers = new List<BuffModifier>();
	public List<ItemAndBuffModifier> m_itemAndBuffModifiers = new List<ItemAndBuffModifier>();
	public List<ItemAndItemModifier> m_itemAndItemModifiers = new List<ItemAndItemModifier>();

	public List<BaseModifier> m_baseModifiers = new List<BaseModifier>();

	public List<BetReward> m_betRewards = new List<BetReward>();
}
