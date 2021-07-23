using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
	[SerializeField]
	private AudioClip m_moneyAdded;

	[SerializeField]
	private AudioClip m_moneyLost;

	[SerializeField]
	private GameObject m_newMoneyPrefab;

	[SerializeField]
	private GameObject m_containerToSpawnIn;

	private AudioSource m_audioSource;
	private TextMeshProUGUI m_text;

	private float moneyToShow = 0.0f;

	private void Awake()
	{
		m_text = GetComponent<TextMeshProUGUI>();
		m_audioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		GameManager gameManager = GameManager.GetInstance();

		gameManager.OnMoneyChanged += OnMoneyChanged;
		gameManager.OnMoneyAdded += PlayMoneyAdded;

		m_text.text = "$" + gameManager.GetCurrentMoney().ToString("F2");
	}

	void OnMoneyChanged(float previousMoney, float newMoney)
	{
		float difference = newMoney - previousMoney;
		// 30  20 = 10

		if (difference != 0)
		{
			GameObject newMoneySpawned = Instantiate(m_newMoneyPrefab, m_containerToSpawnIn.transform);

			if (difference > 0)
			{				
				newMoneySpawned.GetComponent<TextMeshProUGUI>().text = "+$" + difference.ToString("F2");
			}
			else if (difference < 0)
			{
				newMoneySpawned.GetComponent<TextMeshProUGUI>().text = "-$" + System.Math.Abs(difference).ToString("F2");
				newMoneySpawned.GetComponent<TextMeshProUGUI>().color = Color.red;
			}

			PlayNewMoneySequence(newMoneySpawned);
		}

		m_text.text = "$" + newMoney.ToString("F2");
	}

	void PlayNewMoneySequence(GameObject newMoney)
	{
		Sequence mySequence = DOTween.Sequence();
		// Add a movement tween at the beginning
		mySequence.Append(newMoney.GetComponent<CanvasGroup>().DOFade(1.0f, 0.2f));
		mySequence.Append(newMoney.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 59.0f), 0.5f));
		mySequence.Insert(0.2f, newMoney.GetComponent<CanvasGroup>().DOFade(0.0f, 0.4f));

		mySequence.Play();
	}

	void PlayMoneyAdded()
	{
		m_audioSource.clip = m_moneyAdded;
		m_audioSource.Play();
	}

	void PlayMoneyLost()
	{
		m_audioSource.clip = m_moneyLost;
		m_audioSource.Play();
	}
}
