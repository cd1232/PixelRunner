using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BiddingPanel : MonoBehaviour
{
	private List<Toggle> m_allToggles = new List<Toggle>();

	private void Start()
	{
		Toggle[] foundToggles = GetComponentsInChildren<Toggle>();
		foreach (Toggle t in foundToggles)
		{
			t.onValueChanged.AddListener(OnToggleChanged);
		}
	}
	
	void OnToggleChanged(bool bNewValue)
	{
		if (bNewValue)
		{

		}
	}
}
