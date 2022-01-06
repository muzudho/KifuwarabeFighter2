namespace SceneSelect
{
    using System.Collections.Generic;
    using Assets.Scripts;
    using Assets.Scripts.Models.Input;
    using Assets.Scripts.Models.Scene.Select;
    using DojinCircleGrayscale.StellaQL;
    using DojinCircleGrayscale.StellaQL.Acons.Select_Cursor;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class CameraBehaviour : MonoBehaviour
    {
        Dictionary<Player, Animator> animators;

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
            animators = new Dictionary<Player, Animator>
            {
                { Player.N1, GameObject.Find(GameObjectPaths.All[GameObjectKeys.P1Player]).GetComponent<Animator>() },
                { Player.N2, GameObject.Find(GameObjectPaths.All[GameObjectKeys.P2Player]).GetComponent<Animator>() },
            };

            // このシーンのデータベースを用意するぜ☆（＾▽＾）
            //StateExTable.Instance.InsertAllStates();
        }

        // Update is called once per frame
        void Update()
        {
            ReadyingTime++;

            // 現在のアニメーター・ステートに紐づいたデータ
            AcStateRecordable astateRecord0 = AControl.Instance.GetCurrentAcStateRecord(animators[Player.N1]);
            AcStateRecordable astateRecord1 = AControl.Instance.GetCurrentAcStateRecord(animators[Player.N2]);
            if (
                AControl.Instance.StateHash_to_record[Animator.StringToHash(Select_Cursor_AbstractAControl.BASELAYER_READY)].Name == astateRecord0.Name
                &&
                AControl.Instance.StateHash_to_record[Animator.StringToHash(Select_Cursor_AbstractAControl.BASELAYER_READY)].Name == astateRecord1.Name
                )
            {
                // １プレイヤー、２プレイヤー　ともに Ready ステートなら。
                animators[Player.N1].SetTrigger(ThisSceneStatus.TriggerTimeOver);
                animators[Player.N2].SetTrigger(ThisSceneStatus.TriggerTimeOver);
                ThisSceneStatus.TransitionTime = 1;
            }

            if (0 < ThisSceneStatus.TransitionTime)
            {
                ThisSceneStatus.TransitionTime++;

                if (5 == ThisSceneStatus.TransitionTime)
                {
                    SceneManager.LoadScene(AppConstants.sceneToName[(int)KeyOfScene.Fight]);
                }
            }
        }
    }
}
