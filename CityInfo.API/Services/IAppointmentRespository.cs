using CityInfo.API.Models;

namespace CityInfo.API.Services
{
    public interface IAppointmentRespository 
    {
        List<AppointmentModel> GetAll(string search, string sortBy, int page, int page_size);
        AppointmentModel GetById(Guid id);
        Task<AppointmentModelForAdd> Add(AppointmentModelForAdd patient);
        void Update(AppointmentVM patient);
        void Delete(Guid id);
    }
}
