namespace Alpha.ProfilePicture.Models
{
    using System;

    public class ActorProfilePictureModel
    {
        public ActorProfilePictureModel()
        {
        }

        public ActorProfilePictureModel(int custId, byte[] image, int imageId)
        {
            CustId = custId;
            Image = image;
            ImageId = imageId;
        }

        public int CustId { get; private set; }

        public byte[] Image { get; private set; }

        public int ImageId { get; private set; }

        public string ImageBase64String => Image == null ? string.Empty : $"data:image/jpeg;base64,{Convert.ToBase64String(Image)}";
    }
}
