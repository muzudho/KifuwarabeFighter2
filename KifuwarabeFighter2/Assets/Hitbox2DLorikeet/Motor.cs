using Assets.Scripts.Model.Dto.Input;
using DojinCircleGrayscale.StellaQL;
using System.Collections.Generic;
using UnityEngine;

namespace DojinCircleGrayscale.Hitbox2DLorikeet
{
    public enum HitboxIndex
    {
        Hitbox,
        Weakbox,
        Strongbox,
        Num
    }

    public interface Motorable
    {
        /// <summary>
        /// [CliptypeIndex]
        /// </summary>
        Dictionary<int, FramedMotionable> MotionHash_to_instance { get; }
    }

    public abstract class AbstractMotor : Motorable
    {
        public AbstractMotor()
        {
            MotionHash_to_instance = new Dictionary<int, FramedMotionable>();
        }

        /// <summary>
        /// [CliptypeIndex]
        /// </summary>
        public Dictionary<int, FramedMotionable> MotionHash_to_instance { get; private set; }

        public void AddMappings_MotionAssetPath_to_Instance(Dictionary<string, FramedMotionable> mappings)
        {
            foreach (KeyValuePair<string, FramedMotionable> pair in mappings)
            {
                MotionHash_to_instance.Add(Animator.StringToHash(pair.Key), pair.Value);
            }
        }

        /// <summary>
        /// 現在のアニメーション・クリップに対応したユーザー定義レコードを取得。
        /// Current.
        /// </summary>
        /// <returns></returns>
        public FramedMotionable GetCurrentUserDefinedCliptypeRecord(Animator animator, AControllable aControl)
        {
            AnimatorStateInfo animeStateInfo = animator.GetCurrentAnimatorStateInfo(0);

            int motionAssetPathHash = (aControl.StateHash_to_record[animeStateInfo.fullPathHash]).MotionAssetPathHash;

            if (MotionHash_to_instance.ContainsKey(motionAssetPathHash))
            {
                return MotionHash_to_instance[motionAssetPathHash];
            }

            throw new UnityException("Not found record. motionAssetPathHash = [" + motionAssetPathHash + "]");
        }

        public static float GetOffsetX(HitboxIndex hitbox, int serialImage, int slice)
        {
            switch (hitbox)
            {
                case HitboxIndex.Hitbox: return HitboxData.Instance.imageAndSlice_To_OffsetX[serialImage, slice];
                case HitboxIndex.Weakbox: return WeakboxData.Instance.imageAndSlice_To_OffsetX[serialImage, slice];
                case HitboxIndex.Strongbox: return StrongboxData.Instance.imageAndSlice_To_OffsetX[serialImage, slice];
                default: throw new UnityException("未定義のヒットボックス☆");
            }
        }
        public static float GetOffsetY(HitboxIndex hitbox, int serialImage, int slice)
        {
            switch (hitbox)
            {
                case HitboxIndex.Hitbox: return HitboxData.Instance.imageAndSlice_To_OffsetY[serialImage, slice];
                case HitboxIndex.Weakbox: return WeakboxData.Instance.imageAndSlice_To_OffsetY[serialImage, slice];
                case HitboxIndex.Strongbox: return StrongboxData.Instance.imageAndSlice_To_OffsetY[serialImage, slice];
                default: throw new UnityException("未定義のヒットボックス☆");
            }
        }
        public static float GetScaleX(HitboxIndex hitbox, int serialImage, int slice)
        {
            switch (hitbox)
            {
                case HitboxIndex.Hitbox: return HitboxData.Instance.imageAndSlice_To_ScaleX[serialImage, slice];
                case HitboxIndex.Weakbox: return WeakboxData.Instance.imageAndSlice_To_ScaleX[serialImage, slice];
                case HitboxIndex.Strongbox: return StrongboxData.Instance.imageAndSlice_To_ScaleX[serialImage, slice];
                default: throw new UnityException("未定義のヒットボックス☆");
            }
        }
        public static float GetScaleY(HitboxIndex hitbox, int serialImage, int slice)
        {
            switch (hitbox)
            {
                case HitboxIndex.Hitbox: return HitboxData.Instance.imageAndSlice_To_ScaleY[serialImage, slice];
                case HitboxIndex.Weakbox: return WeakboxData.Instance.imageAndSlice_To_ScaleY[serialImage, slice];
                case HitboxIndex.Strongbox: return StrongboxData.Instance.imageAndSlice_To_ScaleY[serialImage, slice];
                default: throw new UnityException("未定義のヒットボックス☆");
            }
        }
        /// <summary>
        /// キャラクターと、モーション、現在のフレームを指定することで、通し画像番号とスライス番号を返す。
        /// これにより Hitbox2DScript と連携を取ることができる。
        /// </summary>
        /// <param name="serialTilesetfileIndex"></param>
        /// <param name="slice"></param>
        /// <param name="character"></param>
        /// <param name="motion"></param>
        /// <param name="currentMotionFrame"></param>
        public static int GetSerialTilesetfileIndex(CharacterIndex character, FramedMotionable cliptypeExRecord)
        {
            // キャラクターと画像種類番号から、通し画像番号を取得。
            return (int)character * (int)TilesetfileType.Num + cliptypeExRecord.TilesetfileType;
        }
        /// <summary>
        /// キャラクターと、モーション、現在のフレームを指定することで、通し画像番号とスライス番号を返す。
        /// これにより Hitbox2DScript と連携を取ることができる。
        /// </summary>
        /// <param name="serialTilesetfileIndex"></param>
        /// <param name="slice"></param>
        /// <param name="character"></param>
        /// <param name="motion"></param>
        /// <param name="currentMotionFrame"></param>
        public static int GetSlice(FramedMotionable cliptypeExRecord, int currentMotionFrame)
        {
            return cliptypeExRecord.Slices[currentMotionFrame];
        }

        /// <summary>
        /// 当たり判定くん☆
        /// 
        /// Upadate( ) の最後に呼び出してください。
        /// </summary>
        /// <param name="player"></param>
        public void Update(Animator animator, AControllable aControl, PlayerKey player, Transform transform, SpriteRenderer[] hitboxsSpriteRenderer, BoxCollider2D weakboxCollider2D)
        {
            if (animator.GetCurrentAnimatorClipInfo(0).Length < 1)
            {
                Debug.LogError("クリップインフォの配列の範囲外エラー☆ player = " + player);
                return;
            }

            // クリップ取得
            AnimationClip clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;

            // FIXME: bug? クリップ名は、Animator Controller Override を使っている場合、継承しているアニメーション・クリップは名前を取れない？
            // string clipName = clip.name;

            // ステートのスピード・プロパティを取得。
            AnimatorStateInfo animeStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float stateSpeed = animeStateInfo.speed;

            FramedMotionable cliptypeExRecord = GetCurrentUserDefinedCliptypeRecord(animator, aControl);
//            Motionable cliptypeExRecord = Motor.Instance.GetCurrentUserDefinedCliptypeRecord(animator, aControl);

            // 正規化時間取得（0～1 の数倍。時間経過で 1以上になる）
            float normalizedTime = animeStateInfo.normalizedTime;
            // ループするモーションでなければ、少しの誤差を除いて、1.0 より大きくはならないはず。

            // Samples、Frame rate は、キー・フレームの数と同じにしている前提。
            // クリップ・レングスは１になる。
            // 全てのモーションは１秒として作っておき、Speed を利用して　表示フレーム数 を調整するものとする。

            // Speed の使い方。
            // 60 / モーション画像枚数 / 表示したいフレーム数
            //
            // 例：　弱パンチは画像２枚として、5フレーム表示したい場合。
            // 60 / 2 / 5 = 6
            //
            // 例：　中パンチは画像３枚として、7フレーム表示したい場合。
            // 60 / 3 / 7 = 約 2.8571
            //
            // 例：　強パンチは画像５枚として、9フレーム表示したい場合。
            // 60 / 5 / 9 = 約 1.3333
            //
            // 例：　投了は画像４枚として、１２０フレーム表示したい場合。
            // 60 / 4 / 120 = 約 0.125

            int currentMotionFrame = Mathf.FloorToInt((normalizedTime % 1.0f) * clip.frameRate);

            // 画像分類　スライス番号　取得
            int serialTilesetfile = GetSerialTilesetfileIndex(
                CommonScript.UseCharacters[player], // キャラクター番号
                cliptypeExRecord
                );
            int slice = GetSlice(
                cliptypeExRecord,
                currentMotionFrame
                );
            //if((int)PlayerSerialId.Player1==iPlayer && MotionDatabaseScript.AclipTypeIndex.Num != aclipType)
            //{
            //    //ebug.Log( " iPlayer = " + iPlayer + " character = " + character + " aclipType = "+ aclipType + " currentMotionFrame = " + currentMotionFrame + " / serialImage = " + serialImage + " slice = " + slice);
            //    // + " motion = " + motion
            //    // "anime.GetCurrentAnimatorClipInfo(0).Length = " + anime.GetCurrentAnimatorClipInfo(0).Length+
            //}

            if (-1 != slice)
            {
                float offsetX;
                float offsetY;
                float scaleX;
                float scaleY;
                for (int iHitbox = 0; iHitbox < (int)HitboxIndex.Num; iHitbox++) // １つのスライスには最大で　赤、青、黄の３つの箱がある前提。
                {
                    // ゲームオブジェクトの位置（スプライト・レンダラー）更新
                    offsetX = transform.position.x + Mathf.Sign(transform.localScale.x) * Common.SCALE * GetOffsetX((HitboxIndex)iHitbox, serialTilesetfile, slice);
                    offsetY = transform.position.y + Common.SCALE * GetOffsetY((HitboxIndex)iHitbox, serialTilesetfile, slice);
                    scaleX = Common.SCALE * GetScaleX((HitboxIndex)iHitbox, serialTilesetfile, slice);
                    scaleY = Common.SCALE * GetScaleY((HitboxIndex)iHitbox, serialTilesetfile, slice);
                    hitboxsSpriteRenderer[iHitbox].transform.position = new Vector3(offsetX, offsetY);
                    hitboxsSpriteRenderer[iHitbox].transform.localScale = new Vector3(scaleX, scaleY);

                    if ((int)HitboxIndex.Weakbox == iHitbox)
                    {
                        // ウィークボックスの場合、衝突判定（コライダー）も変更
                        weakboxCollider2D.transform.position = new Vector3(offsetX, offsetY);
                        weakboxCollider2D.transform.localScale = new Vector3(scaleX, scaleY);
                    }

                    //if ((int)PlayerSerialId.Player1 == iPlayer)
                    //{
                    //ebug.Log("stateSpeed = " + stateSpeed + " clip.frameRate = " + clip.frameRate + " normalizedTime = " + normalizedTime + " currentMotionFrame = " + currentMotionFrame + " 当たり判定くん.position.x = " + player_to_charAttackImgSpriteRenderer[iPlayer].transform.position.x + " 当たり判定くん.position.y = " + player_to_charAttackImgSpriteRenderer[iPlayer].transform.position.y + " scale.x = " + player_to_charAttackImgSpriteRenderer[iPlayer].transform.localScale.x + " scale.y = " + player_to_charAttackImgSpriteRenderer[iPlayer].transform.localScale.y);
                    //    //" clip.length = " + clip.length +
                    //    //" motionFrames = " + motionFrames +
                    //    //" lastKeyframeTime = "+ lastKeyframeTime +
                    //    //" clip.length = "+ clip.length +
                    //    //" motionFrames = "+ motionFrames +
                    //}
                }
            }
        }
    }
}
