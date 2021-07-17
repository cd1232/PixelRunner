using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	

	[SerializeField]
	private GameObject m_playerPrefab;

	private static GameManager Instance;

	private GameObject m_spawnedPlayer;
	private GameObject[] m_spawnPositions;

	public static GameManager GetInstance()
	{
		return Instance;
	}

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		m_spawnPositions = GameObject.FindGameObjectsWithTag(GameTags.s_spawnPositionTag);

		if (m_spawnPositions.Length > 0)
		{
			m_spawnedPlayer = Instantiate(m_playerPrefab, m_spawnPositions[0].transform.position, Quaternion.identity);
		}
	}

	public PlayerAutoRunner GetPlayer()
	{
		return m_spawnedPlayer.GetComponent<PlayerAutoRunner>();
	}

}
