using UnityEngine;

namespace RootMotion
{
    public static class TransformExtended
    {
        public static Transform FoundTransByName(Transform trans, string name)
        {
            if (trans.name == name) return trans;

            foreach (Transform c in trans)
            {
                var result = FoundTransByName(c, name);
                if (result != null) return result;
            }

            return null;
        }  
    }
}