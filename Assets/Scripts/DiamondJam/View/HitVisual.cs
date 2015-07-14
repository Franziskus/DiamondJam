using System;
using UnityEngine;

namespace DiamondJam.View
{
	/// <summary>
	/// HitVisual is a MonoBehaviour that realize IHitable.
	/// </summary>
	public class HitVisual: MonoBehaviour, IHitable
	{
		/// <summary>
		/// A Hitable tries to in invoke this when it's hit.
		/// </summary>
		public event PlayerHit hitEvent;

		/// <summary>
		/// Collider of the instace
		/// </summary>
		public Collider2D coll2D;


		public virtual void Awake() {
			coll2D = GetComponent<Collider2D>();
		}

		/// <summary>
		/// The Player has touch this element. Invoke event
		/// </summary>
		public void HitMe(){
			if(hitEvent != null)
				hitEvent((int)transform.position.x,(int)-transform.position.y);
		}

		/// <summary>
		/// Tests for hit with Vector2 used by Touch controls.
		/// </summary>
		/// <returns><c>true</c>, if the player hits this, <c>false</c> otherwise.</returns>
		/// <param name="screenPos">Screen position.</param>
		private bool TestForHit(Vector2 screenPos){
			return TestForHit(new Vector3(screenPos.x, screenPos.y, 0)); 
		}

		/// <summary>
		/// Tests for hit with Vector3 used by Mouse controls.
		/// </summary>
		/// <returns><c>true</c>, if the player hits this, <c>false</c> otherwise.</returns>
		/// <param name="screenPos">Screen position.</param>
		private bool TestForHit(Vector3 screenPos){
			Vector3 wp = Camera.main.ScreenToWorldPoint(screenPos);
			Vector2 pos = new Vector2(wp.x, wp.y);
			return coll2D == Physics2D.OverlapPoint(pos);
		}

		/// <summary>
		/// Check if the Player touch this element
		/// </summary>
		public virtual void Update () {
			if (Input.touchCount == 1)
			{ 
				if(Input.touches[0].phase == TouchPhase.Began && TestForHit((Vector2)Input.GetTouch(0).position))
					HitMe();
			}else if(Input.GetMouseButtonDown(0)){
				if(TestForHit(Input.mousePosition))
					HitMe();
			}
		}
	}
}

