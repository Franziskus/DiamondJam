using System;
using DiamondJam.Math;

namespace DiamondJam.Control.Physics
{
	/// <summary>
	/// ILevelPhysics takes care of the level Physics
	/// </summary>
	public interface ILevelPhysics
	{
		/// <summary>
		/// Calculate the physics. Is the deepest y position in the level 
		/// that needs to update. The length of this array is the same 
		/// as the level width. If the pos is -1 then this column dos NOT
		/// need an update.
		/// </summary>
		/// <param name="updateYpos">Update ypos array with deepest update pos.</param>
		GemMove[] DoPhysics(int nr, int[] updateYpos);

		int PhysicsSteps();
	}
}

