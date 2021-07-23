using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[SerializeField]
public class Ingredient : ScriptableObject
{
	public string m_name = "";
	public Sprite m_image;
	public Sprite m_potionImage;

	public Vector2 m_positionInBasket = Vector2.zero;
}
