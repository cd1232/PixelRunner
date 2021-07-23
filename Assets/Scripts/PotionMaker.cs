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
	private Sprite m_damageSprite;

	[SerializeField]
	private Sprite m_speedSprite;

	[SerializeField]
	private Sprite m_invincibleSprite;

	[SerializeField]
	private Sprite m_transparentSprite;

	[SerializeField]
	private Sprite m_redSprite;

	[SerializeField]
	private Sprite m_greenSprite;

	[SerializeField]
	private Sprite m_blueSprite;

	private AudioSource m_audioSource;
	private Potion m_potionBeingCreated;

	private List<TextMeshProUGUI> m_texts = new List<TextMeshProUGUI>();

	void Awake()
	{
		m_audioSource = GetComponent<AudioSource>();
	}

	public void Start()
	{
		m_potionBeingCreated = new Potion(true);
		m_buffImage.gameObject.SetActive(false);
	}

	public void MakePotion()
	{
		if (m_madePotionsList.AddPotion(m_potionBeingCreated))
		{
			m_potionBeingCreated = new Potion(true);
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
				m_potionBeingCreated.m_healingStrength = healingIngredient.m_healingStrength;
			}
			else if (buffIngredient)
			{
				m_potionBeingCreated.m_buffType = buffIngredient.m_buffType;

				m_buffImage.gameObject.SetActive(true);
				m_buffImage.sprite = buffIngredient.m_potionImage;

				switch (m_potionBeingCreated.m_buffType)
				{
					case BuffType.Nothing:
						m_buffImage.gameObject.SetActive(false);
						break;
					case BuffType.Speed:
						m_buffImage.sprite = m_speedSprite;
						break;
					case BuffType.Damage:
						m_buffImage.sprite = m_damageSprite;
						break;
					case BuffType.IronSkin:
						m_buffImage.sprite = m_invincibleSprite;
						break;
				}

			}
			else if (colorIngredient)
			{
				m_potionBeingCreated.m_potionColor = colorIngredient.m_potionColor;
				m_image.sprite = colorIngredient.m_potionImage;
			}
		}		
	}
}
