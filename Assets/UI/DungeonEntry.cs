using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonEntry : MonoBehaviour
{
	public Image m_customerImage;
	public TextMeshProUGUI m_statusText;
	public TextMeshProUGUI m_floorText;

	public void SetInfo(Image customerImage, string statusText, string floorText)
	{
		//m_customerImage.;
		m_statusText.text = statusText;
		m_floorText.text = floorText;
	}

	public void UpdateFloor(string floorText)
	{
		m_floorText.text = floorText;
	}
}
