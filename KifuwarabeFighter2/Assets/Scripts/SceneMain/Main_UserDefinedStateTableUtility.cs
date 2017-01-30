using StellaQL;

namespace SceneMain
{
    public abstract class Main_UserDefinedStateTableUtility
    {

        /// <summary>
        /// キャラクターと、モーション、現在のフレームを指定することで、通し画像番号とスライス番号を返す。
        /// これにより Hitbox2DScript と連携を取ることができる。
        /// </summary>
        /// <param name="serialTilesetfileIndex"></param>
        /// <param name="slice"></param>
        /// <param name="character"></param>
        /// <param name="motion"></param>
        /// <param name="currentMotionFrame"></param>
        public static void GetSlice(out int serialTilesetfileIndex, out int slice, CharacterIndex character, CliptypeExRecordable cliptypeExRecord, int currentMotionFrame)
        {
            serialTilesetfileIndex = Hitbox2DOperationScript.GetSerialImageIndex(character, (TilesetfileTypeIndex)cliptypeExRecord.TilesetfileTypeIndex);
            slice = cliptypeExRecord.Slices[currentMotionFrame];
        }
    }
}
