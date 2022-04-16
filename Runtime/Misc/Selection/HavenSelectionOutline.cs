//-----------------------------------------------------------------------
// <copyright file="HavenSelectionOutline.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.XR.Interaction.Toolkit;

    [AddComponentMenu("Haven XR/Selection/HXR Selection Outline")]
    public class HavenSelectionOutline : MonoBehaviour, IAwake, IValidate
    {
#pragma warning disable 0649
        [SerializeField] private HavenSelectionOutlineSettingsObject settings;
        [SerializeField] private XRBaseInteractable interactable;
        [SerializeField] private XRBaseInteractor interactor;
        [SerializeField] private Renderer targetRenderer;
#pragma warning restore 0649

        private MaterialPropertyBlock materialPropertyBlock;
        private int highlightedPropertyId;
        private float highlightedValue;

        public void Validate(List<ValidationError> errors)
        {
            this.AssertNotNull(errors, this.settings, nameof(this.settings));
            this.AssertNotNull(errors, this.targetRenderer, nameof(this.targetRenderer));

            if (this.interactable == null && this.interactor == null)
            {
                string message = $"{nameof(HavenSelectionOutline)} has null {nameof(this.interactable)} AND {nameof(this.interactor)}.  This component will not work.";
                Debug.LogError(message, this);
                errors?.Add(new ValidationError { AffectedObject = this, Name = message });
            }
        }

        public void OnAwake()
        {
            this.highlightedPropertyId = this.settings.Settings.GetMaterialPropertyId();
            this.materialPropertyBlock = new MaterialPropertyBlock();
            this.materialPropertyBlock.SetFloat(this.highlightedPropertyId, this.highlightedValue);
            this.targetRenderer.SetPropertyBlock(this.materialPropertyBlock);

            if (this.interactor != null)
            {
                this.interactor.hoverEntered.AddListener(this.Highlight);
                this.interactor.hoverExited.AddListener(this.RemoveHighlight);
            }

            if (this.interactable != null)
            {
                this.interactable.hoverEntered.AddListener(this.Highlight);
                this.interactable.hoverExited.AddListener(this.RemoveHighlight);
            }
        }

        private void Awake() => ActivationManager.Register(this);

        private void OnDestroy()
        {
            if (this.interactor != null)
            {
                this.interactor.hoverEntered.RemoveListener(this.Highlight);
                this.interactor.hoverExited.RemoveListener(this.RemoveHighlight);
            }

            if (this.interactable != null)
            {
                this.interactable.hoverEntered.RemoveListener(this.Highlight);
                this.interactable.hoverExited.RemoveListener(this.RemoveHighlight);
            }
        }

        private void OnValidate()
        {
            EditorUtil.SetIfNull(this, ref this.interactable);
            EditorUtil.SetIfNull(this, ref this.interactor);
            EditorUtil.SetIfNull(this, ref this.targetRenderer);
            EditorUtil.SetIfNull(this, ref this.settings, "2f105c9520f0cc84fa47f1f66566e48d");
        }

        private void Highlight(HoverEnterEventArgs args)
        {
            this.highlightedValue = 1.0f;
            this.SetFloat(this.highlightedValue);
        }

        private void RemoveHighlight(HoverExitEventArgs args)
        {
            this.highlightedValue = 0.0f;
            this.SetFloat(this.highlightedValue);
        }

        private void SetFloat(float value)
        {
            this.targetRenderer.GetPropertyBlock(this.materialPropertyBlock);
            this.materialPropertyBlock.SetFloat(this.highlightedPropertyId, value);
            this.targetRenderer.SetPropertyBlock(this.materialPropertyBlock);
        }
    }
}
