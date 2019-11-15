namespace Alpha.ProfilePicture.Models
{
    using System;

    public class ProfilePictureModel
    {
        public ProfilePictureModel()
        {
        }

        public ProfilePictureModel(int imageId, byte[] image)
        {
            ImageId = imageId;
            Image = $"data:image/jpeg;base64,{Convert.ToBase64String(image)}";
        }

        public int ImageId { get; private set; }

        public string Image { get; private set; }
    }

    public class ProfilePictureDTO
    {
        public ProfilePictureDTO()
        {
        }

        public ProfilePictureDTO(int imageId, byte[] image)
        {
            ImageId = imageId;
            Image = image;
        }

        public int ImageId { get; private set; }

        public byte[] Image { get; private set; }
    }
}
