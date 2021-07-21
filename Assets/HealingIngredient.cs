using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "HealingIngredient", menuName = "ScriptableObjects/Ingredients/HealingIngredient", order = 2)]
public class HealingIngredient : Ingredient
{
	public HealingStrength m_healingStrength;
}