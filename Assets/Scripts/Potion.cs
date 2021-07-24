using UnityEngine;
using System.Collections.Generic;

public class Potion
{
	public Potion(bool shouldGenerateRandom)
	{
		if (shouldGenerateRandom)
		{
			List<HealingIngredient> healingIngredients = GameManager.GetInstance().GetIngredientsOfType<HealingIngredient>();
			List<BuffIngredient> buffIngredients = GameManager.GetInstance().GetIngredientsOfType<BuffIngredient>();
			List<ColorIngredient> colorIngredients = GameManager.GetInstance().GetIngredientsOfType<ColorIngredient>();

			m_healingIngredient = healingIngredients[Random.Range(0, healingIngredients.Count)];
			m_buffIngredient = buffIngredients[Random.Range(0, buffIngredients.Count)];
			m_colorIngredient = colorIngredients[Random.Range(0, colorIngredients.Count)];
		}
	}

	public static int GetNumMatchingIngredidents(Potion p1, Potion p2)
	{
		int numMatching = 0;
		numMatching += p1.m_healingIngredient == p2.m_healingIngredient && p1.m_healingIngredient != null ? 1 : 0;
		numMatching += p1.m_buffIngredient == p2.m_buffIngredient && p1.m_buffIngredient != null ? 1 : 0;
		numMatching += p1.m_colorIngredient == p2.m_colorIngredient && p1.m_colorIngredient != null ? 1 : 0;

		return numMatching;
	}

	public HealingIngredient m_healingIngredient;
	public BuffIngredient m_buffIngredient;
	public ColorIngredient m_colorIngredient;
}
