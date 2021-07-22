using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BiddingPanel : MonoBehaviour
{
	[SerializeField]
	private Slider m_floorSlider;

	[SerializeField]
	private Button m_bid10Button;

	[SerializeField]
	private Button m_bid20Button;

	[SerializeField]
	private Button m_bid30Button;

	[SerializeField]
	private Sprite m_bid10Up;

	[SerializeField]
	private Sprite m_bid20Up;

	[SerializeField]
	private Sprite m_bid30Up;

	[SerializeField]
	private Sprite m_bid10Down;

	[SerializeField]
	private Sprite m_bid20Down;

	[SerializeField]
	private Sprite m_bid30Down;

	private int m_selectedFloor = 0;
	private int m_bidAmount = -1;

	private void Awake()
	{
		m_bid10Button.GetComponent<Image>().sprite = m_bid10Down;
		m_bid20Button.GetComponent<Image>().sprite = m_bid20Down;
		m_bid30Button.GetComponent<Image>().sprite = m_bid30Down;
	}

	public void OnFloorChanged(float value)
	{
		Debug.Log("Setting floor to: " + value.ToString("F0"));
		m_selectedFloor = (int)value;

		GameManager.GetInstance().SetFloorDeathGuess(m_selectedFloor);
	}

	public void OnBidAmount(int amount)
	{
		m_bidAmount = amount;

		m_bid10Button.GetComponent<Image>().sprite = m_bid10Down;
		m_bid20Button.GetComponent<Image>().sprite = m_bid20Down;
		m_bid30Button.GetComponent<Image>().sprite = m_bid30Down;
		if (amount == 10)
		{
			m_bid10Button.GetComponent<Image>().sprite = m_bid10Up;
																
		}														
		else if (amount == 20)									
		{														
			m_bid20Button.GetComponent<Image>().sprite = m_bid20Up;
																
		}														
		else if (amount == 30)									
		{														
			m_bid30Button.GetComponent<Image>().sprite = m_bid30Up;

		}

		GameManager.GetInstance().SetBidAmount(amount);
	}

	public void Reset()
	{
		m_selectedFloor = 0;
		m_floorSlider.value = 0;
	}
}
