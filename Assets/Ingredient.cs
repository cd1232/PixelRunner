using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum IngredientEffect
{
	Health,
	Buff,
	Color
}

public class Ingredient : ScriptableObject
{
	public string m_name = "";
	public Sprite m_image;
}
