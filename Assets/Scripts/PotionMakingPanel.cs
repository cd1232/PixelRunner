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
	private GridLayoutGroup m_gridLayout;

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
		foreach (var ingredient in m_allIngredientScriptables)
		{
			GameObject newIngredientUI = Instantiate(m_ingredientUIPrefab, m_gridLayout.transform);
			newIngredientUI.GetComponent<IngredientController>().SetupIngredient(ingredient, m_canvas);
			m_ingredientsDisplayed.Add(newIngredientUI.GetComponent<IngredientController>());
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
		foreach (var ingredient in m_allIngredientScriptables)
		{
			if (m_ingredientsDisplayed.Find(ingredientController => ingredientController.GetIngredient() == ingredient) == null)
			{
				GameObject newIngredientUI = Instantiate(m_ingredientUIPrefab, m_gridLayout.transform);
				newIngredientUI.GetComponent<IngredientController>().SetupIngredient(ingredient, m_canvas);
				m_ingredientsDisplayed.Add(newIngredientUI.GetComponent<IngredientController>());
				numRefilled++;
			}
		}

		GameManager.GetInstance().AddMoney(-(numRefilled * 3));
	}
}
