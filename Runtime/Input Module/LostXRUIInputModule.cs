//-----------------------------------------------------------------------
// <copyright file="HavenXRUIInputModule.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost.XR
{
    [UnityEngine.AddComponentMenu("")]
#if USING_UNITY_XR_INTERACTION_TOOLKIT
    public class LostXRUIInputModule : UnityEngine.XR.Interaction.Toolkit.UI.XRUIInputModule
#else
    public class LostXRUIInputModule : UnityEngine.MonoBehaviour
#endif
    {
    }
}

#endif
