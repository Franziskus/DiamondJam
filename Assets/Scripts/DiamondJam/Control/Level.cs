using UnityEngine;
using System.Collections.Generic;
using DiamondJam.Control.Gems;
using DiamondJam.Math;

namespace DiamondJam.Control
{
	/// <summary>
	/// The Level is a logical representation of the game state
	/// </summary>
	public class Level : IGemContext
	{
		/// <summary>
		/// The minimum number of color Gems to pop.
		/// </summary>
		public int minimumPopCount = 3;

		/// <summary>
		/// Bonus points.
		/// </summary>
		[HideInInspector]
		public int bonus = 0;

		/// <summary>
		/// Here are the items stored.
		/// </summary>
		private Gem[] items;

		/// <summary>
		/// The size of this level.
		/// </summary>
		private IntVector2 size;

		/// <summary>
		/// Set a two dimensional level.
		/// </summary>
		/// <param name="level">Level.</param>
		public void SetLevel(Gem[,] level){
			bonus = 0;
			size = new IntVector2(level.GetLength(1), level.GetLength(0));
			items = new Gem[size.x * size.y];
			int i = 0;
			for(int y = 0; y < size.y; y++){
				for(int x = 0; x < size.x; x++){
					items[i++] = level[x,y];
					if(level[x,y] != null)
						level[x,y].Init(this);
				}
			}
		}

		/// <summary>
		/// Set a one dimensional level
		/// </summary>
		/// <param name="level">Level</param>
		/// <param name="size">Size of the level</param>
		public void SetLevel(Gem[] level, IntVector2 size){
			bonus = 0;
			this.size = size;
			this.items = level;
			for(int i = 0; i < level.Length; i++){
				if(level[i] != null)
					level[i].Init(this);
			}
		}

		public Gem[] GetItems(){
			return items;
		}

		/// <summary>
		/// Gets the item in one dimension (index = y * wight + x). 
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="i">index</param>
		public Gem GetItem(int pos){
			return items[pos];
		}

		/// <summary>
		/// Sets the item on x and y.
		/// </summary>
		/// <param name="pos">Position.</param>
		/// <param name="g">A item or null.</param>
		public void SetItem(int pos, Gem g){
			items[pos] = g;
		}

		/// <summary>
		/// Gets the item on x and y.
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="pos">Position in x and y.</param>
		public Gem GetItem(IntVector2 pos){
			return GetItem(pos.ToOneDim(size.x));
		}

		public void SetItem(IntVector2 pos, Gem g){
			SetItem(pos.ToOneDim(size.x),g);
		}

		/// <summary>
		/// Gets the item on x and y.
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public Gem GetItem(int x, int y){
			return GetItem(new IntVector2(x,y));
		}

		/// <summary>
		/// Sets the item on x and y.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="g">The green component.</param>
		public void SetItem(int x, int y, Gem g){
			SetItem(new IntVector2(x,y),g);
		}

		/// <summary>
		/// Returns all positions of one group.
		/// </summary>
		/// <returns>The positions of a group.</returns>
		/// <param name="nr">Nr of the group.</param>
		public	IntVector2[] GetGroup (int nr)
		{
			List<IntVector2> back = new List<IntVector2>();
			for(int i = 0; i < items.Length; i++){
				if(items[i] != null && items[i].group == nr)
					back.Add(IntVector2.FromOneDim(i, size.x));
			}
			return back.ToArray();
		}

		/// <summary>
		/// Returns the minimum pop count for color gems.
		/// </summary>
		/// <returns>The minimum pop count.</returns>
		public int GetMinimumPopCount ()
		{
			return minimumPopCount;
		}

		/// <summary>
		/// Adds the bonus points.
		/// </summary>
		/// <param name="nr">amount of points</param>
		public	void AddBonus (int nr)
		{
			bonus += nr;
		}

		/// <summary>
		/// Gets the size of this level.
		/// </summary>
		/// <returns>The size.</returns>
		public	IntVector2 GetSize ()
		{
			return size;
		}

		/// <summary>
		/// Determines if this position is in the borders of this level.
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="i">The index.</param>
		public	bool IsInLevelBounce (IntVector2 i)
		{
			return i.x >= 0 && i.y >= 0 &&
				i.x < size.x && i.y < size.y;
		}

		/// <summary>
		/// Generates the groups.
		/// </summary>
		public void GenerateGroups(){
			for(int i = 0; i < items.Length; i++){
				if(items[i] != null)
					items[i].group = i;
			}

			for (int x = 0; x < size.x; x++){
				for (int y = 0; y < size.y; y++)
				{
					UnionCoords(x, y, x+1, y);
					UnionCoords(x, y, x, y+1);
				}
			}
			RootDown();
		}

		/// <summary>
		/// Use a Union find data structure to generate the groups
		/// http://en.wikipedia.org/wiki/Disjoint-set_data_structure
		/// </summary>
		/// <param name="currentX">Current x.</param>
		/// <param name="currentY">Current y.</param>
		/// <param name="neighbourX">Neighbour x.</param>
		/// <param name="neighbourY">Neighbour y.</param>
		private void UnionCoords(int currentX, int currentY, int neighbourX, int neighbourY)
		{
			if (neighbourY < size.y && neighbourX < size.x && 
			    GetItem(currentX,currentY) is ColorGem &&
			    ((ColorGem)GetItem(currentX,currentY)).ColorEquals(GetItem(neighbourX,neighbourY)))
				Union(currentY*size.x + currentX, neighbourY*size.x + neighbourX);
		}
		/// <summary>
		/// Union the specified current and neighbour.
		/// http://en.wikipedia.org/wiki/Disjoint-set_data_structure
		/// </summary>
		/// <param name="current">Current.</param>
		/// <param name="neighbour">Neighbour.</param>
		private void Union(int current, int neighbour)
		{
			// get the root component of a and b, and set the one's parent to the other
			while (GetItem(current).group != current)
				current = GetItem(current).group;
			while (GetItem(neighbour).group != neighbour)
				neighbour = GetItem(neighbour).group;
			GetItem(neighbour).group = current;
		}

		/// <summary>
		/// Bring the groups down. so all gems have the root as group. 
		/// </summary>
		private void RootDown(){
			for (int i = 0; i < items.Length; i++){
				if(items[i] != null && items[i].group != i){
					int current = items[i].group;
					while(items[current].group != current){
						current = items[current].group;
					}
					items[i].group = current;
				}
			}
		}

		/// <summary>
		/// Gets the length of this level. It is the size in one dimension (width*height).
		/// </summary>
		/// <returns>The length.</returns>
		public int GetLength(){
			return items.Length;
		}
	}
}
