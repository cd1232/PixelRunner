using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CustomerState
{
	Spawned,
	TravellingToPotionPickup,
	TravellingToDungeon
};

public class CustomerController : MonoBehaviour
{
	private Vector3 m_positionToMoveTo;

	public Action<GameObject> OnCustomerReachedDesk;

	[SerializeField]
	private float m_speed = 0.2f;

	private bool m_hasPositionToMoveTowards = false;

	[SerializeField]
	private Potion m_wantedPotion;

	public Potion m_potionThatWasMade;
	public int m_floorDeathGuess = -1;

	private CustomerState m_customerState = CustomerState.Spawned;

	private int m_positionInQueue = -1;

    void Update()
    {
        if (m_hasPositionToMoveTowards)
		{
			transform.position = Vector3.MoveTowards(transform.position, m_positionToMoveTo, Time.deltaTime * m_speed);

			if (Vector3.Distance(transform.position, m_positionToMoveTo) < 0.05f)
			{
				m_hasPositionToMoveTowards = false;
				if (m_customerState == CustomerState.TravellingToPotionPickup && m_positionInQueue == 0)
				{
					OnCustomerReachedDesk?.Invoke(gameObject);
				}
				else if (m_customerState == CustomerState.TravellingToDungeon)
				{
					MoveToDungeon();
				}
			}
		}
    }

	public void SetPositionToMoveTo(Vector3 positionToMoveTo, CustomerState newState, int positionInQueue)
	{
		m_customerState = newState;
		m_positionToMoveTo = positionToMoveTo;
		m_hasPositionToMoveTowards = true;
		m_positionInQueue = positionInQueue;
	}

	public int GetPositionInQueue()
	{
		return m_positionInQueue;
	}

	public void SetPotion(Potion wantedPotion)
	{
		m_wantedPotion = wantedPotion;
	}

	public Potion GetWantedPotion()
	{
		return m_wantedPotion;
	}

	public void MoveToDungeon()
	{
		GetComponent<SpriteRenderer>().enabled = false;
	}
}
