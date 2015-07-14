using System;
using DiamondJam.Math;

namespace DiamondJam.Control.Gems
{
	/// <summary>
	/// The Target is the hippo you have to rescue. It can NOT pop or activated.
	/// Winning conditions are checked in <see cref="DiamondJam.Control.Rules.RescueTargetRules"/>.
	/// </summary>
	public class Target: Gem
	{
		/// <summary>
		/// Returns an empty array.
		/// </summary>
		/// <param name="pos">Position in the level</param>
		/// <param name="updateOnly">UpdateOnly is always true</param>
		/// <returns>Retuns an empty array</returns>
		public override IntVector2[] Activate (DiamondJam.Math.IntVector2 pos, out bool updateOnly)
		{
			updateOnly = true;
			return new IntVector2[0];
		}

		/// <summary>
		/// Target is not popabel.
		/// </summary>
		/// <returns>always false</returns>
		public override bool IsPopabel(){
			return false;
		}

		public override object Clone(){
			Target t = (Target)this.MemberwiseClone();
			return t;
		}
	}
}

