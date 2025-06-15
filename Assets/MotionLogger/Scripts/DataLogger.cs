using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using UnityEngine;

namespace MotionLogger
{
    public abstract class DataLogger<T> : MonoBehaviour where T : Data
    {
        /// <summary>
        /// データをまとめたリスト
        /// </summary>
        protected List<T> DataList = new List<T>();
        /// <summary>
        /// ファイル名
        /// 重複が発生しないよう日付時刻で決める等する
        /// </summary>
        protected string FileName
        {
            get
            {
                var time = DateTime.Now;
                return $"{time.Year}_{time.Month}_{time.Day}_{time.Hour}_{time.Minute}_{time.Second}.csv";
            }
        }
        [Header("Assets以下のディレクトリを指定する")]
        public string DataPath = "ExperimentData";

        public void Add(T data)
        {
            DataList.Add(data);
            Debug.Log($"DataLogger<{typeof(T)}>.Add: Added a new data.");
        }
        /// <summary>
        /// CSVファイルを生成してデータを出力する
        /// </summary>
        /// <param name="hasHeader"></param>
        public void Export(string fileName, string directory, bool hasHeader = false)
        {
            // FileNameの名前でCSVファイルを生成する
            var file = new StreamWriter($"{Application.dataPath}/{directory}/{fileName}", false, Encoding.GetEncoding("Shift_JIS"));

            // 1行目：
            if (hasHeader)
            {
                file.WriteLine(DataList[0].Header);
            }

            // 2行目以降：
            foreach (Data data in DataList)
            {
                file.WriteLine(data.ToString());
            }

            // StreamWriterを破棄する
            file.Close();
            Debug.Log($"DataLogger<{typeof(T)}>.Export: Exported {fileName} to {directory}.");
        }
        /// <summary>
        /// CSVファイルを生成してデータを出力する
        /// </summary>
        /// <param name="hasHeader"></param>
        public void Export(string directory, bool hasHeader = false) => Export(FileName, directory, hasHeader);
        /// <summary>
        /// CSVファイルを生成してデータを出力する
        /// </summary>
        /// <param name="hasHeader"></param>
        public void Export(bool hasHeader = false) => Export(DataPath, hasHeader);
    }
}