using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public enum HealPower
{
	Invalid = -1,
	Weak,
	Medium,
	Strong
};

[Serializable]
public enum PotionColor
{
	Invalid = -1,
	Red,
	Blue,
	Yellow
};

[Serializable]
public class Potion
{
	public Potion(HealPower healPower, PotionColor potionColor, int speed)
	{
		m_healPower = healPower;
		m_potionColor = potionColor;
		m_speed = speed;
	}

	public HealPower m_healPower;
	public PotionColor m_potionColor;
	public int m_speed;
	public readonly int m_maxSpeed = 3;
}

public class GameManager : MonoBehaviour
{
	#region Singleton
	public static GameManager GetInstance()
	{
		return Instance;
	}

	private static GameManager Instance;
	#endregion

	[SerializeField]
	private GameObject m_customerPrefab;

	[SerializeField]
	private UI m_uiObject;

	[SerializeField]
	private List<Transform> m_customerQueuePositions = new List<Transform>();

	[SerializeField]
	private Transform m_doorPosition;

	private GameObject m_customerSpawnPosition;

	private List<GameObject> m_customerList = new List<GameObject>();
	private List<GameObject> m_dungeonCustomerList = new List<GameObject>();
	private Dictionary<Transform, GameObject> m_queueCustomerDictonary = new Dictionary<Transform, GameObject>();

	public Action OnPotionCompleted;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		GameObject[] spawnPositions = GameObject.FindGameObjectsWithTag(GameTags.s_spawnPositionTag);
		if (spawnPositions.Length > 0)
		{
			m_customerSpawnPosition = spawnPositions[0];
		}
	}

	private void Start()
	{
		foreach (var customerQueuePosition in m_customerQueuePositions)
		{
			Vector3 ignoreYPosition = customerQueuePosition.position;
			ignoreYPosition.y = -5.5f;
			customerQueuePosition.position = ignoreYPosition;

			m_queueCustomerDictonary[customerQueuePosition] = null;
		}


		SpawnCustomer();
	}

	public void PotionCompleted()
	{
		OnPotionCompleted?.Invoke();
		if (m_customerList.Count > 0)
		{
			m_customerList[0].GetComponent<CustomerController>().SetPositionToMoveTo(m_doorPosition.position);

			m_dungeonCustomerList.Add(m_customerList[0]);
			m_customerList.RemoveAt(0);

			m_queueCustomerDictonary[m_customerQueuePositions[0]] = null;
			MoveUpCustomers();
		}

	}

	void MoveUpCustomers()
	{
		// TODO
		// Just spawn a new customer rn
		SpawnCustomer();
	}


	private void SpawnCustomer()
	{
		if (m_customerPrefab == null)
			return;

		GameObject newCustomer = Instantiate(m_customerPrefab, m_customerSpawnPosition.transform.position, Quaternion.identity);

		// Set where the customer should go to
		Transform moveToPosition = transform;
		bool wasPositionFound = false;
		foreach (KeyValuePair<Transform, GameObject> queuePosition in m_queueCustomerDictonary)
		{
			if (queuePosition.Value == null)
			{
				moveToPosition = queuePosition.Key;
				wasPositionFound = true;
				break;
			}
		}

		if (wasPositionFound)
		{
			m_queueCustomerDictonary[moveToPosition] = newCustomer;
			newCustomer.GetComponent<CustomerController>().SetPositionToMoveTo(moveToPosition.position);
			GenerateRandomCustomerPreferences(newCustomer.GetComponent<CustomerController>());
			newCustomer.GetComponent<CustomerController>().OnCustomerReachedDesk += OnCustomerReachedDesk;
		}

		m_customerList.Add(newCustomer);
	}

	private void GenerateRandomCustomerPreferences(CustomerController newCustomer)
	{
		Potion newPotion = new Potion((HealPower)Random.Range(0, 3), (PotionColor)Random.Range(0, 3), Random.Range(1, 4));
		newCustomer.SetPotion(newPotion);
	}

	void OnCustomerReachedDesk(GameObject customer)
	{
		m_uiObject.OnCustomerReachedDesk(customer.GetComponent<CustomerController>());
	}

}
