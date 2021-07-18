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

	[SerializeField]
	private float m_minSpawnTimeBetweenCustomers = 2.0f;

	[SerializeField]
	private float m_maxSpawnTimeBetweenCustomers = 4.0f;

	private float m_spawnTimeTimer = 0.0f;

	private GameObject m_customerSpawnPosition;

	private const int customerListSize = 5;
	GameObject[] m_customerList = new GameObject[customerListSize];
	private List<GameObject> m_dungeonCustomerList = new List<GameObject>();
	private Dictionary<Transform, GameObject> m_queueCustomerDictonary = new Dictionary<Transform, GameObject>();


	public Action OnPotionCompleted;
	public Action<GameObject> OnNewDungeonCustomer;

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
	}

	private void Update()
	{
		if (!IsQueueFull())
		{
			if (m_spawnTimeTimer > 0.0f)
			{
				m_spawnTimeTimer -= Time.deltaTime;
			}

			if (m_spawnTimeTimer <= 0.0f)
			{
				SpawnCustomer();
				m_spawnTimeTimer = Random.Range(m_minSpawnTimeBetweenCustomers, m_maxSpawnTimeBetweenCustomers);
			}
		}
	}

	public void PotionCompleted(Potion potionMade, int floorGuess)
	{
		OnPotionCompleted?.Invoke();
		if (m_customerList[0] != null)
		{
			CustomerController customerController = m_customerList[0].GetComponent<CustomerController>();

			customerController.SetPositionToMoveTo(m_doorPosition.position, CustomerState.TravellingToDungeon, -1);
			customerController.m_potionThatWasMade = potionMade;
			customerController.m_floorDeathGuess = floorGuess;

			m_dungeonCustomerList.Add(m_customerList[0]);
			OnNewDungeonCustomer?.Invoke(m_customerList[0]);

			m_customerList[0] = null;
			m_queueCustomerDictonary[m_customerQueuePositions[0]] = null;
			MoveUpCustomers();
		}

	}

	void MoveUpCustomers()
	{
		for (int i = 0; i < customerListSize - 1; ++i)
		{
			if (m_customerList[i + 1] != null)
			{
				// i will be max 3 and i+1 will be 4 (the last position in the queue)
				m_customerList[i] = m_customerList[i + 1];
				m_customerList[i + 1] = null;
				m_queueCustomerDictonary[m_customerQueuePositions[i]] = m_customerList[i];
				m_queueCustomerDictonary[m_customerQueuePositions[i + 1]] = null;

				CustomerController newCustomerController = m_customerList[i].GetComponent<CustomerController>();
				Vector3 newPosition = m_customerQueuePositions[i].position;

				Debug.Log("Setting new position in queue to " + (newCustomerController.GetPositionInQueue() - 1).ToString());
				newCustomerController.SetPositionToMoveTo(newPosition, CustomerState.TravellingToPotionPickup, newCustomerController.GetPositionInQueue() - 1);
			}
		}
	}


	private bool IsQueueFull()
	{
		for (int i = 0; i < 5; ++i)
		{
			if (m_customerList[i] == null)
			{
				return false;
			}
		}

		return true;
	}


	private void SpawnCustomer()
	{
		if (m_customerPrefab == null)
			return;

		GameObject newCustomer = Instantiate(m_customerPrefab, m_customerSpawnPosition.transform.position, Quaternion.identity);

		// Set where the customer should go to
		Transform moveToPosition = transform;
		bool wasPositionFound = false;

		int positionInQueue = 0;
		foreach (KeyValuePair<Transform, GameObject> queuePosition in m_queueCustomerDictonary)
		{
			if (queuePosition.Value == null)
			{
				moveToPosition = queuePosition.Key;
				wasPositionFound = true;
				break;
			}
			positionInQueue++;
		}

		if (wasPositionFound)
		{
			m_queueCustomerDictonary[moveToPosition] = newCustomer;
			CustomerController newCustomerController = newCustomer.GetComponent<CustomerController>();

			GenerateRandomCustomerPreferences(newCustomerController);
			newCustomerController.SetPositionToMoveTo(moveToPosition.position, CustomerState.TravellingToPotionPickup, positionInQueue);
			newCustomerController.OnCustomerReachedDesk += OnCustomerReachedDesk;
		}

		m_customerList[positionInQueue] = newCustomer;
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
