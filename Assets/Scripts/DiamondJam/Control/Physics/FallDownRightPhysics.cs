using System;
using UnityEngine;
using DiamondJam.Control;
using DiamondJam.Control.Gems;
using DiamondJam.Control.Manager;
using DiamondJam.Math;
using System.Collections.Generic;

namespace DiamondJam.Control.Physics
{
	/// <summary>
	/// FallDownRightPhysics.
	/// All gems fall down. 
	/// If a complite column disappear the left neighbors a shifted 
	/// to fill the space
	/// </summary>
	public class FallDownRightPhysics: MonoBehaviour, ILevelPhysics
	{
		public IGemContext level;
	
		public int PhysicsSteps(){
			return 2;
		}

		/// <summary>
		/// Register this Physics to ILevelManager.
		/// </summary>
		public void Start(){
			ILevelManager levelManger = (ILevelManager)GetComponent(typeof (ILevelManager));
			level = levelManger.GetGemContext();
			levelManger.SetLevelPhysics(this);
		}

		/// <summary>
		/// Calculate the phyics. Is the deepes y pos in the level 
		/// that needs to update. The length of this array is the same 
		/// as the level wigth. If the pos is -1 then this column dos NOT
		/// need an update.
		/// </summary>
		/// <param name="updateYpos">Update ypos array with deepes update pos.</param>
		public GemMove[] DoPhysics(int nr, int[] updateYpos){
			if(nr == 0){
				return FallingDown(updateYpos);
			}else if(nr == 1){
				return FallingRight();
			}else{
				throw new System.ArgumentException("Number needs to be between 0 and "+PhysicsSteps()+".");
			}
		}

		/// <summary>
		/// Bring all gems down. 
		/// </summary>
		/// <param name="updateHorizontalArray">Update horizontal array.</param>
		private GemMove[] FallingDown(int[] updateHorizontalArray){
			int width = updateHorizontalArray.Length;
			List<IntVector2> fallingBlocksStart = new List<IntVector2>();
			List<IntVector2> fallingBlocksSEnd = new List<IntVector2>();

			for(int x = 0; x < width; x++){
				bool firstRun = true;
				if(updateHorizontalArray[x] !=-1){
					// this column needs an update
					int currentY = updateHorizontalArray[x];
					bool somethingToMove = true;
					while(currentY > -1 && somethingToMove){
						somethingToMove = false;
						//if there is a item now we can start one item up
						if(level.GetItem(new IntVector2(x,currentY)) != null){
							currentY--;
						}
						// if there is space bring them all one down
						for(int y = currentY-1; y >= 0; y--){
							IntVector2 currentXY = new IntVector2(x,y+1);
							IntVector2 currentXYTop = new IntVector2(x,y);
							if(level.GetItem(currentXY) == null){
								level.SetItem(currentXY, level.GetItem(currentXYTop));
								if(level.GetItem(currentXYTop) != null){
									if(firstRun){
										fallingBlocksStart.Add(currentXYTop);
										fallingBlocksSEnd.Add(currentXY);
									}else{
										int pos = fallingBlocksSEnd.IndexOf(currentXYTop);
										if(pos != -1){
											fallingBlocksSEnd[pos] = currentXY;
										}else{
											string s = "try to move a not found object?\n";
											s += "Look for "+currentXYTop+"\n";
											for(int i = 0; i < fallingBlocksSEnd.Count; i++)
												s+= fallingBlocksSEnd[i].ToString() + ", ";
											Debug.LogWarning(s);
										}
								}
								}
								level.SetItem(currentXYTop, null);
							}
						}
						//if there are items over the currentY pos we need to loop more. 
						for(int y = currentY-1; y >= 0 && !somethingToMove; y--){
							if(level.GetItem(new IntVector2(x,y+1)) != null)
								somethingToMove = true;
						}
						firstRun = false;
					}
				}
			}
			GemMove[] fallingMoves = new GemMove[fallingBlocksStart.Count];
			for(int i = 0; i < fallingMoves.Length; i++){
				fallingMoves[i].from = fallingBlocksStart[i];
				fallingMoves[i].to = fallingBlocksSEnd[i];
			}
			return fallingMoves;
		}

		/// <summary>
		/// Falling to the right.
		/// </summary>
		GemMove[] FallingRight(){
			IntVector2 size = level.GetSize();

			List<int> startXGround = new List<int>();
			List<int> endXGround = new List<int>();
			List<GemMove> moves = new List<GemMove>();

            //find large holes
			for(int x = 0; x < size.x; x++){
				//find the first empty space
				if(level.GetItem(new IntVector2(x,size.y-1)) == null){
					startXGround.Add(x);
					//if the next line is also empty we will move over it
					for(int tx = x; tx < size.x; tx++){
						if(level.GetItem(new IntVector2(tx,size.y-1)) == null){
							x++;
						}else{
							endXGround.Add(x);
							break;
						}
					}
				}			
			}
			if(endXGround.Count < startXGround.Count){
				endXGround.Add(size.x);
			}
			//string s = "list";
			//for(int i = 0; i < startXGround.Count; i++){
			//	s += "("+(startXGround[i]-1) + "," +(endXGround[i]-1)+")";
			//}
			//Debug.Log(s);

			int targetX = size.x-1;
			int distance = 1;
			while(targetX-distance >= 0 && targetX >= 0){
                //if we know we have a hole we add this to move distance 
				if(endXGround.Contains(targetX)){
					int index = endXGround.IndexOf(targetX);
					distance += (targetX - startXGround[index]) - 1;
				}
				if(level.GetItem(new IntVector2(targetX,size.y-1)) == null){
                    //the target pos could also be empty => then we have to move the item even more.
					while(targetX-distance >= 0 && level.GetItem(new IntVector2(targetX-distance,size.y-1)) == null){
						distance += 1;
					}
					if(targetX-distance >= 0){
						for(int y = 0; y < size.y; y++){
							IntVector2 from = new IntVector2(targetX-distance,y);
							IntVector2 to = new IntVector2(targetX,y);
							Gem currentGem = level.GetItem(from);
							if(currentGem != null){
								moves.Add(new GemMove(from, to));
								level.SetItem(to, currentGem);
								level.SetItem(from, null);
							}
						}
					}
				}

				targetX--;
			}
			return moves.ToArray();
		}
	}
}

