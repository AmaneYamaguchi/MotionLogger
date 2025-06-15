using System.Collections.Generic;
using UnityEngine;

namespace MotionLogger
{
    public abstract class Data
    {
        /// <summary>
        /// 参加者ID
        /// </summary>
        public int Id { private set; get; }
        /// <summary>
        /// 実験条件
        /// enumで管理するとなお良い
        /// </summary>
        public int Condition { private set; get; }
        /// <summary>
        /// 値を全て取得する
        /// </summary>
        public abstract List<object> Values { get; }
        /// <summary>
        /// CSVファイルの1行目
        /// </summary>
        public abstract string Header { get; }

        /// <summary>
        /// コンストラクタ
        /// 適宜overrideしてValuesを追加する
        /// </summary>
        /// <param name="id"></param>
        /// <param name="condition"></param>
        public Data(int id, int condition)
        {
            Id = id;
            Condition = condition;
        }
        /// <summary>
        /// string型へのキャスト
        /// CSVの2行目以降を構成するように各値をコンマで繋ぐ
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = $"{Id},{Condition}";
            for (int i = 0; i < Values.Count; i++)
            {
                result += $",{Values[i].ToString()}";
            }
            return result;
        }
    }
}