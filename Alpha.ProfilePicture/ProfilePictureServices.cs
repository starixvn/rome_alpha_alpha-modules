namespace Alpha.ProfilePicture.Services
{
    using Alpha.AccountInfo.Features.Shared.Models;
    using Alpha.ProfilePicture.AppResources;
    using Alpha.ProfilePicture.Models;
    using Alpha.ProfilePicture.Params;
    using Alpha.ProfilePicture.Params.SPs.Commands;
    using Alpha.ProfilePicture.Params.SPs.Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fanex.Logging.Extensions.Web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ProfilePictureServices
    {
        ProfilePictureResult UpdateManualProfilePictureCommand(byte[] image, string filename);

        Task<ActorProfilePictureModel> GetActorProfilePictureAsync(int custId);

        ActorProfilePictureModel GetActorProfilePicture(int custId);

        Task<IEnumerable<ProfilePictureModel>> FectchProfilePictureLibraryAsync();

        Task<ProfilePictureResult> UpdateActorProfilePictureAsync(UpdateActorProfilePictureParam param);

        ProfilePictureResult UpdateActorProfilePicture(UpdateActorProfilePictureParam param);
    }

    public class ProfilePictureServicesImpl : ProfilePictureServices
    {
        private const int ErrorCodeDefault = 100;
        private readonly IDynamicRepository dynamicRepository;
        private readonly ProfilePictureResult ErrorDefaultResult =
            ProfilePictureResult.CreateErrorResult(ErrorCodeDefault, ProfilePictureResources.ProfilePictureUpdatedFailed);
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
                ExceptionHandler.Handle(ex);
                return ProfilePictureResult.CreateErrorResult(ErrorCodeDefault, filename);
            }
        }

        public async Task<ActorProfilePictureModel> GetActorProfilePictureAsync(int custId)
        {
            try
            {
                var model = await dynamicRepository.GetAsync<ActorProfilePictureModel>(new SelectActorProfilePictureCriteria(custId)).ConfigureAwait(false);
                return model ?? new ActorProfilePictureModel();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
                return new ActorProfilePictureModel();
            }
        }

        public ActorProfilePictureModel GetActorProfilePicture(int custId)
        {
            try
            {
                var model = dynamicRepository.Get<ActorProfilePictureModel>(new SelectActorProfilePictureCriteria(custId));
                return model ?? new ActorProfilePictureModel();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
                return new ActorProfilePictureModel();
            }
        }

        public async Task<IEnumerable<ProfilePictureModel>> FectchProfilePictureLibraryAsync()
        {
            try
            {
                var binaryProfilePictures = await dynamicRepository.FetchAsync<ProfilePictureDTO>(new SelectProfilePictureLibraryCriteria()).ConfigureAwait(false);
                return binaryProfilePictures == null ? Enumerable.Empty<ProfilePictureModel>() :
                    binaryProfilePictures.Select(profilePictureDto => new ProfilePictureModel(profilePictureDto.ImageId, profilePictureDto.Image));
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
                return Enumerable.Empty<ProfilePictureModel>();
            }
        }

        public async Task<ProfilePictureResult> UpdateActorProfilePictureAsync(UpdateActorProfilePictureParam param)
        {
            try
            {
                var command = BuildUpdateActorProfilePictureCommand(param);
                await dynamicRepository.ExecuteAsync(command).ConfigureAwait(false);
                return BuildUpdateActorProfilePictureResult(command);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
                return ErrorDefaultResult;
            }
        }

        public ProfilePictureResult UpdateActorProfilePicture(UpdateActorProfilePictureParam param)
        {
            try
            {
                var command = BuildUpdateActorProfilePictureCommand(param);
                dynamicRepository.Execute(command);
                return BuildUpdateActorProfilePictureResult(command);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
                return ErrorDefaultResult;
            }
        }

        private UpdateActorProfilePictureCommand BuildUpdateActorProfilePictureCommand(UpdateActorProfilePictureParam param)
            => new UpdateActorProfilePictureCommand(
                        param.ActorId,
                        param.ImageId,
                        param.Image,
                        param.IsDefault,
                        param.Mode,
                        param.IP);

        private ProfilePictureResult BuildUpdateActorProfilePictureResult(UpdateActorProfilePictureCommand command)
            => command.ErrCode.Equals(0) ?
                    ProfilePictureResult.CreateSuccessResult() :
                    ProfilePictureResult.CreateErrorResult(command.ErrCode, command.ErrMsg);
    }
}