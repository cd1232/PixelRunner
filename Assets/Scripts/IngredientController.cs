using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientController : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	private Canvas m_canvas;

	private CanvasGroup m_canvasGroup;
	private RectTransform m_rectTransform;
	private Image m_ingredientImage;

	private TextMeshProUGUI m_text;

	private GridLayoutGroup gridLayoutGroup;

	private Ingredient m_ingredient;

	void Awake()
	{
		m_canvasGroup = GetComponent<CanvasGroup>();
		m_rectTransform = GetComponent<RectTransform>();
		m_ingredientImage = GetComponent<Image>();
		m_text = GetComponentInChildren<TextMeshProUGUI>();
	}

	void Start()
	{
		gridLayoutGroup = GetComponentInParent<GridLayoutGroup>();
	}

	public Ingredient GetIngredient()
	{
		return m_ingredient;
	}

	public void SetupIngredient(Ingredient ingredient, Canvas canvas)
	{
		m_canvas = canvas;
		m_ingredient = ingredient;
		m_text.text = ingredient.m_name;

		m_ingredientImage.sprite = ingredient.m_image;
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
		transform.SetParent(gridLayoutGroup.transform);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
	}
}
