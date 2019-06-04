namespace Fanex.BetDetails.BetBuilder.Model
{
    using System.Threading;
    using Extensions;
    using Fanex.RemotingRef;

    public class BetBuilderDetailModel
    {
        private readonly IRemotingRefService remotingRefService;
        private string result { get; set; }

        public BetBuilderDetailModel() : this(null)
        {

        }

        public BetBuilderDetailModel(IRemotingRefService remotingRefService)
        {
            this.remotingRefService = remotingRefService ?? new RemotingRefService();
        }

        public int SelectionType { get; set; }

        public string Result
        {
            get
            {
                return result;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var toCamel = value.ToCamelCase().Trim();
                    var resourceText = Resources.Resource.ResourceManager.GetString(toCamel);

                    result = string.IsNullOrEmpty(resourceText) ? toCamel : resourceText;
                }
            }
        }

        public string SelectionName
        {
            get
            {
                var names = remotingRefService.GetBetBuilderSelectionNames(new int[] { SelectionType }, Thread.CurrentThread.CurrentCulture.Name);

                var name = names != null && names.Length == 1 ? names[0] : SelectionType.ToString();

                return name;
            }

        }


    }
}
