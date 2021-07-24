using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PotionMaker : MonoBehaviour, IDropHandler
{
	[SerializeField]
	private Image m_image;

	[SerializeField]
	private Image m_buffImage;

	[SerializeField]
	private MadePotionsList m_madePotionsList;

	[SerializeField]
	private Sprite m_transparentSprite;

	private AudioSource m_audioSource;
	private Potion m_potionBeingCreated;

	private List<TextMeshProUGUI> m_texts = new List<TextMeshProUGUI>();

	void Awake()
	{
		m_audioSource = GetComponent<AudioSource>();
	}

	public void Start()
	{
		m_potionBeingCreated = new Potion(false);
		m_buffImage.gameObject.SetActive(false);
	}

	public void MakePotion()
	{
		if (m_madePotionsList.AddPotion(m_potionBeingCreated))
		{
			m_potionBeingCreated = new Potion(false);
			m_buffImage.gameObject.SetActive(false);
			m_image.sprite = m_transparentSprite;
		}
	}

	public void OnDrop(PointerEventData eventData)
	{
		if (!eventData.pointerDrag.GetComponent<IngredientController>())
		{
			return;
		}

		Ingredient ingredientInfo = eventData.pointerDrag.GetComponent<IngredientController>().GetIngredient();
		if (ingredientInfo)
		{
			Destroy(eventData.pointerDrag);
			BuffIngredient buffIngredient = ingredientInfo as BuffIngredient;
			HealingIngredient healingIngredient = ingredientInfo as HealingIngredient;
			ColorIngredient colorIngredient = ingredientInfo as ColorIngredient;

			m_audioSource.Play();

			if (healingIngredient)
			{
				m_potionBeingCreated.m_healingIngredient = healingIngredient;
			}
			else if (buffIngredient)
			{
				m_potionBeingCreated.m_buffIngredient = buffIngredient;
				m_buffImage.gameObject.SetActive(true);
				m_buffImage.sprite = buffIngredient.m_potionImage;

			}
			else if (colorIngredient)
			{
				m_potionBeingCreated.m_colorIngredient = colorIngredient;
				m_image.sprite = colorIngredient.m_potionImage;
			}
		}		
	}
}
