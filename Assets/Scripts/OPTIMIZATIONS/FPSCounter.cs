using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{
    public class FPSCounter : MonoBehaviour
    {
		public Text m_Text;
		public Text min_Text;
		public Text max_Text;

        const float fpsMeasurePeriod = 0.5f;
        private int m_FpsAccumulator = 0;
        private float m_FpsNextPeriod = 0;
        private int m_CurrentFps;
		private int minFPS = -1;
		private int maxFPS = -1;

        private void Start()
        {
            m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
        }


        private void Update()
        {
            // measure average frames per second
            m_FpsAccumulator++;
            if (Time.realtimeSinceStartup > m_FpsNextPeriod)
            {
                m_CurrentFps = (int) (m_FpsAccumulator/fpsMeasurePeriod);

				if (Time.realtimeSinceStartup > 20) {
					if (minFPS == -1) {
						minFPS = m_CurrentFps;
						maxFPS = m_CurrentFps;
					} else {
						if (minFPS > m_CurrentFps)
							minFPS = m_CurrentFps;
						if (maxFPS < m_CurrentFps)
							maxFPS = m_CurrentFps;
					}
				}

                m_FpsAccumulator = 0;
                m_FpsNextPeriod += fpsMeasurePeriod;

				if (m_Text != null) {
					m_Text.text = string.Format ("FPS:{0}", m_CurrentFps);
					min_Text.text = string.Format ("minFPS:{0}", minFPS);
					max_Text.text = string.Format ("maxFPS:{0}", maxFPS);
				}
            }
        }
    }
}
