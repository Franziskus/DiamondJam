using DiamondJam.Control.Gems;

namespace DiamondJam.View.Queue
{
	/// <summary>
    /// Ths simply instruct to show a specific Gem setup immediately.
	/// </summary>
    public class LevelRefreshStep : IVisualStep
	{
		private IVisualManager context;
		private Gem[] items;
		private Gem[] newLine;

        /// <summary>
        /// Creates a LevelRefreshStep.
        /// </summary>
        /// <param name="items">Items for the setup. It will save it's own copy</param>
        /// <param name="newLine">Items to show at the new line</param>
		public LevelRefreshStep (Gem[] items, Gem[] newLine)
		{
			this.items = new Gem[items.Length];
			for(int i = 0; i < items.Length; i++){
				if(items[i] != null)
					this.items[i] = (Gem)items[i].Clone();
			}
			this.newLine = newLine;
		}

        /// <summary>
        /// The context call's this method and sets the current Visual Manager. This will happen before calling the Execute method.
        /// </summary>
        /// <param name="context">A reference to the current Visual Manager</param>
		public void SetContext(IVisualManager context){
			this.context = context;
		}

        /// <summary>
        /// Instruct VisualContext to show Gem setup.
        /// </summary>
        /// <returns></returns>
		public float Execute(){
			for(int i = 0; i < items.Length; i++){
				context.SetGem(i,items[i]);
			}
			context.SetExtraLine(newLine);
			return 0;
		}
	}
}

