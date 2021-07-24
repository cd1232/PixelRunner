using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsScreen : MonoBehaviour
{
	[SerializeField]
	private TMPro.TMP_Dropdown m_resolutions;

	[SerializeField]
	private TMPro.TMP_Dropdown m_fullScreenModes;

	List<Resolution> m_supportedResolutions = new List<Resolution>();
	Resolution m_currentResolution;


	Resolution m_selectedResolution;
	FullScreenMode m_selectedMode;
    // Start is called before the first frame update
    void Start()
    {
		m_resolutions.ClearOptions();
		m_fullScreenModes.ClearOptions();

		m_supportedResolutions.AddRange(Screen.resolutions);
		m_currentResolution = Screen.currentResolution;

		List<string> resNames = new List<string>();
		int index = 0;
		int currentResIndex = -1;
		foreach (Resolution resolution in m_supportedResolutions)
		{
			if (resolution.Equals(m_currentResolution))
			{
				currentResIndex = index;
			}

			resNames.Add(resolution.ToString());
			index++;
		}

		m_resolutions.AddOptions(resNames);

		m_resolutions.value = currentResIndex;


		//FullScreenMode.ExclusiveFullScreen 0
		//FullScreenMode.FullScreenWindow 1
		//FullScreenMode.MaximizedWindow 2
		//FullScreenMode.Windowed 3
		List<string> fullScreenModeNames = new List<string>();
		fullScreenModeNames.Add("Fullscreen");
		fullScreenModeNames.Add("Windowed Fullscreen");
		fullScreenModeNames.Add("Maximized Window");
		fullScreenModeNames.Add("Windowed");

		m_fullScreenModes.AddOptions(fullScreenModeNames);

		m_fullScreenModes.value = (int)Screen.fullScreenMode;


		m_resolutions.onValueChanged.AddListener(OnResolutionChanged);
		m_fullScreenModes.onValueChanged.AddListener(OnFullscreenModeChanged);
	}

	void OnResolutionChanged(int index)
	{
		m_currentResolution = m_supportedResolutions[index];
		Screen.SetResolution(m_supportedResolutions[index].width, m_supportedResolutions[index].height, Screen.fullScreenMode, m_supportedResolutions[index].refreshRate);
	}

	void OnFullscreenModeChanged(int index)
	{
		Screen.SetResolution(m_currentResolution.width, m_currentResolution.height, (FullScreenMode) index, m_supportedResolutions[index].refreshRate);
	}

}
