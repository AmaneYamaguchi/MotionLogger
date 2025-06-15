# MotionLogger
VRなどで人間のユーザの動きをある程度のクォリティで保存・再生するためのプログラムです。

- VRのCameraRigなど特定のオブジェクトの `Transform.position` と `Transform.rotation` をcsvに保存する機能
- その形式のcsvを読み込んで元のオブジェクトに戻し、再生する機能

があります。

# Getting Started
## Objects in Sample Scene
サンプルシーンには以下のゲームオブジェクトがあります。

- Motion DataLogger
- Logger
- Replayer
- Camera Rig
  - Cube
  - Sphere

このうち、Motion DataLoggerはデータをcsvに読み書きするクラスです。
LoggerとReplayerがMotion DataLoggerに対してデータの出し入れをすることで、シーン内のオブジェクトの動きを保存・再生しています。

Camera RigはTransformならなんでもいいので、ここでは本当に適当なオブジェクトを入れています。

## Export CSV
1. 動きを保存したいオブジェクトに `LoggedTransaform` をアタッチする
2. `LoggedTransform` のインスペクタからラベルを適当に設定する
    - 重複が無いこと
    - カンマが含まれていないこと

※サンプルシーンではここまで完了しています。
以下はサンプルシーンを参照してください。
新たなシーンに用意する場合は、それぞれの項目で言及しているオブジェクトをシーンに配置した上で、各種コンポーネントを追加してください。

3. Motion DataLoggerオブジェクトにアタッチされている同名のスクリプト `MotionDataLogger` で保存先のディレクトリを指定する（無ければ予め作成する）
4. Loggerオブジェクトにアタッチされているスクリプト `LoggerInScene` のインスペクタから1. で用意した `LoggedTransform` を全て格納する
5. 実験等で使うなら、 `LoggerInScene` の `m_id` と `m_condition` を適当に設定する

※以下、プレイモード

6. `LoggerInScene` のインスペクタから「Start Logging」ボタンを押下して保存を開始する
7. `LoggerInScene` のインスペクタから「Stop Logging」ボタンを押下して保存を終了する
8. 3.で設定したディレクトリに日付と時刻の名前のcsvが生成されていることを確認する

## Import CSV
3. までは Export CSV と同じ。
4. Replayerオブジェクトにアタッチされているスクリプト `ReplayerInScene` のインスペクタから1. で用意した `LoggerTransform` を全て格納する

※以下、プレイモード

5. `ReplayerInScene` のインスペクタから「Start Replaying」ボタンを押下して再生を開始する
6. `ReplayerInScene` のインスペクタから「StopReplaying」ボタンを押下して再生を終了する

なお、再生中に `ReplayerInScene` のインスペクタから `m_updateFixedTime` を `false` にすると自動再生が止まります。
この状態でその上の `m_index` の数値を変更すると、そのフレームに直飛びできます。
