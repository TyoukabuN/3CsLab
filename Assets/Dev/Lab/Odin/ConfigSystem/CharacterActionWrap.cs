using System;

namespace LS.Game
{
    public partial class EnumWrap
    {
        public Enum enumValue;
        public EntityActionTagConfigItem config;
        public MotionFlag motionFlag;
        public EnumWrap(Enum enumValue)
        {
            this.enumValue = enumValue;
            config = EntityActionTagConfig.GetConfigByEnum(enumValue);
            motionFlag = new MotionFlag(config.strValue);
        }
        public static implicit operator EntityActionTagConfigItem(EnumWrap host)
        {
            return host.config;
        }
    }
}