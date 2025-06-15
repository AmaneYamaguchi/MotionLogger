using UnityEngine;
using System.Collections.Generic;

namespace MotionLogger
{
    public class MotionData : Data
    {
        public override List<object> Values
        {
            get => m_values;
        }

        public override string Header
        {
            get
            {
                var result = $"Id,Condition";
                for (int i = 0; i < m_transformCount; i++)
                {
                    result += $",{LoggedTransform.Header}"; // 各LoggedTransformのヘッダーを追加
                }
                return result;
            }
        }

        int m_transformCount;
        List<object> m_values;

        public MotionData(int id, int condition, params LoggedTransform[] values) : base(id, condition)
        {
            m_transformCount = values.Length;
            m_values = new(values.Length * LoggedTransform.ValueCount);
            foreach (var transform in values)
            {
                var serializedData = transform.Serialize();
                if (serializedData.Length != LoggedTransform.ValueCount)
                {
                    Debug.LogError($"MotionData: Expected {LoggedTransform.ValueCount} values, but got {values.Length}.");
                    continue;
                }
                m_values.AddRange(serializedData); // 位置、回転、スケールの値を追加
            }
        }
        public MotionData(int id, int condition, params string[] values) : base(id, condition)
        {
            m_transformCount = values.Length / LoggedTransform.ValueCount; // 1つのTransformは11個の値を持つ
            m_values = new List<object>(values); // 直接Listに変換

            if (m_values.Count != m_transformCount * LoggedTransform.ValueCount)
            {
                Debug.LogError($"MotionData: Expected {m_transformCount * LoggedTransform.ValueCount} values, but got {m_values.Count}.");
            }

            // valuesの内容をチェックして、10個ずつのTransformデータとして追加
            for (int i = 0; i < m_transformCount; i++)
            {
                int startIndex = i * LoggedTransform.ValueCount;
                if (startIndex + LoggedTransform.ValueCount - 1 < m_values.Count)
                {
                    m_values.Add(values[startIndex]); // ラベルを先頭に追加
                    m_values.Add(values[startIndex + 1]);
                    m_values.Add(values[startIndex + 2]);
                    m_values.Add(values[startIndex + 3]);
                    m_values.Add(values[startIndex + 4]);
                    m_values.Add(values[startIndex + 5]);
                    m_values.Add(values[startIndex + 6]);
                    m_values.Add(values[startIndex + 7]);
                    m_values.Add(values[startIndex + 8]);
                    m_values.Add(values[startIndex + 9]);
                    m_values.Add(values[startIndex + 10]);
                }
                else
                {
                    Debug.LogError($"MotionData: Not enough values for Transform at index {i}.");
                }
            }
        }

        public string GetSerializedTransform(string label)
        {
            int labelIndex = m_values.FindIndex(v => v is string s && s == label);
            if (labelIndex < 0)
            {
                Debug.LogError($"MotionData.GetTransform: No transform found with label '{label}'.");
                return "";
            }
            string result = $""; // ラベルを先頭に追加
            for (int i=labelIndex; i<labelIndex + LoggedTransform.ValueCount; i++)
            {
                if (m_values[i] is string value)
                {
                    result += $"{value},";
                }
                else
                {
                    Debug.LogError($"MotionData.GetTransform: Value at index {i} is not a string.");
                    return "";
                }
            }
            return result.TrimEnd(','); // 最後のカンマを削除して返す
        }
    }
}
