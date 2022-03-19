//-----------------------------------------------------------------------
// <copyright file="XRInteractionHelper.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if true // UNITY && USING_UNITY_XR_INTERACTION_TOOLKIT

namespace Lost.XR
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.XR.Interaction.Toolkit;

    public static class XRInteractionHelper
    {
        private static XRInteractionManager xrInteractionManagerinstance;

        public static XRInteractionManager XRInteractionManagerInstance => xrInteractionManagerinstance;

        [EditorEvents.OnExitPlayMode]
        private static void OnExitPlayMode()
        {
            xrInteractionManagerinstance = null;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            FindOrCreateXRInteractionManager();
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private static void FindOrCreateXRInteractionManager()
        {
            if (xrInteractionManagerinstance != null)
            {
                return;
            }

            xrInteractionManagerinstance = GameObject.FindObjectOfType<XRInteractionManager>();

            if (xrInteractionManagerinstance == null)
            {
                xrInteractionManagerinstance = new GameObject("XRInteractionManager", typeof(XRInteractionManager)).GetComponent<XRInteractionManager>();
                GameObject.DontDestroyOnLoad(xrInteractionManagerinstance.gameObject);
            }
        }

        private static void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
        {
            FindOrCreateXRInteractionManager();
            FixTeleports();
        }

        private static void FixTeleports()
        {
            // xrInteractionManagerInstance.StartCoroutine(Coroutine());
            // 
            // IEnumerator Coroutine()
            // {
            //     yield return HavenRig.WaitForRig();
            // 
            //     var rig = HavenRig.GetRig();
            //     var teleportProvider = rig.GetComponentInChildren<TeleportationProvider>();
            // 
            //     foreach (var teleport in GameObject.FindObjectsOfType<BaseTeleportationInteractable>(true))
            //     {
            //         teleport.teleportationProvider = teleportProvider;
            //         teleport.interactionManager = xrInteractionManagerInstance;
            //     }
            // }
        }
    }
}

#endif
