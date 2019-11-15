namespace Alpha.ProfilePicture.Params
{
    public class UpdateActorProfilePictureParam
    {
        public UpdateActorProfilePictureParam(
            int actorId,
            int? imageId,
            byte[] image,
            byte mode,
            bool isDefault,
            string ip)
        {
            ActorId = actorId;
            ImageId = imageId;
            Image = image;
            Mode = mode;
            IsDefault = isDefault;
            IP = ip;
        }
        public int ActorId { get; }

        public int? ImageId { get; }

        public byte[] Image { get; }

        public byte Mode { get; }

        public bool IsDefault { get; }

        public string IP { get; }
    }
}