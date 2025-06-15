using UnityEngine;

namespace MotionLogger
{
    /// <summary>
    /// <see cref="MotionDataLogger"/>を使ってデータ保存を行うコンポーネント。
    /// </summary>
    public class LoggerInScene : MonoBehaviour
    {
        [SerializeField] LoggedTransform[] m_loggedTransforms;
        [SerializeField] int m_id;
        [SerializeField] int m_condition;
        [SerializeField] bool m_isLogging = false;

        [ContextMenu("Start Logging")]
        public void StartLogging()
        {
            m_isLogging = true;
            Debug.Log($"LoggingManagerInScene.StartLogging: Logging started with ID {m_id} and condition {m_condition}.");
            MotionDataLogger.Instance?.ResetData();
        }
        [ContextMenu("Stop Logging")]
        public void StopLogging()
        {
            m_isLogging = false;
            Debug.Log($"LoggingManagerInScene.StopLogging: Logging stopped for ID {m_id} and condition {m_condition}.");
            MotionDataLogger.Instance?.Export(hasHeader: true);
        }

        private void Awake()
        {
            m_isLogging = false;
        }
        private void FixedUpdate()
        {
            if (!m_isLogging) return;

            MotionDataLogger.Instance?.Add(new MotionData(m_id, m_condition, m_loggedTransforms));
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(LoggerInScene))]
    public class LoggingManagerInSceneEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var loggingManager = (LoggerInScene)target;

            if (GUILayout.Button("Start Logging"))
            {
                loggingManager.StartLogging();
            }
            if (GUILayout.Button("Stop Logging"))
            {
                loggingManager.StopLogging();
            }
        }
    }
#endif
}
