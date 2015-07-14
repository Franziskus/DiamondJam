using System;
using UnityEngine;
using DiamondJam.Math;
using DiamondJam.Control;

namespace DiamondJam.Control.Gems
{
	/// <summary>
	/// Gem is the superclass of all items in the game.
	/// It's the logical representation of an item.
	/// 
	/// It handles the activation (When a player clicks on it or chain reaction) and
	/// the popping (when it gets destroyed or trying to destroy it).
	/// </summary>
	public abstract class Gem: ICloneable
	{
		/// <summary>
		/// The group of an item. All neighbours of a group have the same number.
		/// The context uses this field after every update.
		/// </summary>
		public int group;

		/// <summary>
		/// Can this item be part of a chain reaction?
		/// </summary>
		public bool chain = false;

		/// <summary>
		/// Initializes this Gem with passing the level context.
		/// </summary>
		/// <param name="context">Context.</param>
		public virtual void Init(IGemContext context){
		}

		/// <summary>
		/// Determines whether this instance is popabel.
		/// For example the Target should never get popped.
		/// </summary>
		/// <returns><c>true</c> if this instance is popabel; otherwise, <c>false</c>.</returns>
		public virtual bool IsPopabel(){
			return true;
		}

		/// <summary>
		/// What should happen if the Player click on the item.
		/// 
		/// If updateOnly is true after execution then the item edited
		/// the level by itself. 
		/// Otherwise the context need to pop the positions of the return
		/// array.
		/// In both cases the returned array contains the touched items.
		/// 
		/// For chain reaction types it's important that the first position
		/// in array are the item itself. Otherwise it is likely to create a
		/// endless loop.
		/// </summary>
		/// <param name="pos">Position in the level</param>
		/// <param name="updateOnly">UpdateOnly: false = context has to pop. Otherwise item thaking care by itself</param>
		/// <returns>Array of item position to pop or to update</returns>
		public abstract IntVector2[] Activate(IntVector2 pos, out bool updateOnly);

		/// <summary>
		/// Pop this instance.
		/// </summary>
		/// <returns>true = Item is destroyed. Otherwise false</returns>
		public virtual bool Pop(){
			return IsPopabel();
		}

		public abstract object Clone();

	}
}

