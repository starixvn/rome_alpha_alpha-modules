namespace Alpha.Web.Helpers.Impl
{
    using Alpha.Web.Exceptions;

    public class PartialUMServiceImpl : PartialUMService
    {
        public void CheckPartialUM(string settingKey)
        {
            var functionModel = PartialUMHelper.GetUMFunction(settingKey);

            if (functionModel != null)
            {
                throw new PartialUMException(functionModel.UMFrom, functionModel.UMTo);
            }
        }
    }
}
