using System;
using DiamondJam.Math;

namespace DiamondJam.Control.Gems
{
	/// <summary>
	/// Firework pops all Gems in a cross shape around his position.
	/// Can be part of a chain.
	/// </summary>
	public class Firework : Gem
	{
		public IGemContext context;

		/// <summary>
		/// Initializes a new instance of the <see cref="DiamondJam.Control.Gems.Firework"/> class.
		/// </summary>
		public Firework(){
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
		/// Returns an array of items to pop in a cross shape.
		/// </summary>
		/// <param name="pos">Position in the level</param>
		/// <param name="updateOnly">UpdateOnly is always false</param>
		/// <returns>Array of item position to pop</returns>
		public override IntVector2[] Activate(IntVector2 pos, out bool updateOnly){
			updateOnly = false;
			IntVector2 leveSize = context.GetSize();
			IntVector2[] back = new IntVector2[leveSize.x + leveSize.y -1];

			back[0] = pos;
			int counter = 1;

			//Insert horizontal, without our own pos.
			for(int y = 0; y < pos.y; y++){
				back[counter++] = new IntVector2(pos.x, y);
			}
			for(int y = pos.y+1; y < leveSize.y; y++){
				back[counter++] = new IntVector2(pos.x, y);
			}


			//Insert vertical, without our own pos.
			for(int x = 0; x < pos.x; x++){
				back[counter++] = new IntVector2(x, pos.y);
			}
			for(int x = pos.x+1; x < leveSize.x; x++){
				back[counter++] = new IntVector2(x, pos.y);
			}

			return back;
		}

		public override string ToString(){
			return "Firework [group = "+group+" chain = "+chain+"]";
		}

		public override object Clone(){
			Firework f = (Firework)this.MemberwiseClone();
			return f;
		}
	}
}

