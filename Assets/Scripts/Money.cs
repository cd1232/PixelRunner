using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
	[SerializeField]
	private AudioClip m_moneyAdded;

	[SerializeField]
	private AudioClip m_moneyLost;

	private AudioSource m_audioSource;

	private void Awake()
	{
		m_audioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		GameManager.GetInstance().OnMoneyAdded += PlayMoneyAdded;
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
