namespace Alpha.ProfilePicture.Services
{
    using Alpha.AccountInfo.Features.Shared.Models;
    using Alpha.ProfilePicture.Models;
    using Alpha.ProfilePicture.Params;
    using Alpha.ProfilePicture.Params.SPs.Commands;
    using Alpha.ProfilePicture.Params.SPs.Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ProfilePictureServices
    {
        ProfilePictureResult UpdateManualProfilePictureCommand(byte[] image, string filename);

        Task<ActorProfilePictureModel> GetActorProfilePictureAsync(int custId);

        ActorProfilePictureModel GetActorProfilePicture(int custId);

        Task<IEnumerable<ProfilePictureModel>> GetProfilePictureLibraryAsync();

        Task<ProfilePictureResult> UpdateActorProfilePictureAsync(UpdateActorProfilePictureParam param);

        ProfilePictureResult UpdateActorProfilePicture(UpdateActorProfilePictureParam param);
    }

    public class ProfilePictureServicesImpl : ProfilePictureServices
    {
        private const string ProfilePictureLibaryKey = "Alpha.AccountInfo.Site.Features.ProfilePicture.Library";
        private readonly IDynamicRepository dynamicRepository;

        public ProfilePictureServicesImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public ProfilePictureResult UpdateManualProfilePictureCommand(byte[] image, string filename)
        {
            try
            {
                var command = new UpdateManualProfilePictureCommand(image);

                dynamicRepository.Execute(command);

                return ProfilePictureResult.CreateSuccessResult();
            }
            catch (Exception ex)
            {
                return ProfilePictureResult.CreateErrorResult(100, filename);
            }
        }

        public async Task<ActorProfilePictureModel> GetActorProfilePictureAsync(int custId)
        {
            try
            {
                return await dynamicRepository.GetAsync<ActorProfilePictureModel>(new SelectActorProfilePictureCriteria(custId)).ConfigureAwait(false);
            }
            catch
            {
                return new ActorProfilePictureModel();
            }
        }

        public ActorProfilePictureModel GetActorProfilePicture(int custId)
        {
            try
            {
                return dynamicRepository.Get<ActorProfilePictureModel>(new SelectActorProfilePictureCriteria(custId));
            }
            catch
            {
                return new ActorProfilePictureModel();
            }
        }

        public async Task<IEnumerable<ProfilePictureModel>> GetProfilePictureLibraryAsync()
            => await await ConfiguredCacheService.GetOrSetAsync(
                    ProfilePictureLibaryKey,
                    async () => await FectchProfilePictureLibraryAsync().ConfigureAwait(false)).ConfigureAwait(false);

        internal async Task<IEnumerable<ProfilePictureModel>> FectchProfilePictureLibraryAsync()
        {
            var binaryProfilePictures = await dynamicRepository.FetchAsync<ProfilePictureDTO>(new SelectProfilePictureLibraryCriteria()).ConfigureAwait(false);
            return binaryProfilePictures.Select(profilePictureDto => new ProfilePictureModel(profilePictureDto.ImageId, profilePictureDto.Image));
        }

        public async Task<ProfilePictureResult> UpdateActorProfilePictureAsync(UpdateActorProfilePictureParam param)
        {
            var command = new UpdateActorProfilePictureCommand(
                    param.ActorId,
                    param.ImageId,
                    param.Image,
                    param.IsDefault,
                    param.Mode,
                    param.IP);

            await dynamicRepository.ExecuteAsync(command).ConfigureAwait(false);

            return command.ErrCode.Equals(0) ?
                ProfilePictureResult.CreateSuccessResult() :
                ProfilePictureResult.CreateErrorResult(command.ErrCode, command.ErrMsg);
        }

        public ProfilePictureResult UpdateActorProfilePicture(UpdateActorProfilePictureParam param)
        {
            var command = new UpdateActorProfilePictureCommand(
                    param.ActorId,
                    param.ImageId,
                    param.Image,
                    param.IsDefault,
                    param.Mode,
                    param.IP);

            dynamicRepository.Execute(command);

            return command.ErrCode.Equals(0) ?
                ProfilePictureResult.CreateSuccessResult() :
                ProfilePictureResult.CreateErrorResult(command.ErrCode, command.ErrMsg);
        }
    }
}