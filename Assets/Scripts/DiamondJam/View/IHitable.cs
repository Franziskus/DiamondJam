using System;

public delegate void PlayerHit(int x, int y);

namespace DiamondJam.View
{
	/// <summary>
	/// Hitable indicates a element that can be clicked or touch by the player.
	/// </summary>
	public interface IHitable
	{
		/// <summary>
		/// A Hitable tries to in invoke this when it's hit.
		/// </summary>
		event PlayerHit hitEvent; 
	}
}

