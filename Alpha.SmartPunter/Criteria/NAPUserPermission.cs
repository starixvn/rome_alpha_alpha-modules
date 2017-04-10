namespace Alpha.SmartPunter.Criteria
{
    public class NAPUserPermission
    {
        public NAPUserPermission(int customerId)
        {
            CustId = customerId;
        }

        public int CustId { get; }
    }
}