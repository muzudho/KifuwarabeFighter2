/// The license of this file is unknown. Author: 2dgames_jp
/// 出典 http://qiita.com/2dgames_jp/items/11bb76167fb44bb5af5f
using UnityEngine;
using System.Collections;

/// キャラクター基底クラス.
/// SpriteRendererが必要.
[RequireComponent(typeof(SpriteRenderer))]
public class TakoyakiTokenScript : MonoBehaviour
{
    /// プレハブ取得.
    /// プレハブは必ず"Resources/Prefabs/"に配置すること.
    public static GameObject GetPrefab(GameObject prefab, string name)
    {
        return prefab ?? (prefab = Resources.Load("Prefabs/" + name) as GameObject);
    }

    /// インスタンスを生成してスクリプトを返す.
    public static Type CreateInstance<Type>(GameObject prefab, Vector3 p, float direction = 0.0f, float speed = 0.0f) where Type : TakoyakiTokenScript
    {
        GameObject g = Instantiate(prefab, p, Quaternion.identity) as GameObject;
        Type obj = g.GetComponent<Type>();
        obj.SetVelocity(direction, speed);
        return obj;
    }

    public static Type CreateInstance2<Type>(GameObject prefab, float x, float y, float direction = 0.0f, float speed = 0.0f) where Type : TakoyakiTokenScript
    {
        Vector3 pos = new Vector3(x, y, 0);
        return CreateInstance<Type>(prefab, pos, direction, speed);
    }

    /// 生存フラグ.
    bool m_exists_ = false;

    public bool Exists
    {
        get { return m_exists_; }
        set { m_exists_ = value; }
    }

    /// アクセサ.
    /// レンダラー.
    SpriteRenderer m_renderer_ = null;

    public SpriteRenderer Renderer
    {
        get { return m_renderer_ ?? (m_renderer_ = gameObject.GetComponent<SpriteRenderer>()); }
    }

    /// 描画フラグ.
    public bool Visible
    {
        get { return Renderer.enabled; }
        set { Renderer.enabled = value; }
    }

    /// ソーティングレイヤー名.
    public string SortingLayer
    {
        get { return Renderer.sortingLayerName; }
        set { Renderer.sortingLayerName = value; }
    }

    /// ソーティング・オーダー.
    public int SortingOrder
    {
        get { return Renderer.sortingOrder; }
        set { Renderer.sortingOrder = value; }
    }

    /// 座標(X).
    public float X
    {
        set
        {
            Vector3 pos = transform.position;
            pos.x = value;
            transform.position = pos;
        }
        get { return transform.position.x; }
    }

    /// 座標(Y).
    public float Y
    {
        set
        {
            Vector3 pos = transform.position;
            pos.y = value;
            transform.position = pos;
        }
        get { return transform.position.y; }
    }

    /// 座標を足し込む.
    public void AddPosition(float dx, float dy)
    {
        X += dx;
        Y += dy;
    }

    /// 座標を設定する.
    public void SetPosition(float x, float y)
    {
        Vector3 pos = transform.position;
        pos.Set(x, y, 0);
        transform.position = pos;
    }

    /// スケール値(X).
    public float ScaleX
    {
        set
        {
            Vector3 scale = transform.localScale;
            scale.x = value;
            transform.localScale = scale;
        }
        get { return transform.localScale.x; }
    }

    /// スケール値(Y).
    public float ScaleY
    {
        set
        {
            Vector3 scale = transform.localScale;
            scale.y = value;
            transform.localScale = scale;
        }
        get { return transform.localScale.y; }
    }

    /// スケール値を設定.
    public void SetScale(float x, float y)
    {
        Vector3 scale = transform.localScale;
        scale.Set(x, y, (x + y) / 2);
        transform.localScale = scale;
    }

    /// スケール値(X/Y).
    public float Scale
    {
        get
        {
            Vector3 scale = transform.localScale;
            return (scale.x + scale.y) / 2.0f;
        }
        set
        {
            Vector3 scale = transform.localScale;
            scale.x = value;
            scale.y = value;
            transform.localScale = scale;
        }
    }

    /// スケール値を足し込む.
    public void AddScale(float d)
    {
        Vector3 scale = transform.localScale;
        scale.x += d;
        scale.y += d;
        transform.localScale = scale;
    }

    /// スケール値をかける.
    public void MulScale(float d)
    {
        transform.localScale *= d;
    }

    /// 剛体.
    Rigidbody2D m_rigidbody2D_ = null;

    public Rigidbody2D RigidBody
    {
        get { return m_rigidbody2D_ ?? (m_rigidbody2D_ = gameObject.GetComponent<Rigidbody2D>()); }
    }

    /// 移動量を設定.
    public void SetVelocity(float direction, float speed)
    {
        Vector2 v;
        v.x = TakoyakiUtilScript.CosEx(direction) * speed;
        v.y = TakoyakiUtilScript.SinEx(direction) * speed;
        RigidBody.velocity = v;
    }

    /// 移動量を設定(X/Y).
    public void SetVelocityXY(float vx, float vy)
    {
        Vector2 v;
        v.x = vx;
        v.y = vy;
        RigidBody.velocity = v;
    }

    /// 移動量をかける.
    public void MulVelocity(float d)
    {
        RigidBody.velocity *= d;
    }

    /// 移動量(X).
    public float VX
    {
        get { return RigidBody.velocity.x; }
        set
        {
            Vector2 v = RigidBody.velocity;
            v.x = value;
            RigidBody.velocity = v;
        }
    }

    /// 移動量(Y).
    public float VY
    {
        get { return RigidBody.velocity.y; }
        set
        {
            Vector2 v = RigidBody.velocity;
            v.y = value;
            RigidBody.velocity = v;
        }
    }

    /// 方向.
    public float Direction
    {
        get
        {
            Vector2 v = m_rigidbody2D_.velocity;
            return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        }
    }

    /// 速度.
    public float Speed
    {
        get
        {
            Vector2 v = m_rigidbody2D_.velocity;
            return Mathf.Sqrt(v.x * v.x + v.y * v.y);
        }
    }

    /// 重力.
    public float GravityScale
    {
        get { return RigidBody.gravityScale; }
        set { RigidBody.gravityScale = value; }
    }

    /// 回転角度.
    public float Angle
    {
        set { transform.eulerAngles = new Vector3(0, 0, value); }
        get { return transform.eulerAngles.z; }
    }

    /// スプライトの設定.
    public void SetSprite(Sprite sprite)
    {
        Renderer.sprite = sprite;
    }

    /// 色設定.
    public void SetColor(float r, float g, float b)
    {
        var c = Renderer.color;
        c.r = r;
        c.g = g;
        c.b = b;
        Renderer.color = c;
    }

    /// アルファ値を設定.
    public void SetAlpha(float a)
    {
        var c = Renderer.color;
        c.a = a;
        Renderer.color = c;
    }

    /// アルファ値を取得.
    public float GetAlpha()
    {
        var c = Renderer.color;
        return c.a;
    }

    /// アルファ値.
    public float Alpha
    {
        set { SetAlpha(value); }
        get { return GetAlpha(); }
    }

    /// サイズを設定.
    float _width = 0.0f;
    float _height = 0.0f;

    public void SetSize(float width, float height)
    {
        _width = width;
        _height = height;
    }

    /// スプライトの幅.
    public float SpriteWidth
    {
        get { return Renderer.bounds.size.x; }
    }

    /// スプライトの高さ.
    public float SpriteHeight
    {
        get { return Renderer.bounds.size.y; }
    }

    /// コリジョン（円）.
    CircleCollider2D m_circleCollider_ = null;

    public CircleCollider2D CircleCollider
    {
        get { return m_circleCollider_ ?? (m_circleCollider_ = GetComponent<CircleCollider2D>()); }
    }
    // 円コリジョンの半径.
    public float CollisionRadius
    {
        get { return CircleCollider.radius; }
        set { CircleCollider.radius = value; }
    }
    // 円コリジョンの有効無効を設定する.
    public bool CircleColliderEnabled
    {
        get { return CircleCollider.enabled; }
        set { CircleCollider.enabled = value; }
    }

    /// コリジョン（矩形）.
    BoxCollider2D m_boxCollider_ = null;

    public BoxCollider2D BoxCollider
    {
        get { return m_boxCollider_ ?? (m_boxCollider_ = GetComponent<BoxCollider2D>()); }
    }

    /// 矩形コリジョンの幅.
    public float BoxColliderWidth
    {
        get { return BoxCollider.size.x; }
        set
        {
            var size = BoxCollider.size;
            size.x = value;
            BoxCollider.size = size;
        }
    }

    /// 矩形コリジョンの高さ.
    public float BoxColliderHeight
    {
        get { return BoxCollider.size.y; }
        set
        {
            var size = BoxCollider.size;
            size.y = value;
            BoxCollider.size = size;
        }
    }
    // 箱コリジョンのサイズを設定する.
    public void SetBoxColliderSize(float w, float h)
    {
        BoxCollider.size.Set(w, h);
    }
    // 箱コリジョンの有効無効を設定する
    public bool BoxColliderEnabled
    {
        get { return BoxCollider.enabled; }
        set { BoxCollider.enabled = value; }
    }

    /// 移動して画面内に収めるようにする.
    public void ClampScreenAndMove(Vector2 v)
    {
        Vector2 min = GetWorldMin();
        Vector2 max = GetWorldMax();
        Vector2 pos = transform.position;
        pos += v;

        // 画面内に収まるように制限をかける.
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        // プレイヤーの座標を反映.
        transform.position = pos;
    }

    /// 画面内に収めるようにする.
    public void ClampScreen()
    {
        Vector2 min = GetWorldMin();
        Vector2 max = GetWorldMax();
        Vector2 pos = transform.position;
        // 画面内に収まるように制限をかける.
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        // プレイヤーの座標を反映.
        transform.position = pos;
    }

    /// 画面外に出たかどうか.
    public bool IsOutside()
    {
        Vector2 min = GetWorldMin();
        Vector2 max = GetWorldMax();
        Vector2 pos = transform.position;
        if (pos.x < min.x || pos.y < min.y)
        {
            return true;
        }
        if (pos.x > max.x || pos.y > max.y)
        {
            return true;
        }
        return false;
    }

    /// 画面の左下のワールド座標を取得する.
    public Vector2 GetWorldMin(bool noMergin = false)
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(Vector2.zero);
        if (noMergin)
        {
            // そのまま返す.
            return min;
        }

        // 自身のサイズを考慮する.
        min.x += _width;
        min.y += _height;
        return min;
    }

    /// 画面右上のワールド座標を取得する.
    public Vector2 GetWorldMax(bool noMergin = false)
    {
        Vector2 max = Camera.main.ViewportToWorldPoint(Vector2.one);
        if (noMergin)
        {
            // そのまま返す.
            return max;
        }

        // 自身のサイズを考慮する.
        max.x -= _width;
        max.y -= _height;
        return max;
    }

    /// 消滅（メモリから削除）.
    public void DestroyObj()
    {
        Destroy(gameObject);
    }

    /// アクティブにする.
    public virtual void Revive()
    {
        gameObject.SetActive(true);
        Exists = true;
        Visible = true;
    }

    /// 消滅する（オーバーライド可能）
    /// ただし base.Vanish()を呼ばないと消滅しなくなることに注意
    public virtual void Vanish()
    {
        VanishCannotOverride();
    }
    /// 消滅する（オーバーライド禁止）
    public void VanishCannotOverride()
    {
        gameObject.SetActive(false);
        Exists = false;
    }
}
