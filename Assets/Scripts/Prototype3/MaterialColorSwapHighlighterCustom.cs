namespace VRTK.Highlighters
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;


	public class MaterialColorSwapHighlighterCustom : VRTK_MaterialColorSwapHighlighter {

				protected InteractableObjectCustom interactableObjScript;

				public override void Initialise(Color? color = null, Dictionary<string, object> options = null)
        {
					interactableObjScript = GetComponent<InteractableObjectCustom>();
					base.Initialise(color, options);
				}

		/// <summary>
        /// The Highlight method initiates the change of colour on the object and will fade to that colour (from a base white colour) for the given duration.
        /// </summary>
        /// <param name="color">The colour to highlight to.</param>
        /// <param name="duration">The time taken to fade to the highlighted colour.</param>
        public override void Highlight(Color? color, float duration = 0f)
        {
            if (color == null)
            {
                return;
            }
            if(interactableObjScript.GetForceReleaseTime() != Time.time) {
							ChangeToHighlightColor((Color)color, duration);
						}
        }
	}
}
