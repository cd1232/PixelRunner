using System;
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
	
	private Ingredient m_ingredient;

	private Transform m_parent;

	private int m_indexInGrid = -1;

	public Action<IngredientController> OnIngredientConsumed;

	void Awake()
	{
		m_canvasGroup = GetComponent<CanvasGroup>();
		m_rectTransform = GetComponent<RectTransform>();
		m_ingredientImage = GetComponent<Image>();
	}

	void Start()
	{
		m_parent = transform.parent;
	}

	public Ingredient GetIngredient()
	{
		return m_ingredient;
	}

	private void OnDestroy()
	{
		OnIngredientConsumed?.Invoke(this);
	}

	public void SetupIngredient(Ingredient ingredient, Canvas canvas, int indexInGrid)
	{
		m_indexInGrid = indexInGrid;
		m_canvas = canvas;
		m_ingredient = ingredient;
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
		transform.SetParent(m_parent);		
		transform.SetSiblingIndex(m_indexInGrid);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
	}
}
