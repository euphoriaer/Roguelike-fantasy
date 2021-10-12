// (c) Copyright HutongGames, LLC 2021. All rights reserved.

#if ENABLE_INPUT_SYSTEM

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("PlayerInput")]
	[Tooltip("Get the Vector2 value from an InputAction in a PlayerInput component.")]
	public class PlayerInputGetVector2 : PlayerInputUpdateActionBase
    {
        [UIHint(UIHint.Variable)]
        public FsmVector2 storeVector2;

        [UIHint(UIHint.Variable)]
        public FsmFloat storeX;

        [UIHint(UIHint.Variable)]
        public FsmFloat storeY;

        public override void Reset()
		{
			base.Reset();
            storeVector2 = null;
            storeX = null;
            storeY = null;
        }

        // Note we're updating by polling instead of using callbacks.
        // This is because this action is in a stack with other actions
        // updated in order, from top to bottom.
        // If this action was updated out of sync with the other actions,
        // it could lead to unexpected behaviour that would be hard to debug.

        protected override void Execute()
        {
            if (action == null) return;

            storeVector2.Value = action.ReadValue<Vector2>();
            storeX.Value = storeVector2.Value.x;
            storeY.Value = storeVector2.Value.y;
        }
    }
}

#endif