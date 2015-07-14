using DiamondJam.View;
using DiamondJam.View.Queue;

namespace DiamondJam.View.Queue
{
    /// <summary>
    /// Here you can register a callback.
    /// This will simply get called when the Visual Manager is in this step in the queue.
    /// It's usually used as last element to inform that the queue has ended.
    /// </summary>
	public class CallbackStep : IVisualStep
	{
		public delegate void StepsCallback();

		private StepsCallback callback;

        /// <summary>
        /// Creates a CallbackStep. 
        /// </summary>
        /// <param name="callback">this gets called when this step is reached in the queue</param>
		public CallbackStep (StepsCallback callback)
		{
			this.callback = callback;
		}

        /// <summary>
        /// Does nothing here.
        /// </summary>
        /// <param name="context"></param>
		public void SetContext(IVisualManager context){
		}

        /// <summary>
        /// Executes the callback.
        /// </summary>
        /// <returns></returns>
		public float Execute(){
			callback.Invoke();
			return 0;
		}
	}
}

