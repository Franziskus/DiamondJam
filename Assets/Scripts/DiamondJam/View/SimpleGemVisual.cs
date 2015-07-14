using System;
using UnityEngine;
using DiamondJam.Control.Gems;
using DiamondJam.Math;

namespace DiamondJam.View
{
	/// <summary>
	/// SimpleGemVisual is a MonoBehaviour that realize IGemVisual.
	/// </summary>
	[RequireComponent(typeof(SpriteRenderer))]
	public class SimpleGemVisual : HitVisual, IGemVisual
	{
		public Sprite green;
		public Sprite red;
		public Sprite lime;
		public Sprite orange;
		public Sprite blue; 
		public Sprite white;
		public Sprite target;
		public Sprite bomb;
		public Sprite firework;
		public Sprite turn;
		
		public GameObject ice;
		public GameObject bonus;

		private float animationDuration = 0.5f;
		private float animationEndTime;
		private SpriteRenderer spriteRenderer;
		private IntVector2 startPos;
		private IntVector2 targetPos;

        /// <summary>
        /// Initialize this Class
        /// </summary>
		public override void Awake() {
			base.Awake();
			spriteRenderer = GetComponent<SpriteRenderer>();
			startPos = new IntVector2((int)transform.position.x, (int)-transform.position.y);
			targetPos = startPos;
		}

        /// <summary>
        /// Simulate a movement to target position. In a specific time.
        /// </summary>
        /// <param name="targetPos">target position</param>
        /// <param name="animationDuration">time to complete the movement</param>
		public void MoveTo(IntVector2 targetPos, float animationDuration){
			this.targetPos = targetPos;
			animationEndTime = Time.time + animationDuration;
			this.animationDuration = animationDuration;		
		}

        /// <summary>
        /// This method is called every frame.
        /// It's reasonable for the movement animation.
        /// if the target position is not same as the initial position 
        /// we will lerp between initial an target position depending on the animationDuration.
        /// </summary>
		public override void Update(){
			base.Update();
			if(targetPos != startPos){
				float lerpPos = animationEndTime - Time.time;
				lerpPos /= animationDuration;
				if(lerpPos >= 0){
					Vector3 pos = Vector3.Lerp(new Vector3(targetPos.x,-targetPos.y,0), new Vector3(startPos.x,-startPos.y,0),lerpPos);
					transform.position = pos;
				}else{
					targetPos = startPos;
				}
			}
		}

		/// <summary>
		/// Change your visuals to represent this Gem
		/// </summary>
		/// <param name="g">The Gem item.</param>
		public void Show(Gem g){ 
			transform.position = new Vector3(startPos.x,-startPos.y,0);
			name = "("+(int)transform.position.x+","+(-(int)transform.position.y)+")";
			if(g == null){
				spriteRenderer.enabled = false;
				ice.SetActive(false);
				bonus.SetActive(false);
				name = "None "+name;
			}else if(g is ColorGem){
				spriteRenderer.enabled = true;
				ColorGem cg = (ColorGem)g;
				name = "Color "+name;
				switch (cg.color) {
				case ColorGem.GemColor.WHITE:
					spriteRenderer.sprite = white;
					break;
				case ColorGem.GemColor.BLUE:
					spriteRenderer.sprite = blue;
					break;
				case ColorGem.GemColor.GREEN:
					spriteRenderer.sprite = green;
					break;
				case ColorGem.GemColor.LIME:
					spriteRenderer.sprite = lime;
					break;
				case ColorGem.GemColor.ORANGE:
					spriteRenderer.sprite = orange;
					break;
				case ColorGem.GemColor.RED:
					spriteRenderer.sprite = red;
					break;
				}
				if(cg is IcedColorGem){
					name = "Iced"+name;
					int icehp = ((IcedColorGem)cg).hitpoints;
					ice.SetActive(icehp > 0);
				}else{
					ice.SetActive(false);
				}

				if(cg is BonusColorGem){
					name = "Bonus"+name;
					bonus.SetActive(true);
				}else{
					bonus.SetActive(false);
				}
			}else{
				spriteRenderer.enabled = true;
				bonus.SetActive(false);
				ice.SetActive(false);
				if(g is Bomb){
					name = "Bomb "+name;
					spriteRenderer.sprite = bomb;
				}else if(g is Firework){
					name = "Firework "+name;
					spriteRenderer.sprite = firework;
				}else if(g is Target){
					name = "Target "+name;
					spriteRenderer.sprite = target;
				}else if(g is Turn){
					name = "Turn "+name;
					spriteRenderer.sprite = turn;
				}else{
					Debug.LogError("Unknown Gem Type in Visuals");
				}
			}
		}


	}
}

