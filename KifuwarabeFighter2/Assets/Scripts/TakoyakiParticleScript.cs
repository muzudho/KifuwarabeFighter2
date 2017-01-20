/// The license of this file is unknown. Author: 2dgames_jp
/// 出典 http://qiita.com/2dgames_jp/items/11bb76167fb44bb5af5f
using UnityEngine;
using System.Collections;
using SceneMain;

/// パーティクル
public class TakoyakiParticleScript : TakoyakiTokenScript
{
    /// プレハブ
    static GameObject m_prefab_ = null;
    /// パーティクルの生成
    public static TakoyakiParticleScript Add(float x, float y)
    {
        // プレハブを取得
        m_prefab_ = GetPrefab(m_prefab_, SceneCommon.Prefab_TakoyakiParticle0);
        // プレハブからインスタンスを生成
        return CreateInstance2<TakoyakiParticleScript>(m_prefab_, x, y);
    }

    /// 開始。コルーチンで処理を行う
    IEnumerator Start()
    {
        // 移動方向と速さをランダムに決める
        float dir = Random.Range(0, 359);
        float spd = Random.Range(10.0f, 20.0f);
        SetVelocity(dir, spd);

        // 見えなくなるまで小さくする
        while (ScaleX > 0.01f)
        {
            // 0.01秒ゲームループに制御を返す
            yield return new WaitForSeconds(0.01f);
            // だんだん小さくする
            MulScale(0.9f);
            // だんだん減速する
            MulVelocity(0.9f);
        }

        // 消滅
        DestroyObj();
    }
}