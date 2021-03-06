//-----------------------------------------------------------------------
// <copyright file="HavenSpawnPoint.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY

namespace Lost.Haven
{
    using System.Collections;
    using UnityEngine;

    [AddComponentMenu("Haven XR/HXR Spawn Point")]
    public class HavenSpawnPoint : MonoBehaviour, IOnManagersReady
    {
        private void Awake() => ManagersReady.Register(this);

        public void OnManagersReady()
        {
            CoroutineRunner.Instance.StartCoroutine(Coroutine());

            IEnumerator Coroutine()
            {
                yield return HavenRig.WaitForRig();
                var rig = HavenRig.Instance;
                rig.transform.position = this.transform.position;
            }
        }
    }
}

#endif
