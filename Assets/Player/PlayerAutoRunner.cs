using System;
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

	public event Action<GameObject> OnPlayerDeath;

	[SerializeField]
	private float m_stopTime = 5.0f;

	[SerializeField]
	private float m_jumpForce = 400.0f;

	[SerializeField]
	private float m_speed = 200.0f;

	[SerializeField]
	private ParticleSystem m_bloodSpatter;

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

	public void Jump()
	{
		m_rigidBody.AddForce(new Vector2(0.0f ,m_jumpForce), ForceMode2D.Force);
	}

	public void Stop()
	{
		StartCoroutine(StopPlayer());
	}

	public void Flip()
	{
		m_runDirection = m_runDirection == ERunDirection.Right ? ERunDirection.Left : ERunDirection.Right;
		Vector3 myScale = transform.localScale;
		myScale.x *= -1;
		transform.localScale = myScale;
	}

	public void Kill(GameObject killer)
	{
		OnPlayerDeath?.Invoke(killer);
		m_bloodSpatter.Play();
		m_currentSpeed = 0.0f;
	}

	IEnumerator StopPlayer()
	{
		m_currentSpeed = 0.0f;
		yield return new WaitForSeconds(m_stopTime);
		m_currentSpeed = m_speed;
	}
}
