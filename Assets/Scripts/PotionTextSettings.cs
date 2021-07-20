using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PotionColorText
{
	public PotionColor color;
	public string text;
}

[Serializable]
public class HealPowerText
{
	public HealingStrength healPower;
	public string text;
}

[Serializable]
public class BuffText
{
	public BuffType buffType;
	public string text;
}


[CreateAssetMenu(fileName = "potionTextSettings", menuName = "ScriptableObjects/PotionTextSettings", order = 1)]
public class PotionTextSettings : ScriptableObject
{
	public string m_startText;
	
	public List<HealPowerText> m_healPowerText;
	public List<BuffText> m_buffText;
	public List<PotionColorText> m_potionColorText;
}

