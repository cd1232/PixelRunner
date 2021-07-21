using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PotionMaker : MonoBehaviour, IDropHandler
{
	private Potion m_potionBeingCreated;

	private Image m_image;

	[SerializeField]
	private MadePotionsList m_madePotionsList;

	//[SerializeField]
	//private AudioClip m_ingredientAdded;

	private AudioSource m_audioSource;

	private List<TextMeshProUGUI> m_texts = new List<TextMeshProUGUI>();

	private bool[] m_ingredientsAdded = new bool[3];

	void Awake()
	{
		m_image = GetComponent<Image>();
		m_audioSource = GetComponent<AudioSource>();
		m_texts.AddRange(GetComponentsInChildren<TextMeshProUGUI>());
	}

	public void Start()
	{
		m_potionBeingCreated = new Potion(true);
	}

	public void MakePotion()
	{
		if (m_madePotionsList.AddPotion(m_potionBeingCreated))
		{
			m_potionBeingCreated = new Potion(true);
			m_texts[0].text = "None";
			m_texts[1].text = "Nothing";
			m_texts[2].text = "Transparent";
		}
	}

	public void OnDrop(PointerEventData eventData)
	{
		Ingredient ingredientInfo = eventData.pointerDrag.GetComponent<IngredientController>().GetIngredient();
		if (ingredientInfo)
		{
			Destroy(eventData.pointerDrag);
			BuffIngredient buffIngredient = ingredientInfo as BuffIngredient;
			HealingIngredient healingIngredient = ingredientInfo as HealingIngredient;
			ColorIngredient colorIngredient = ingredientInfo as ColorIngredient;

			m_audioSource.Play();

			// What to do if we already have an ingredient
			if (healingIngredient)
			{
				m_potionBeingCreated.m_healingStrength = healingIngredient.m_healingStrength;
				m_texts[0].text = healingIngredient.m_name;
			}
			else if (buffIngredient)
			{
				m_potionBeingCreated.m_buffType = buffIngredient.m_buffType;
				//m_ingredientsAdded[0] = true;
				m_texts[1].text = buffIngredient.m_name;

			}
			else if (colorIngredient)
			{
				m_potionBeingCreated.m_potionColor = colorIngredient.m_potionColor;
				//m_ingredientsAdded[2] = true;
				m_texts[2].text = colorIngredient.m_name;
			}

			//if (m_ingredientsAdded[0] && m_ingredientsAdded[1] && m_ingredientsAdded[2])
			//{
			//	m_madePotionsList.AddPotion(m_potionBeingCreated);
			//	m_potionBeingCreated.Reset();

			//	for (int i = 0; i < m_ingredientsAdded.Length; ++i)
			//	{
			//		m_ingredientsAdded[i] = false;
			//	}
			//}
		}		
	}
}
