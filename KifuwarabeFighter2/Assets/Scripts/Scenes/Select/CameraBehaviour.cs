namespace SceneSelect
{
    using System.Collections.Generic;
    using Assets.Scripts.Model.Dto.Input;
    using Assets.Scripts.Model.Dto.Select;
    using DojinCircleGrayscale.StellaQL;
    using DojinCircleGrayscale.StellaQL.Acons.Select_Cursor;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class CameraBehaviour : MonoBehaviour
    {
        Dictionary<PlayerIndex, Animator> animators;

        /// <summary>
        /// computer character selecting minimum time; コンピューターがキャラクターセレクトしている最低時間。
        /// </summary>
        public const int READY_TIME_LENGTH = 180;
        /// <summary>
        /// computer character selecting minimum time counter; コンピューターがキャラクターセレクトしている最低時間カウンター。
        /// </summary>
        int readyingTime; public int ReadyingTime { get { return readyingTime; } set { readyingTime = value; } }

        void Start()
        {
            animators = new Dictionary<PlayerIndex, Animator>
            {
                { PlayerIndex.Player1, GameObject.Find(GameObjectPaths.All[GameObjectIndexes.P1Player]).GetComponent<Animator>() },
                { PlayerIndex.Player2, GameObject.Find(GameObjectPaths.All[GameObjectIndexes.P2Player]).GetComponent<Animator>() },
            };

            // このシーンのデータベースを用意するぜ☆（＾▽＾）
            //StateExTable.Instance.InsertAllStates();
        }

        // Update is called once per frame
        void Update()
        {
            ReadyingTime++;

            // 現在のアニメーター・ステートに紐づいたデータ
            AcStateRecordable astateRecord0 = AControl.Instance.GetCurrentAcStateRecord(animators[PlayerIndex.Player1]);
            AcStateRecordable astateRecord1 = AControl.Instance.GetCurrentAcStateRecord(animators[PlayerIndex.Player2]);
            if (
                AControl.Instance.StateHash_to_record[Animator.StringToHash(Select_Cursor_AbstractAControl.BASELAYER_READY)].Name == astateRecord0.Name
                &&
                AControl.Instance.StateHash_to_record[Animator.StringToHash(Select_Cursor_AbstractAControl.BASELAYER_READY)].Name == astateRecord1.Name
                )
            {
                // １プレイヤー、２プレイヤー　ともに Ready ステートなら。
                animators[PlayerIndex.Player1].SetTrigger(ThisSceneDto.TriggerTimeOver);
                animators[PlayerIndex.Player2].SetTrigger(ThisSceneDto.TriggerTimeOver);
                ThisSceneDto.TransitionTime = 1;
            }

            if (0 < ThisSceneDto.TransitionTime)
            {
                ThisSceneDto.TransitionTime++;

                if (5 == ThisSceneDto.TransitionTime)
                {
                    SceneManager.LoadScene(CommonScript.Scene_to_name[(int)SceneIndex.Fight]);
                }
            }
        }
    }
}
