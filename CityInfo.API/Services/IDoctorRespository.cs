using CityInfo.API.Models;

namespace CityInfo.API.Services
{
    public interface IDoctorRespository
    {
        List<DoctorModel> GetAll(string search, int page = 1, int page_size = 5);
        List<DoctorModelName> GetNameByDeparterment(string doctor);
        DoctorModel Add(DoctorModel model);
        void Delete(Guid id);
        void Update(DoctorVM doctor);
    }
}
