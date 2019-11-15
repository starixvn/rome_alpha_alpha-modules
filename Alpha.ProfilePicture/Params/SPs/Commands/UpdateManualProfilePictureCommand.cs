namespace Alpha.ProfilePicture.Params.SPs.Commands
{
    using Fanex.Data.Repository;

    public class UpdateManualProfilePictureCommand : NonQueryCommand
    {
        public UpdateManualProfilePictureCommand(byte[] image, bool isMBC = true)
        {
            Image = image;
            IsMBC = isMBC;
        }

        public byte[] Image { get; }

        public bool IsMBC { get; }

        public override string GetSettingKey() => "DBA_InsertProfilePictureLibrary";

        public override bool IsValid() => true;
    }
}