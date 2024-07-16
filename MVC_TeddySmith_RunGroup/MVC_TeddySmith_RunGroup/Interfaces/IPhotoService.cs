using CloudinaryDotNet.Actions;

namespace MVC_TeddySmith_RunGroup.Interfaces
{
    public interface IPhotoService
    {
        //to use ImageUploadResult upload nuget CloudinaryDotNet
        //IFormFile: when we upload file it has all of the properties 
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
