﻿using UnityEngine;

namespace MotionLogger
{
    public class LoggedTransform : MonoBehaviour
    {
        public string Label => m_label;

        [Tooltip("データのラベル。カンマは使わないこと。")]
        [SerializeField] string m_label = "TransformData"; // データのラベル

        public static string Header
        {
            get
            {
                return "Label," + // ラベルを先頭に追加
                       "PositionX,PositionY,PositionZ," + // 位置
                       "RotationX,RotationY,RotationZ,RotationW," + // 回転
                       "ScaleX,ScaleY,ScaleZ"; // スケール
            }
        }
        public static int ValueCount => 11; // ラベルに加え、位置、回転、スケールの値がそれぞれ3つ、4つ、3つで合計11個
        public string[] Serialize()
        {
            return new string[]
            {
                m_label,
                transform.position.x.ToString(),
                transform.position.y.ToString(),
                transform.position.z.ToString(),
                transform.rotation.x.ToString(),
                transform.rotation.y.ToString(),
                transform.rotation.z.ToString(),
                transform.rotation.w.ToString(),
                transform.localScale.x.ToString(),
                transform.localScale.y.ToString(),
                transform.localScale.z.ToString()
            };
        }
        public void Deserialize(string data)
        {
            var values = data.Split(',');
            if (values.Length != ValueCount)
            {
                Debug.LogError($"Invalid data format for LoggedTransform deserialization. Count={values.Length}");
                return;
            }
            Deserialize(values[0], values[1], values[2], values[3],
                        values[4], values[5], values[6], values[7],
                        values[8], values[9], values[10]);
        }
        public void Deserialize(string label, string positionX, string positionY, string positionZ,
                                string rotationX, string rotationY, string rotationZ, string rotationW,
                                string scaleX, string scaleY, string scaleZ)
        {
            if (label != m_label)
            {
                Debug.LogError($"Label mismatch: expected '{m_label}', got '{label}'.");
                return;
            }
            transform.position = new Vector3(float.Parse(positionX), float.Parse(positionY), float.Parse(positionZ));
            transform.rotation = new Quaternion(float.Parse(rotationX), float.Parse(rotationY), float.Parse(rotationZ), float.Parse(rotationW));
            transform.localScale = new Vector3(float.Parse(scaleX), float.Parse(scaleY), float.Parse(scaleZ));
        }
    }
}
