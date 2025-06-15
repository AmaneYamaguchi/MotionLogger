using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace MotionLogger
{
    public class MotionDataLogger : DataLogger<MotionData>
    {
        /// <summary>
        /// MotionDataLoggerのインスタンス。
        /// シーン上に1つだけ存在することを保証するためのシングルトンパターンを使用する。
        /// </summary>
        public static MotionDataLogger Instance { get; private set; }
        bool m_isImported = false;

        public int Count => DataList.Count;
        public string GetSerializedTransform(int index, string label)
        {
            if (!m_isImported)
            {
                Debug.LogError("MotionDataLogger.GetSerializedTransform: Data has not been imported yet.");
                return null;
            }

            if (index < 0 || index >= DataList.Count)
            {
                Debug.LogError($"MotionDataLogger.GetSerializedTransform: Index {index} is out of range.");
                return null;
            }

            var motionData = DataList[index];
            if (motionData == null)
            {
                Debug.LogError($"MotionDataLogger.GetSerializedTransform: MotionData at index {index} is null.");
                return null;
            }
            var serializedData = motionData.GetSerializedTransform(label);
            if (serializedData == "")
            {
                Debug.LogWarning($"MotionDataLogger.GetSerializedTransform: No transform found with label '{label}' in MotionData at index {index}.");
                return null;
            }
            return serializedData;
        }

        /// <summary>
        /// 指定されたパスからMotionDataをインポートする。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public void Import(string fileName, string directory, bool hasHeader=true)
        {
            var file = new StreamReader($"{Application.dataPath}/{directory}/{fileName}");

            // 1行目はヘッダーとして読み飛ばす
            if (hasHeader)
            {
                file.ReadLine();
            }
            var dataList = new List<MotionData>();
            while (!file.EndOfStream)
            {
                var line = file.ReadLine();
                if (string.IsNullOrEmpty(line)) continue; // 空行はスキップ

                var values = line.Split(',');
                if (values.Length < 2) continue; // データが不完全な行はスキップ

                // 参加者IDと条件を取得
                int id = int.Parse(values[0]);
                int condition = int.Parse(values[1]);

                // valuesからidとconditionを削除する
                var transformValues = values[2..];
                var motionData = new MotionData(id, condition, transformValues);

                dataList.Add(motionData);
            }
            file.Close();
            Debug.Log($"MotionDataLogger.Import: Imported {dataList.Count} x {2 + dataList[0].Values.Count} MotionData entries from {fileName} in {directory}.");
            m_isImported = true;
            //return dataList;

            DataList = dataList;
        }
        public void Import(string fileName, bool hasHeader = true) => Import(fileName, DataPath, hasHeader);

        public void ResetData()
        {
            DataList.Clear();
        }

        private void Awake()
        {
            // Singletonパターンの実装
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("MotionDataLogger instance already exists. Destroying the new instance.");
                Destroy(gameObject);
            }

            m_isImported = false;
            m_windowId = Random.Range(0, 100000000);
        }


        int m_windowId = 0;
        Rect m_windowRect = new Rect(20, 20, 300, 200);
        [SerializeField] bool m_drawWindow = false;
        Vector2 m_scrollVector = Vector2.zero;
        private void OnGUI()
        {
            if (!m_drawWindow) return;

            m_windowRect = GUILayout.Window(m_windowId, m_windowRect, DrawWindow, "Motion Data Logger");
        }
        private void DrawWindow(int windowId)
        {
            m_scrollVector = GUILayout.BeginScrollView(m_scrollVector, GUILayout.Width(300), GUILayout.Height(200));
            foreach (var data in DataList)
            {
                GUILayout.Label(data.ToString());
            }
            GUILayout.EndScrollView();

            GUI.DragWindow();
        }
    }
}
