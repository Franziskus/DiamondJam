using System;
using DiamondJam.Math;
using UnityEngine;
using System.Collections.Generic;

namespace DiamondJam.Control.Gems
{
	/// <summary>
	/// This Gem turns items around his position clockwise, and then destroys itself.
	/// </summary>
	public class Turn : Gem
	{
		public IGemContext context;

		/// <summary>
		/// Items to rotate. The order is important.
		/// </summary>
		private IntVector2[] items=new IntVector2[]{ 
			new IntVector2(-1,-1), new IntVector2( 0,-1), new IntVector2( 1,-1), new IntVector2( 1,0),
			new IntVector2( 1, 1), new IntVector2( 0, 1), new IntVector2(-1, 1), new IntVector2(-1,0)
		};

		/// <summary>
		/// Initializes this Gem with passing the level context.
		/// </summary>
		/// <param name="context">Context.</param>
		public override void Init(IGemContext context){
			this.context = context;
		}

		/// <summary>
		/// Turns items around his position clockwise. Then destroys itself.
		/// 
		/// It takes care of all the changes by itself. So no popping outside.
		/// Only physics needs to get used.
		/// </summary>
		/// <param name="pos">Position in the level</param>
		/// <param name="updateOnly">UpdateOnly always true</param>
		/// <returns>Array of item position to update</returns>
		public override IntVector2[] Activate(IntVector2 pos, out bool updateOnly){
			List<IntVector2> back = new List<IntVector2>();
			updateOnly = true;
			back.Add(pos);

			IntVector2 levelSize = context.GetSize();

			//Test if the position is in bounce. If the turn to near to the left, right top or ground
			//it only pops itself.
			if(
				pos.x != 0 && pos.y != 0 &&
				pos.x != levelSize.x-1 && pos.y != levelSize.y-1

			   ){
				//save the first item
				IntVector2 current = pos + items[0];
				Gem temp = context.GetItem(current);

				//swap all items clockwise.
				IntVector2 last = current;
				for(int i = items.Length -1; i > 0; i--){
					current = pos + items[i];
					context.SetItem(last, context.GetItem(current));
					back.Add(last);
					last = current;
				}
				//set the first item on the position of the last
				context.SetItem(last,temp);
				back.Add(last);
			}
			//remove turn from the level
			context.SetItem(pos,null);
			return back.ToArray();
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="DiamondJam.Control.Gems.Turn"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="DiamondJam.Control.Gems.Turn"/>.</returns>
		public override string ToString(){
			return "Turn [group = "+group+" chain = "+chain+"]";
		}

		public override object Clone(){
			Turn t = (Turn)this.MemberwiseClone();
			return t;
		}
	}
}

