using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
	private enum CustomerState
	{
		Spawned,
		TravellingToPotionPickup,
		TravellingToDungeon
	};


	private Vector3 m_positionToMoveTo;

	public Action<GameObject> OnCustomerReachedDesk;

	[SerializeField]
	private float m_speed = 0.2f;

	private bool m_hasPositionToMoveTowards = false;

	[SerializeField]
	private Potion m_wantedPotion;

	private CustomerState m_customerState = CustomerState.Spawned;

    void Update()
    {
        if (m_hasPositionToMoveTowards)
		{
			transform.position = Vector3.MoveTowards(transform.position, m_positionToMoveTo, Time.deltaTime * m_speed);

			if (Vector3.Distance(transform.position, m_positionToMoveTo) < 0.05f)
			{
				m_hasPositionToMoveTowards = false;
				if (m_customerState == CustomerState.TravellingToPotionPickup)
				{
					OnCustomerReachedDesk?.Invoke(gameObject);
				}
				else
				{
					MoveToDungeon();
				}
			}
		}
    }

	public void SetPositionToMoveTo(Vector3 positionToMoveTo)
	{
		m_customerState++;
		m_positionToMoveTo = positionToMoveTo;
		m_hasPositionToMoveTowards = true;
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
