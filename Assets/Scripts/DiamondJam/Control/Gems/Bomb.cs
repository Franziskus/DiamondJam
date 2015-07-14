using System;
using System.Collections.Generic;
using DiamondJam.Math;

namespace DiamondJam.Control.Gems
{
	/// <summary>
	/// Bomb blows items around this position away.
	/// Can be part of a chain.
	/// </summary>
	public class Bomb : Gem
	{
		/// <summary>
		/// Positions for the explosion. Leaving yourself out because needs to added in first place.
		/// </summary>
		private IntVector2[] explosion={
			                                           new IntVector2(0,-2),
			                     new IntVector2(-1,-1),new IntVector2(0,-1), new IntVector2(1,-1),
			new IntVector2(-2,0),new IntVector2(-2,-1), 				     new IntVector2(1, 0),new IntVector2(2,0),
			                     new IntVector2(-1, 1),new IntVector2(0, 1), new IntVector2(1, 1),
			                                           new IntVector2(0, 2),
		};

		public IGemContext context;

		/// <summary>
		/// Initializes a new instance of the <see cref="DiamondJam.Control.Gems.Bomb"/> class.
		/// </summary>
		public Bomb(){
			base.chain = true;
		}

		/// <summary>
		/// Initializes this Gem with passing the level context.
		/// </summary>
		/// <param name="context">Context.</param>
		public override void Init(IGemContext context){
			this.context = context;
		} 

		/// <summary>
		/// Returns an area around the Bomb.
		/// </summary>
		/// <param name="pos">Position in the level</param>
		/// <param name="updateOnly">UpdateOnly is always false</param>
		/// <returns>Array of item position to pop.</returns>
		public override IntVector2[] Activate(IntVector2 pos, out bool updateOnly){
			updateOnly = false;
			List<IntVector2> back = new List<IntVector2>();
			back.Add(pos);
			for(int i = 0; i < explosion.Length; i++){
				IntVector2 testPos = pos + explosion[i];
				if(context.IsInLevelBounce(pos + explosion[i]))
				   back.Add(testPos);
			}
			return back.ToArray();
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="DiamondJam.Control.Gems.Bomb"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="DiamondJam.Control.Gems.Bomb"/>.</returns>
		public override string ToString(){
			return "Bomb [group = "+group+" chain = "+chain+"]";
		}

		public override object Clone(){
			Bomb b = (Bomb)this.MemberwiseClone();
			return b;
		}
	}
}

