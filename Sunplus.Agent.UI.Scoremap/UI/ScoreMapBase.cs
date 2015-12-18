namespace Sunplus.Agent.UI.ScoreMap.UI
{
    using System.Data;

    public abstract class ScoreMapBase : ColumnScoreMap_Template, IScoreMapItem
    {
        public abstract string Render();
        public abstract void Assign(DataRow row);
        
        #region IComponent Members
        public abstract void HideLinkedResources();
        #endregion
    }
}