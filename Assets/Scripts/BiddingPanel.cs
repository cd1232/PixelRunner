using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BiddingPanel : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_bidAmountText;

	[SerializeField]
	private TextMeshProUGUI m_selectedFloorText;

	[SerializeField]
	private GameObject m_buttonsContainer;

	private List<Toggle> m_allToggles = new List<Toggle>();

	private int m_selectedFloor = -1;
	private int m_bidAmount = -1;

	private void Start()
	{
		Toggle[] foundToggles = m_buttonsContainer.GetComponentsInChildren<Toggle>();
		foreach (Toggle t in foundToggles)
		{
			t.onValueChanged.AddListener(OnToggleChanged);
		}

		m_allToggles.AddRange(foundToggles);
	}
	
	void OnToggleChanged(bool bNewValue)
	{
		if (bNewValue)
		{
			for (int i = 0; i < m_allToggles.Count; ++i)
			{
				if (m_allToggles[i].isOn)
				{
					m_selectedFloor = i + 1;
					m_selectedFloorText.text = "Selected Floor: " + m_selectedFloor;
					GameManager.GetInstance().SetFloorDeathGuess(m_selectedFloor);
					break;
				}
			}
		}
		else
		{
			m_selectedFloorText.text = "Selected Floor: None";
			m_selectedFloor = -1;
			GameManager.GetInstance().SetFloorDeathGuess(m_selectedFloor);
		}
	}

	public void OnBidAmount(int amount)
	{
		m_bidAmount = amount;
		m_bidAmountText.text = "Current Bid Amount: $" + m_bidAmount;
		GameManager.GetInstance().SetBidAmount(amount);
	}
}
