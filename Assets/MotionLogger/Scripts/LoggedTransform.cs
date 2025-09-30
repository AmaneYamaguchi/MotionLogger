using UnityEngine;

namespace MotionLogger
{
    public class LoggedTransform : MonoBehaviour
    {
        public string Label => m_label;

        [Tooltip("データのラベル。カンマは使わないこと。")]
        [SerializeField] string m_label = "TransformData"; // データのラベル
        [SerializeField] bool m_useLocalPosition = true;
        [SerializeField] bool m_useLocalRotation = true;
        [Header("デシリアライズ設定")]
        [SerializeField] bool m_deserializePosition = true;
        [SerializeField] bool m_deserializeRotation = true;
        [SerializeField] bool m_deserializeScale = false;

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
            var pos = m_useLocalPosition ? transform.localPosition : transform.position;
            var rot = m_useLocalRotation ? transform.localRotation : transform.rotation;
            return new string[]
            {
                m_label,
                pos.x.ToString(),
                pos.y.ToString(),
                pos.z.ToString(),
                rot.x.ToString(),
                rot.y.ToString(),
                rot.z.ToString(),
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
            if (m_deserializePosition)
            {
                var pos = new Vector3(float.Parse(positionX), float.Parse(positionY), float.Parse(positionZ));
                if (m_useLocalPosition)
                    transform.localPosition = pos;
                else
                    transform.position = pos;
            }
            if (m_deserializeRotation)
            {
                var rot = new Quaternion(float.Parse(rotationX), float.Parse(rotationY), float.Parse(rotationZ), float.Parse(rotationW));
                if (m_useLocalRotation)
                    transform.localRotation = rot;
                else
                    transform.rotation = rot;
            }
            if (m_deserializeScale)
                transform.localScale = new Vector3(float.Parse(scaleX), float.Parse(scaleY), float.Parse(scaleZ));
        }
    }
}
