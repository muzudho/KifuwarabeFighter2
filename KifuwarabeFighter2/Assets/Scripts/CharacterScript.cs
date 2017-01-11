using UnityEngine;

public class CharacterScript : MonoBehaviour {

    public int playerNumber;
    public GameObject bullet;

    void Update()
    {
        // 弾を撃つぜ☆
        if (
            (playerNumber==1&&(
                Input.GetButton(CommonScript.BUTTON_03_P1_LP) ||
                Input.GetButton(CommonScript.BUTTON_04_P1_MP) ||
                Input.GetButton(CommonScript.BUTTON_05_P1_HP) ||
                Input.GetButton(CommonScript.BUTTON_06_P1_LK) ||
                Input.GetButton(CommonScript.BUTTON_07_P1_MK) ||
                Input.GetButton(CommonScript.BUTTON_08_P1_HK)
            ))
            ||
            (playerNumber == 2 && (
                Input.GetButton(CommonScript.BUTTON_13_P2_LP) ||
                Input.GetButton(CommonScript.BUTTON_14_P2_MP) ||
                Input.GetButton(CommonScript.BUTTON_15_P2_HP) ||
                Input.GetButton(CommonScript.BUTTON_16_P2_LK) ||
                Input.GetButton(CommonScript.BUTTON_17_P2_MK) ||
                Input.GetButton(CommonScript.BUTTON_18_P2_HK)
            ))
        )
        {
            // 下キー: -1、上キー: 1
            float leverY = Input.GetAxisRaw(CommonScript.BUTTON_02_P1_VE);
            float startY;

            if (0 < leverY)// 上段だぜ☆
            {
                startY = 1.2f;
            }
            else if (0 == leverY)// 中段だぜ☆
            {
                startY = 0.6f;
            }
            else // 下段だぜ☆
            {
                startY = 0.0f;
            }

            // 弾の作成☆
            GameObject newBullet = Instantiate(bullet, transform.position + new Vector3(0f, startY, 0f), transform.rotation);
            SpriteRenderer newBulletSpriteRenderer = newBullet.GetComponent<SpriteRenderer>();

            // 弾の画像を差し替えたいぜ☆（＾～＾）
            {
                int r = Random.Range(0, 14);
                Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/弾１_将棋の駒");
                Sprite sprite2;
                switch (r)
                {
                    case 0: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("弾１_将棋の駒_0")); break;//歩
                    case 1: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("弾１_将棋の駒_1")); break;//香
                    case 2: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("弾１_将棋の駒_2")); break;//桂
                    case 3: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("弾１_将棋の駒_3")); break;//銀
                    case 4: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("弾１_将棋の駒_4")); break;//金
                    case 5: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("弾１_将棋の駒_5")); break;//角
                    case 6: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("弾１_将棋の駒_6")); break;//飛
                    case 7: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("弾１_将棋の駒_7")); break;//玉
                    case 8: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("弾１_将棋の駒_8")); break;//と
                    case 9: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("弾１_将棋の駒_9")); break;//杏
                    case 10: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("弾１_将棋の駒_10")); break;//圭
                    case 11: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("弾１_将棋の駒_11")); break;//全
                    case 12: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("弾１_将棋の駒_12")); break;//馬
                    default: sprite2 = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals("弾１_将棋の駒_13")); break;//竜
                }
                newBulletSpriteRenderer.sprite = sprite2;
            }
        }
    }

}
