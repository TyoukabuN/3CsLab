using System;

namespace LS.Game
{
    public partial class ActionTagWrap
    {
        public Enum enumValue;
        public string strValue;
        //
        public EntityActionTagConfigItem config;
        public MotionFlag motionFlag;
        public ActionTagWrap(Enum enumValue)
        {
            this.enumValue = enumValue;
            config = EntityActionTagConfig.GetConfigByEnum(enumValue);
            motionFlag = new MotionFlag(config.strValue);
        }
        public ActionTagWrap(string strValue)
        {
            this.strValue = strValue;
            config = EntityActionTagConfig.GetConfigByStrValue(strValue);
            motionFlag = new MotionFlag(config.strValue);
        }
        public static implicit operator EntityActionTagConfigItem(ActionTagWrap host)
        {
            return host.config;
        }
    }
}