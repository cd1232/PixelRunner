using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MadePotion : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	public Potion m_potion;

	private Canvas m_canvas;

	private CanvasGroup m_canvasGroup;
	private RectTransform m_rectTransform;
	private Image m_potionImage;

	private List<TextMeshProUGUI> m_texts = new List<TextMeshProUGUI>();

	private HorizontalLayoutGroup m_horizontalLayoutGroup;

	private Button m_destoryButton;

	private MadePotionsList m_madePotionsParent;


	// TODO duplicated from ingredient controller

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
		m_horizontalLayoutGroup = GetComponentInParent<HorizontalLayoutGroup>();
		m_canvas = canvas;
		m_potion = potion;
		m_madePotionsParent = madePotionsParent;

		m_texts[0].text = m_potion.m_healingStrength.ToString();
		m_texts[1].text = m_potion.m_buffType.ToString();
		m_texts[2].text = m_potion.m_potionColor.ToString();
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
