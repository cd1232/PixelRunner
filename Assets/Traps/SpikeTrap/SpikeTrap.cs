using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SpikeTrap : TrapBase
{
	private BoxCollider2D m_boxCollider;
	private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
		m_boxCollider = GetComponent<BoxCollider2D>();
		m_animator = GetComponent<Animator>();
    }

	public void CheckTrapCollision()
	{
		List<Collider2D> results = new List<Collider2D>();
		ContactFilter2D filter = new ContactFilter2D();
		filter.useTriggers = true;
		m_boxCollider.OverlapCollider(filter, results);
		foreach (var result in results)
		{
			GameObject resultGameObject = result.gameObject;
			if (resultGameObject.CompareTag(GameTags.s_playerTag))
			{
				PlayerAutoRunner autoRunner = resultGameObject.GetComponent<PlayerAutoRunner>();
				if (autoRunner)
				{
					autoRunner.Kill(gameObject);
				}
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag(GameTags.s_playerTag))
		{
			m_animator.enabled = true;
		}
	}
}
