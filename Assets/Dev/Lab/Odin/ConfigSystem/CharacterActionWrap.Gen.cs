namespace LS.Game
{
    public partial class EnumWrap
    {
        public static EnumWrap None;
        public static EnumWrap Run;
        public static EnumWrap Jump;
        public static EnumWrap Roll;
        public static EnumWrap Attack;
        public static EnumWrap Aim;
        public static EnumWrap AimCancel;
        public static EnumWrap Shoot;
        public static EnumWrap AttackCharge;
        public static EnumWrap AttackJustRelease;
        public static EnumWrap AttackRelease;
        public static EnumWrap Defend;
        public static EnumWrap UseItem;
        public static EnumWrap Interact;
        public static EnumWrap TestTemp;

        static EnumWrap()
        {
            None = new EnumWrap(CharacterAction.None);
            Run = new EnumWrap(CharacterAction.Run);
            Jump = new EnumWrap(CharacterAction.Jump);
            Roll = new EnumWrap(CharacterAction.Roll);
            Attack = new EnumWrap(CharacterAction.Attack);
            Aim = new EnumWrap(CharacterAction.Aim);
            AimCancel = new EnumWrap(CharacterAction.AimCancel);
            Shoot = new EnumWrap(CharacterAction.Shoot);
            AttackCharge = new EnumWrap(CharacterAction.AttackCharge);
            AttackJustRelease = new EnumWrap(CharacterAction.AttackJustRelease);
            AttackRelease = new EnumWrap(CharacterAction.AttackRelease);
            Defend = new EnumWrap(CharacterAction.Defend);
            UseItem = new EnumWrap(CharacterAction.UseItem);
            Interact = new EnumWrap(CharacterAction.Interact);
            TestTemp = new EnumWrap(CharacterAction.TestTemp);
        }
    }
}