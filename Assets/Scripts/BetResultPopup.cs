using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BetResultPopup : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_placeOfDeath;

	[SerializeField]
	private TextMeshProUGUI m_finalWords;

	[SerializeField]
	private TextMeshProUGUI m_heading;

	// Includes bet amount. like $20 on floor 1
	[SerializeField]
	private TextMeshProUGUI m_floorBet;

	[SerializeField]
	private TextMeshProUGUI m_potionStrength;

	[SerializeField]
	private TextMeshProUGUI m_potionBuff;

	[SerializeField]
	private Image m_potionImage;

	[SerializeField]
	private TextMeshProUGUI m_amountWon;

	[SerializeField]
	private Button m_receiveButton;

	[SerializeField]
	private AudioClip m_good;

	[SerializeField]
	private AudioClip m_bad;

	[SerializeField]
	private Customer m_customerContainer;

	[SerializeField]
	private Sprite m_transparentSprite;


	private AudioSource m_audioSource;
	public Action OnReceiveButtonPressed;


	public void Start()
	{
		m_receiveButton.onClick.AddListener(OnClick);
	}

	void OnClick()
	{
		OnReceiveButtonPressed?.Invoke();
	}

	public void ShowInfo(HeroInDungeon heroInDungeon)
	{
		m_audioSource = GetComponent<AudioSource>();

		HeroStats heroStats = heroInDungeon.m_hero.m_heroStats;
		Potion createdPotion = heroInDungeon.m_hero.m_createdPotion;

		m_customerContainer.SetCustomer(heroInDungeon.m_hero);

		m_potionStrength.text = createdPotion.m_healingIngredient ? createdPotion.m_healingIngredient.m_name : "No Healing";
		m_potionBuff.text = createdPotion.m_buffIngredient ? createdPotion.m_buffIngredient.m_name : "No Buff";

		if (createdPotion.m_colorIngredient)
		{
			m_potionImage.sprite = createdPotion.m_colorIngredient.m_potionImage;
		}
		else
		{
			m_potionImage.sprite = m_transparentSprite;
		}

		if (heroInDungeon.m_hero.m_selectedFloor > 0)
		{
			m_floorBet.text = "$" + heroInDungeon.m_bidAmount.ToString("F0") + " on floor " + heroInDungeon.m_hero.m_selectedFloor;
			m_amountWon.text = "Reward: $" + heroInDungeon.m_MoneyWon.ToString("F2");

			if (heroInDungeon.m_placeOfDeath != heroInDungeon.m_hero.m_selectedFloor)
			{
				m_heading.text = "you were wrong!";
			}
			else if (heroInDungeon.m_placeOfDeath == heroInDungeon.m_hero.m_selectedFloor)
			{
				m_heading.text = "you were right!";
			}
		}
		else
		{
			m_heading.text = "Results";
			m_floorBet.text = "No Bet";
			m_amountWon.text = "Reward: $0.00";
		}
	

		if (heroInDungeon.m_hasDungeonBeenBeaten)
		{
			m_placeOfDeath.text = "The hero didn't die!";
			m_amountWon.text = "Lost: $" + Math.Abs(heroInDungeon.m_MoneyWon).ToString("F2");
			m_finalWords.text = "";
			m_audioSource.clip = m_bad;
		}
		else
		{
			m_placeOfDeath.text = "They died on floor " + heroInDungeon.m_placeOfDeath;
			m_finalWords.text = "Final Words: " + heroInDungeon.m_finalWords;

			if (heroInDungeon.m_MoneyWon > 0)
			{
				m_audioSource.clip = m_good;
			}
			else
			{
				m_audioSource.clip = m_bad;
			}
		}

		gameObject.SetActive(true);
		m_audioSource.Play();
	}
}
