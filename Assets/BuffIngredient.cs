using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "BuffIngredient", menuName = "ScriptableObjects/Ingredients/BuffIngredient", order = 1)]
public class BuffIngredient : Ingredient
{
	public BuffType m_buffType;
}