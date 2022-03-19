//-----------------------------------------------------------------------
// <copyright file="HavenContinuousTurnProvider.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost.XR
{
    using UnityEngine;
    using UnityEngine.XR.Interaction.Toolkit;

    public class HavenContinuousTurnProvider : ContinuousTurnProviderBase
    {
        private static readonly Vector2 Zero = new Vector2(0.0f, 0.0f);

        private HavenHand turnHand;
        private bool isLeftHand;
        private bool enableTurnAround;

        public void UpdateSettings(HavenRigSettings settings, HavenHand leftHand, HavenHand rightHand)
        {
            this.enabled = settings.TurnType == TurnType.Continuous;
            this.turnSpeed = settings.ContinuousTurnSpeed;
            this.isLeftHand = settings.TurnHand == Hand.Left;
            this.turnHand = this.isLeftHand ? leftHand : rightHand;
            this.enableTurnAround = settings.EnableTurnAround;
        }

        protected override Vector2 ReadInput()
        {
            if (this.turnHand.IsTeleporting || this.turnHand.DidJustTeleport)
            {
                return Zero;
            }

            return this.isLeftHand ? XRControllersState.Instance.LeftStick : XRControllersState.Instance.RightStick;
        }

        protected override float GetTurnAmount(Vector2 input)
        {
            // Special case for turn around
            if (this.enableTurnAround && input.y < -0.9f)
            {
                return 180;
            }

            return base.GetTurnAmount(input);
        }
    }
}
