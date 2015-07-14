using System;
using DiamondJam.View;

namespace DiamondJam.View.Queue
{
    /// <summary>
    /// This Interface declares a class as Visual step.
    /// VisualManager has the StepsQueue(IVisualStep[]) Method to interrate through all IVisualSteps in array in order.
    /// Here are some example for implementation:
    /// - Its used to show specific level setups (LevelRefreshStep). 
    /// - Or a Gems movement for example falling (MoveStep).
    /// - There is also a CallbackSep to get informed about a specific progress in the queue.
    /// </summary>
	public interface IVisualStep
    {
        /// <summary>
        /// The context call's this method and sets the current Visual Manager. This will happen before calling the Execute method.
        /// </summary>
        /// <param name="context">A reference to the current Visual Manager</param>
		void SetContext(IVisualManager context);

        /// Implement all changes in Visual Manager for this step here.
        /// return the time your changes will need.
        /// The Queue will wait for the returned time after this Method is called.
        /// before executing the next VisualStep in queue. 
        /// Is is usually interesting for animation steps.
        /// </summary>
        /// <returns>time to wait after this method is called</returns>
		float Execute();
	}
}

