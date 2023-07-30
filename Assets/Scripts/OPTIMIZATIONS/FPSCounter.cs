using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Utility/FPS counter")]
public class FPSCounter : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private Text m_Text;
	[SerializeField] private Text min_Text;
	[SerializeField] private Text max_Text;

	private const float fpsMeasurePeriod = 0.5f;
	private int m_FpsAccumulator;
	private float m_FpsNextPeriod;
	private int m_CurrentFps;
	private int minFPS = -1;
	private int maxFPS = -1;
	
	private void Start()
	{
		m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
	}

	private void Update()
	{
		m_FpsAccumulator++;
		if (Time.realtimeSinceStartup > m_FpsNextPeriod)
		{
			m_CurrentFps = (int)(m_FpsAccumulator / fpsMeasurePeriod);

			if (Time.realtimeSinceStartup > 20)
			{
				if (minFPS == -1)
				{
					minFPS = m_CurrentFps;
					maxFPS = m_CurrentFps;
				}
				else
				{
					if (minFPS > m_CurrentFps)
						minFPS = m_CurrentFps;
					if (maxFPS < m_CurrentFps)
						maxFPS = m_CurrentFps;
				}
			}

			m_FpsAccumulator = 0;
			m_FpsNextPeriod += fpsMeasurePeriod;

			if (m_Text != null)
			{
				m_Text.text = $"FPS:{m_CurrentFps}";
				min_Text.text = $"minFPS:{minFPS}";
				max_Text.text = $"maxFPS:{maxFPS}";
			}
		}
	}
}
