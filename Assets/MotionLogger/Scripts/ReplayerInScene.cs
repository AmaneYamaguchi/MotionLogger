using UnityEngine;

namespace MotionLogger
{
    public class ReplayerInScene : MonoBehaviour
    {
        public int Index
        {
            get => m_index;
            set
            {
                if (value < 0)
                {
                    m_index = MotionDataLogger.Instance?.Count - 1 ?? 0;
                }
                else if (value >= MotionDataLogger.Instance?.Count)
                {
                    m_index = 0;
                }
                else
                {
                    m_index = value;
                }
            }
        }

        [Header("çƒê∂Ç∑ÇÈÉfÅ[É^ÇÃê›íË")]
        [SerializeField] string m_fileName = "MotionData.csv";

        [Header("çƒê∂ê›íË")]
        [SerializeField] LoggedTransform[] m_loggedTransforms;

        [SerializeField] bool m_isReplaying = false;
        [SerializeField] int m_index = 0;
        [SerializeField] bool m_updateFixedTime = true;

        [ContextMenu("Start Replaying")]
        public void StartReplaying()
        {
            MotionDataLogger.Instance?.Import(m_fileName, hasHeader: true);
            m_isReplaying = true;
        }
        [ContextMenu("Stop Replaying")]
        public void StopReplaying()
        {
            m_isReplaying = false;
        }
        private void FixedUpdate()
        {
            if (!m_isReplaying) return;

            if (m_updateFixedTime)
            {
                Index++;
            }

            for (int i = 0; i < m_loggedTransforms.Length; i++)
            {
                var motionData = MotionDataLogger.Instance?.GetSerializedTransform(m_index, m_loggedTransforms[i].Label);
                if (motionData == null)
                {
                    Debug.LogWarning($"ReplayerInScene.FixedUpdate: No motion data found for index {m_index} and label '{m_loggedTransforms[i].Label}'.");
                    continue;
                }
                m_loggedTransforms[i].Deserialize(motionData);
            }
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(ReplayerInScene))]
    public class ReplayerInSceneEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var replayer = (ReplayerInScene)target;

            if (GUILayout.Button("Start Replaying"))
            {
                replayer.StartReplaying();
            }
            if (GUILayout.Button("Stop Replaying"))
            {
                replayer.StopReplaying();
            }
        }
    }
#endif
}
