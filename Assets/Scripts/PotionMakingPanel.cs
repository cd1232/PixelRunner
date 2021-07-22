using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionMakingPanel : MonoBehaviour
{
	[SerializeField]
	private List<Ingredient> m_allIngredientScriptables;

	[SerializeField]
	private GameObject m_ingredientUIPrefab;

	[SerializeField]
	private GameObject m_gridLayout;

	[SerializeField]
	private Canvas m_canvas;

	[SerializeField]
	private Button m_refillButton;

	private List<IngredientController> m_ingredientsDisplayed = new List<IngredientController>();

	private void Start()
	{
		GameManager.GetInstance().OnPotionGiven += HideRefillButton;
		GameManager.GetInstance().OnDisplayHero += ShowRefillButton;

		m_refillButton.onClick.AddListener(OnRefillButtonPressed);

		//Add all ingredients to grid panel
		int index = 0;
		foreach (var ingredient in m_allIngredientScriptables)
		{
			GameObject newIngredientUI = Instantiate(m_ingredientUIPrefab, m_gridLayout.transform);
			newIngredientUI.GetComponent<IngredientController>().SetupIngredient(ingredient, m_canvas, index);
			m_ingredientsDisplayed.Add(newIngredientUI.GetComponent<IngredientController>());
			index++;
		}
	}

	void HideRefillButton()
	{
		m_refillButton.gameObject.SetActive(false);
	}

	void ShowRefillButton(Hero hero)
	{
		m_refillButton.gameObject.SetActive(true);
	}

	void OnRefillButtonPressed()
	{
		int numRefilled = 0;
		int indexInGrid = 0;

		foreach (var ingredient in m_ingredientsDisplayed)
		{
			Debug.Log("Ingredient Displayed: " + ingredient.GetIngredient().m_name);
		}

		foreach (var ingredient in m_allIngredientScriptables)
		{
			// If can't find the ingredient in the disaplyed ingredients
			IngredientController ic = m_ingredientsDisplayed.Find(ingredientController => ingredientController.GetIngredient().m_name == ingredient.m_name);

			if (m_ingredientsDisplayed.Find(ingredientController => ingredientController.GetIngredient().m_name == ingredient.m_name) == null)
			{
				Debug.Log("Couldn't find " + ingredient.m_name);
				GameObject newIngredientUI = Instantiate(m_ingredientUIPrefab, m_gridLayout.transform);
				newIngredientUI.GetComponent<IngredientController>().SetupIngredient(ingredient, m_canvas, indexInGrid);
				newIngredientUI.transform.SetSiblingIndex(indexInGrid);
				m_ingredientsDisplayed.Add(newIngredientUI.GetComponent<IngredientController>());
				numRefilled++;
			}
			indexInGrid++;
		}

		GameManager.GetInstance().AddMoney(-(numRefilled * 3));
	}
}
