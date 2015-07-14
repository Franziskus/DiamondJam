using UnityEngine;
using DiamondJam.Math;

namespace DiamondJam.View.Queue
{
    /// <summary>
    /// With this class you can simulate movements in the game.
    /// Usually used for falling Gems. All moves will start at the same time.
    /// </summary>
	public class MoveStep : IVisualStep
	{
		private IVisualManager context;
		private GemMove[] moves;
		private float time;

        /// <summary>
        /// Creates a MoveStep. 
        /// </summary>
        /// <param name="moves">Moves for this step</param>
        /// <param name="time">Maximum time to complete the step (a Gem drops from top to bottom of the field)</param>
		public MoveStep (GemMove[] moves, float time)
		{
			this.moves = moves;
			this.time = time;
		}

        /// <summary>
        /// The context call's this method and sets the current Visual Manager. This will happen before calling the Execute method.
        /// </summary>
        /// <param name="context">A reference to the current Visual Manager</param>
		public void SetContext(IVisualManager context){
			this.context = context;
		}

        /// <summary>
        /// Starts all animations and returns the time for the longes aniamtion.
        /// </summary>
        /// <returns></returns>
		public float Execute(){
            //Time of one Gem falling one field.
			float oneMove = OneMoveTime();
			float longestStep = 0;
			for(int i = 0; i < moves.Length; i++){
				GemMove currentMove = moves[i];
				int steps = ManhattanDistance(currentMove.to, currentMove.from);
				float stepTime = oneMove * steps;
				context.PlayGemAnimation(currentMove.from,currentMove.to, stepTime);
				if(stepTime > longestStep){
					longestStep = stepTime;
				}
			}
			return longestStep;
		}

        /// <summary>
        /// Manhattan Distance calculated faster then Euclidean distance. 
        /// Also we expect Gems moving on the grid and not move 
        /// straight through the playing field.
        /// </summary>
        /// <param name="a">start pos</param>
        /// <param name="b">end pos</param>
        /// <returns>distance between them (number of fields to move)</returns>
		private int ManhattanDistance(IntVector2 a, IntVector2 b){
			return Mathf.Abs(a.x-b.x)+ Mathf.Abs(a.y-b.y);
		}

        /// <summary>
        /// Calculates the for moving an item one field by assuming time determinants a fall from top to bottom.
        /// </summary>
        /// <returns></returns>
		private float OneMoveTime(){
			return time / context.GetLevelHeight();
		}
	}
}

