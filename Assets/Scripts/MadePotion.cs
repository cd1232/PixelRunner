using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MadePotion : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	[SerializeField]
	private Image m_buffImage;

	[HideInInspector]
	public Potion m_potion;

	private Canvas m_canvas;

	private CanvasGroup m_canvasGroup;
	private RectTransform m_rectTransform;
	private Image m_potionImage;

	private List<TextMeshProUGUI> m_texts = new List<TextMeshProUGUI>();

	private Button m_destoryButton;

	private MadePotionsList m_madePotionsParent;

	private Transform m_potionContainer;

	private Vector3 m_oldAnchoredPosition;

	void Awake()
	{
		m_canvasGroup = GetComponent<CanvasGroup>();
		m_rectTransform = GetComponent<RectTransform>();
		m_potionImage = GetComponent<Image>();
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
		m_oldAnchoredPosition = m_rectTransform.anchoredPosition;

		m_potionContainer = GetComponentInParent<Transform>();
		m_canvas = canvas;
		m_potion = potion;
		m_madePotionsParent = madePotionsParent;

		m_texts[0].text = m_potion.m_healingIngredient.m_name;
		m_texts[1].text = m_potion.m_buffIngredient.m_name;

		m_buffImage.sprite = m_potion.m_buffIngredient.m_potionImage;
		m_potionImage.sprite = m_potion.m_colorIngredient.m_potionImage;
	}

	private void OnDestroy()
	{
		m_madePotionsParent.RemovePotion(this);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		m_canvasGroup.blocksRaycasts = false;
		m_canvasGroup.alpha = 0.6f;
	}

	public void OnDrag(PointerEventData eventData)
	{
		m_rectTransform.anchoredPosition += eventData.delta / m_canvas.scaleFactor;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		m_canvasGroup.blocksRaycasts = true;
		m_canvasGroup.alpha = 1.0f;
		m_rectTransform.anchoredPosition = m_oldAnchoredPosition;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
	}
}
