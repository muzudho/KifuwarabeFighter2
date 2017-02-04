using System.Collections.Generic;

namespace StellaQL.Acons
{
    public abstract class AconSelect : AbstractAControll
    {
        public const string BASELAYER_ = "Base Layer.";
        public const string BASELAYER_MOVE = "Base Layer.Move";
        public const string BASELAYER_READY = "Base Layer.Ready";
        public const string BASELAYER_STAY = "Base Layer.Stay";
        public const string BASELAYER_TIMEOVER = "Base Layer.Timeover";
        public AconSelect()
        {
            Code.Register(StateHash_to_record, new List<AcStateRecordable>()
            {
                new DefaultAcState( BASELAYER_),
                new DefaultAcState( BASELAYER_MOVE),
                new DefaultAcState( BASELAYER_READY),
                new DefaultAcState( BASELAYER_STAY),
                new DefaultAcState( BASELAYER_TIMEOVER),
            });
        }
    }
}
