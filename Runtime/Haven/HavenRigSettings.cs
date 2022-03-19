//-----------------------------------------------------------------------
// <copyright file="HavenRigSettings.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace Lost.XR
{
    public enum Hand
    {
        Left,
        Right
    }

    public enum MovementMode
    {
        TeleportOnly,
        ContinuousOnly,
        ContinuousAndTeleport,
    }

    public enum TurnType
    {
        Continuous,
        Snap,
    }

    public enum ContinuousMovementSource
    {
        Head,
        LeftHand,
        RightHand,
    }

    [Serializable]
    public struct HavenRigSettings
    {
        public static readonly HavenRigSettings Default = new HavenRigSettings
        {
            MovementMode = MovementMode.ContinuousAndTeleport,
            TurnType = TurnType.Snap,
            EnableTurnAround = false,

            MovementHand = Hand.Left,
            MovementSource = ContinuousMovementSource.Head,
            AllowStrafe = true,

            TurnHand = Hand.Right,
            ContinuousTurnSpeed = 180f,
            SnapTurnAmmount = 30.0f,
        };

        public MovementMode MovementMode;
        public TurnType TurnType;
        public bool EnableTurnAround;

        // Continuous Movement
        public Hand MovementHand;
        public ContinuousMovementSource MovementSource;
        public bool AllowStrafe;

        // Turning Options
        public Hand TurnHand;
        public float ContinuousTurnSpeed;
        public float SnapTurnAmmount;
    }
}
