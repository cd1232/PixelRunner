using UnityEngine;

[SerializeField]
public class Ingredient : ScriptableObject
{
	public string m_name = "";
	public string m_hint = "";
	public Sprite m_image;
	public Sprite m_potionImage;

	public Color m_hintColor;

	public Vector2 m_positionInBasket = Vector2.zero;
}
