using UnityEngine;

namespace CustomEventBus.Signals
{
    public class AvatarChangedSignal
    {
        public readonly Sprite Avatar;

        public AvatarChangedSignal(Sprite avatar)
        {
            Avatar = avatar;
        }
    }
}
