using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Customer : MonoBehaviour, IDropHandler
{
	[SerializeField]
	private TextMeshProUGUI m_customerComments;

	[SerializeField]
	private Image m_weaponImage;

	[SerializeField]
	private Image m_armorImage;

	[SerializeField]
	private Slider m_healthBar;

	[SerializeField]
	private Image m_heroImage;

	[SerializeField]
	private Sprite m_swordSprite;

	[SerializeField]
	private Sprite m_daggerSprite;

	[SerializeField]
	private Sprite m_unarmedSprite;


	[SerializeField]
	private Sprite m_lightArmorSprite;

	[SerializeField]
	private Sprite m_heavyArmorSprite;

	[SerializeField]
	private Sprite m_noArmorSprite;

	[SerializeField]
	private PotionTextSettings m_potionTextSettings;

	private Potion m_wantedPotion;

	private AudioSource m_audioSource;

	private string m_originalCustomerComments;

	private bool m_hasReceivedPotion = false;

	private void Awake()
	{
		m_audioSource = GetComponent<AudioSource>();
	}

	public void Start()
	{
		GameManager.GetInstance().OnGameEnded += OnGameEnded;
	}

	void OnGameEnded()
	{
		if (m_customerComments)
		{
			m_customerComments.text = "";
		}
	}

	public void OnDrop(PointerEventData eventData)
	{
		if (m_hasReceivedPotion)
		{
			return;	
		}

		IngredientController ingredientController = eventData.pointerDrag.GetComponent<IngredientController>();
		if (ingredientController != null)
		{
			if (m_customerComments)
			{
				StartCoroutine(ShowIngredientComment());
				// return to original comments
				return;
			}
		}

		MadePotion givenPotion = eventData.pointerDrag.GetComponent<MadePotion>();
		if (givenPotion != null)
		{
			m_audioSource.Play();

			Potion potion = givenPotion.m_potion;

			if (m_customerComments)
			{
				int matchingIngredients = Potion.GetNumMatchingIngredidents(potion, m_wantedPotion);
				if (matchingIngredients == 3)
				{
					m_customerComments.text = "Thanks this is exactly what I wanted";
				}
				else if (matchingIngredients == 2)
				{
					m_customerComments.text = "This is kind of what I wanted I guess";
				}
				else if (matchingIngredients == 1)
				{
					m_customerComments.text = "At least you got something right..";
				}
				else
				{
					m_customerComments.text = "Did you even listen to what I wanted?";
				}
			}			

			m_hasReceivedPotion = true;
			GameManager.GetInstance().SetCreatedPotion(potion);
			Destroy(givenPotion.gameObject);
		}
	}

	IEnumerator ShowIngredientComment()
	{
		m_customerComments.text = "I don't want an ingredient...";
		yield return new WaitForSeconds(3.0f);
		m_customerComments.text = m_originalCustomerComments;
	}

	public void SetCustomer(Hero hero)
	{
		if (hero != null)
		{
			m_hasReceivedPotion = false;
			m_wantedPotion = hero.m_wantedPotion;
			HeroStats stats = hero.m_heroStats;

			string startText = m_potionTextSettings.m_startText;
			string healingStrengthText = m_wantedPotion.m_healingIngredient != null ? m_wantedPotion.m_healingIngredient.m_hint : "no healing";
			string buffText = m_wantedPotion.m_buffIngredient != null ? m_wantedPotion.m_buffIngredient.m_hint : "no buff";
			string potionColorText = m_wantedPotion.m_colorIngredient != null ? m_wantedPotion.m_colorIngredient.m_hint : "no color";

			Color healingStrengthHintColor = m_wantedPotion.m_healingIngredient.m_hintColor;
			Color buffHintColor = m_wantedPotion.m_buffIngredient.m_hintColor;
			Color potionHintColor = m_wantedPotion.m_colorIngredient.m_hintColor;
			
			string textToDisplay = startText + "<#" + ColorUtility.ToHtmlStringRGB(healingStrengthHintColor) + ">" + healingStrengthText + "</color> and <#" + ColorUtility.ToHtmlStringRGB(buffHintColor) +">" + buffText + "</color>.";
			textToDisplay += " Can you make it <#" + ColorUtility.ToHtmlStringRGB(potionHintColor) + ">" + potionColorText + "</color>?";

			m_originalCustomerComments = textToDisplay;

			if (m_customerComments)
			{
				m_customerComments.text = textToDisplay;
			}

			m_heroImage.sprite = hero.m_heroSprite;

			switch (stats.m_weaponType)
			{
				case WeaponType.EmptyHand:
					m_weaponImage.sprite = m_unarmedSprite;
					break;
				case WeaponType.Dagger:
					m_weaponImage.sprite = m_daggerSprite;
					break;
				case WeaponType.Sword:
					m_weaponImage.sprite = m_swordSprite;
					break;
				default:
					break;
			}

			switch (stats.m_armorType)
			{
				case ArmorType.NakedDisplay:
					m_armorImage.sprite = m_noArmorSprite;
					break;
				case ArmorType.LightArmor:
					m_armorImage.sprite = m_lightArmorSprite;
					break;
				case ArmorType.HeavyArmor:
					m_armorImage.sprite = m_heavyArmorSprite;
					break;
			}

			TextMeshProUGUI healthBarText = m_healthBar.GetComponentInChildren<TextMeshProUGUI>();
			if (healthBarText)
			{
				healthBarText.text = stats.m_currentHP + "/" + stats.m_maxHP;					
			}

			m_healthBar.value = (float)stats.m_currentHP / (float)stats.m_maxHP;
		}
		else
		{
			if (m_customerComments)
			{
				m_customerComments.text = "";
			}
		}

	}
}
