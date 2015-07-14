using System;
namespace DiamondJam.Control.Gems
{
	/// <summary>
	/// IcedColorGem is a <see cref="DiamondJam.Control.Gems.ColorGem"/> but it has a ice barrier too.
	/// So you need to pop it multiple times.
	/// </summary>
	public class IcedColorGem : ColorGem
	{
		/// <summary>
		/// How often needs this gem get hit to break the ice.
		/// </summary>
		private int _hitpoints = 1;

		/// <summary>
		/// Left hit points.
		/// </summary>
		/// <value>The hit points.</value>
		public int hitpoints{
			get{
				return _hitpoints;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DiamondJam.Control.Gems.IcedColorGem"/> class.
		/// </summary>
		/// <param name="c">color of this gem</param>
		public IcedColorGem(GemColor c): base(c){
		}

		/// <summary>
		/// Pop this Gem. You have to hit IceGems multiple times to destroy them.
		/// </summary>
		/// <returns>true = Item is destroyed. Otherwise false</returns>
		public override bool Pop(){
			//You have to crack the ice.
			_hitpoints--;
			//if the barrier is away the block is popped
			if(_hitpoints < 0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="DiamondJam.Control.Gems.IcedColorGem"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="DiamondJam.Control.Gems.IcedColorGem"/>.</returns>
		public override string ToString(){
			return "IcedColorGem [group = "+group+" chain = "+chain+", color = "+color+", hitpoints = "+_hitpoints+" hash = "+base.GetHashCode()+"]";
		}

		public override object Clone(){
			IcedColorGem i = (IcedColorGem)this.MemberwiseClone();
			return i;
		}
	}
}

