using CityInfo.API.DbContexts;
using CityInfo.API.Models;

namespace CityInfo.API.Services
{
    public interface IPatientRespository
    {
        List<PatientVM> GetAll(string search, string sortBy, int page, int page_size);
        PatientModel GetById(Guid id);
        PatientModel Add(PatientModel patient);
        void Update(PatientVM patient);
        void Delete(Guid id);
        Dictionary<string, int> PatientChart(int n);
    }
}
