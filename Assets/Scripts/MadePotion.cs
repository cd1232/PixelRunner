using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MadePotion : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
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

	public Potion m_potion;

	private Canvas m_canvas;

	private CanvasGroup m_canvasGroup;
	private RectTransform m_rectTransform;
	private Image m_potionImage;

	[SerializeField]
	private Image m_buffImage;

	private List<TextMeshProUGUI> m_texts = new List<TextMeshProUGUI>();

	private HorizontalLayoutGroup m_horizontalLayoutGroup;

	private Button m_destoryButton;

	private MadePotionsList m_madePotionsParent;


	void Awake()
	{
		m_canvasGroup = GetComponent<CanvasGroup>();
		m_rectTransform = GetComponent<RectTransform>();
		m_potionImage = GetComponent<Image>();
		m_buffImage = GetComponentInChildren<Image>();
		m_destoryButton = GetComponentInChildren<Button>();
		m_texts.AddRange(GetComponentsInChildren<TextMeshProUGUI>());
	}

	void Start()
	{
		m_destoryButton.onClick.AddListener(OnDestroyButton);
	}

	void OnDestroyButton()
	{
		Destroy(gameObject);
	}

	public void SetupMadePotion(Potion potion, Canvas canvas, MadePotionsList madePotionsParent)
	{
		m_horizontalLayoutGroup = GetComponentInParent<HorizontalLayoutGroup>();
		m_canvas = canvas;
		m_potion = potion;
		m_madePotionsParent = madePotionsParent;

		m_texts[0].text = m_potion.m_healingStrength.ToString();
		m_texts[1].text = m_potion.m_buffType.ToString();
		m_texts[2].text = m_potion.m_potionColor.ToString();


		m_buffImage.gameObject.SetActive(true);
		switch (m_potion.m_buffType)
		{
			case BuffType.Nothing:
				//m_buffImage.gameObject.SetActive(false);
				break;
			case BuffType.Speed:
				m_buffImage.sprite = m_speedSprite;
				break;
			case BuffType.Damage:
				m_buffImage.sprite = m_damageSprite;
				break;
			case BuffType.HardenedSkin:
				m_buffImage.sprite = m_invincibleSprite;
				break;
		}


		switch (m_potion.m_potionColor)
		{
			case PotionColor.Transparent:
				m_potionImage.sprite = m_transparentSprite;
				break;
			case PotionColor.Red:
				m_potionImage.sprite = m_redSprite;
				break;
			case PotionColor.Green:
				m_potionImage.sprite = m_greenSprite;
				break;
			case PotionColor.Blue:
				m_potionImage.sprite = m_blueSprite;
				break;
		}
	}

	private void OnDestroy()
	{
		m_madePotionsParent.RemovePotion(this);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		m_canvasGroup.blocksRaycasts = false;
		m_canvasGroup.alpha = 0.6f;
		transform.SetParent(m_canvas.transform);
	}

	public void OnDrag(PointerEventData eventData)
	{
		m_rectTransform.anchoredPosition += eventData.delta / m_canvas.scaleFactor;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		m_canvasGroup.blocksRaycasts = true;
		m_canvasGroup.alpha = 1.0f;
		transform.SetParent(m_horizontalLayoutGroup.transform);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
	}
}
