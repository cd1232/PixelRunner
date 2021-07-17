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
	public HealPower healPower;
	public string text;
}

[Serializable]
public class SpeedText
{
	public int speed;
	public string text;
}


[CreateAssetMenu(fileName = "potionTextSettings", menuName = "ScriptableObjects/PotionTextSettings", order = 1)]
public class PotionTextSettings : ScriptableObject
{
	public string m_startText;
	
	public List<PotionColorText> m_potionColorText;
	public List<HealPowerText> m_healPowerText;
	public List<SpeedText> m_speedText;

}

