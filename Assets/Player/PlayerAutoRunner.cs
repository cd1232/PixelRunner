using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoRunner : MonoBehaviour
{
	public enum ERunDirection
	{
		Left = -1,
		Right = 1
	}

	[SerializeField]
	private float m_speed = 200.0f;

	private Rigidbody2D m_rigidBody;
	private ERunDirection m_runDirection = ERunDirection.Right;
	private float m_currentSpeed = 0.0f;


    void Start()
    {
		m_rigidBody = GetComponent<Rigidbody2D>();
		m_currentSpeed = m_speed;
    }

    void FixedUpdate()
    {
		m_rigidBody.velocity = new Vector3(m_currentSpeed * Time.deltaTime * (int)m_runDirection, m_rigidBody.velocity.y, 0.0f);
    }

	public void Flip()
	{
		m_runDirection = m_runDirection == ERunDirection.Right ? ERunDirection.Left : ERunDirection.Right;
		Vector3 myScale = transform.localScale;
		myScale.x *= -1;
		transform.localScale = myScale;
	}

	public void Kill()
	{
		m_currentSpeed = 0.0f;
	}
}
