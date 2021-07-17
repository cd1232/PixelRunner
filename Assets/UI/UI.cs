using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
	public void Start()
	{
		GameManager gameManager = GameManager.GetInstance();
		PlayerAutoRunner player = gameManager.GetPlayer();

		player.OnPlayerDeath += OnPlayerDeath;
	}

	private void OnPlayerDeath(GameObject obj)
	{
		//Show kill reason
	}

	public void PlayerJump()
	{
		GameManager gameManager = GameManager.GetInstance();
		PlayerAutoRunner player = gameManager.GetPlayer();
		player.Jump();
	}

	public void PlayerStop()
	{
		GameManager gameManager = GameManager.GetInstance();
		PlayerAutoRunner player = gameManager.GetPlayer();
		player.Stop();
	}

	public void PlayerReverse()
	{
		GameManager gameManager = GameManager.GetInstance();
		PlayerAutoRunner player = gameManager.GetPlayer();
		player.Flip();
	}

}
