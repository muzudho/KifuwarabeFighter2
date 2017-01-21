using System;

/// <summary>
/// 出典：http://baba-s.hatenablog.com/entry/2014/08/10/130145
/// 
/// Enum 型の拡張メソッドを管理するクラス
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// 現在のインスタンスで 1 つ以上のビット フィールドが設定されているかどうかを判断します。
    /// 探される方.HasFlag(探したいもの)
    /// </summary>
    public static bool HasFlag(this Enum self, Enum flag)
    {
        if (self.GetType() != flag.GetType())
        {
            throw new ArgumentException("flag の型が、現在のインスタンスの型と異なっています。");
        }

        var selfValue = Convert.ToUInt64(self);
        var flagValue = Convert.ToUInt64(flag);

        return (selfValue & flagValue) == flagValue;
    }
}
