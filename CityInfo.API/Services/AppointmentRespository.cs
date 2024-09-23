using CityInfo.API.DbContexts;
using CityInfo.API.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Numerics;
using System.Linq;

namespace CityInfo.API.Services
{
    public class AppointmentRespository: IAppointmentRespository
    {
        private readonly DoctorManagementContext _context;
        private readonly IEmailSender _emailSender;
        

        public static int PAGE_SIZE { get; set; } = 5;
        public AppointmentRespository(DoctorManagementContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }
        public List<AppointmentModel> GetAll(string search, string sortBy, int page = 1, int page_size = 5)
        {
            var allAppointment = _context.Appointments.AsQueryable();

            #region Filtering
            if (!string.IsNullOrWhiteSpace(search))
            {
                allAppointment = allAppointment.Where(pt => pt.AppointmentStatus.Contains(search));
            }
            #endregion

            #region Sorting
            allAppointment = allAppointment.OrderBy(pt => pt.AppointmentTime);

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy)
                {
                    case "date_asc": allAppointment = allAppointment.OrderBy(pt => pt.AppointmentTime); break;
                    case "date_desc": allAppointment = allAppointment.OrderByDescending(pt => pt.AppointmentTime); break;
                }
            }
            #endregion

            #region Paging
            if (!int.IsNegative(page_size))
            {
                PAGE_SIZE = page_size;
            }
            if (PAGE_SIZE != 0)
            {
                allAppointment = allAppointment.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            }
            #endregion
            
            
            var result = allAppointment.Select(ap => new AppointmentModel
            {
                AppointmentId = ap.AppointmentId,
                AppointmentTime = ap.AppointmentTime,
                AppointmentStatus = ap.AppointmentStatus,
                AppointmentComment = ap.AppointmentComment,
                AppointmentConfirmed = ap.AppointmentConfirmed,
                PatientStatus = ap.Patient.Status,
                PatientId = ap.PatientId,
                DoctorId = ap.DoctorId,
                Gender = ap.Patient.Gender,
                Avatar = ap.Patient.Avatar,
                FullName   = ap.Patient.FullName,
                Date = ap.Patient.Date,
                Diseases = ap.Patient.Diseases,
                Age = DateTime.Now.Year - ap.Patient.Date.Year -
                    (DateTime.Now < ap.Patient.Date.AddYears(DateTime.Now.Year - ap.Patient.Date.Year) ? 1 : 0)
            });


            return result.ToList();
        }

        public List<AppointmentModel> RecentPatient(int n)
        {
            var allAppointment = _context.Appointments.AsQueryable();

            allAppointment = allAppointment.Where(ap => ap.AppointmentConfirmed == "Finished");
            allAppointment = allAppointment.OrderByDescending(ap => ap.AppointmentTime);
            allAppointment = allAppointment.Take(n);


            var result = allAppointment.Select(ap => new AppointmentModel
            {
                AppointmentId = ap.AppointmentId,
                AppointmentTime = ap.AppointmentTime,
                AppointmentStatus = ap.AppointmentStatus,
                AppointmentComment = ap.AppointmentComment,
                AppointmentConfirmed = ap.AppointmentConfirmed,
                PatientStatus = ap.Patient.Status,
                PatientId = ap.PatientId,
                DoctorId = ap.DoctorId,
                Gender = ap.Patient.Gender,
                Avatar = ap.Patient.Avatar,
                FullName = ap.Patient.FullName,
                Date = ap.Patient.Date,
                Diseases = ap.Patient.Diseases,
                Age = DateTime.Now.Year - ap.Patient.Date.Year -
                    (DateTime.Now < ap.Patient.Date.AddYears(DateTime.Now.Year - ap.Patient.Date.Year) ? 1 : 0)
            });

            return result.ToList();
        }

        public AppointmentModel GetById(Guid id)
        {
            var appointment = _context.Appointments.Include(pt => pt.Patient).SingleOrDefault(ap => ap.AppointmentId == id);
            if (appointment != null)
            {
                return new AppointmentModel
                {
                    AppointmentId = appointment.AppointmentId,
                    AppointmentTime = appointment.AppointmentTime,
                    AppointmentStatus = appointment.AppointmentStatus,
                    AppointmentComment = appointment.AppointmentComment,
                    AppointmentConfirmed = appointment.AppointmentConfirmed,
                    PatientStatus = appointment.Patient.Status,
                    PatientId = appointment.PatientId,
                    DoctorId = appointment.DoctorId,
                    Gender = appointment.Patient.Gender,
                    Avatar = appointment.Patient.Avatar,
                    FullName = appointment.Patient.FullName,
                    Date = appointment.Patient.Date,
                    Diseases = appointment.Patient.Diseases,
                    Age = DateTime.Now.Year - appointment.Patient.Date.Year -
                    (DateTime.Now < appointment.Patient.Date.AddYears(DateTime.Now.Year - appointment.Patient.Date.Year) ? 1 : 0)
                };
            }

            return null;
        }

        public void Update(AppointmentVM appointment)
        {       

            var _appointment = _context.Appointments.SingleOrDefault(ap => ap.AppointmentId == appointment.AppointmentId);

            if (_appointment != null)
            {
                if (appointment.AppointmentStatus == "Confirmed")
                {
                    _appointment.AppointmentConfirmed = "Not Started";
                    var patient = _context.Patients.SingleOrDefault(pt => pt.PatientId == appointment.PatientId);
                    var doctor = _context.Doctors.SingleOrDefault(dt => dt.DoctorId == appointment.DoctorId);

                    if (patient.PatientId == appointment.PatientId)
                    {
                        EmailRequest mailrequest = new EmailRequest();
                        mailrequest.ToEmail = "formchuongtrinh@gmail.com";
                        mailrequest.Subject = "Xác nhận cuộc hẹn của bạn với Doctor Management";
                        mailrequest.Body = $@"
                                <div style='font-size: 120%; font-family: Arial; text-align: center; background-color: rgb(228, 238, 228); width: 800px; height: 1000px'>
                                    <p style='font-size: 150%; font-weight: bold; padding-top: 5%; color: rgb(0, 98, 0)'>Doctor Management Website</p>
                                    <div style='background-color: rgb(244, 247, 242); position: relative; width:94%; margin-left:3%; height: 88%'>
                                        <img style='height: 30%; margin-top: 2%; user-select: none; pointer-events: none;' src='https://img.icons8.com/sf-black/1028/40C057/checked.png' alt='checked'/>
                                        <p style='font-size: 280%; font-weight: bold; margin-top: 0;'>Xác nhận thành công</p>
                                        <h4>Kính gửi: {patient.FullName}</h4>  
                                        <h4>Điện thoại: {patient.PhoneNumber}</h4>  
                                        <p>Xác nhận thành công với thông tin chi tiết như sau:</p>    
                                        <hr style='width: 80%'>
                                        <h4>Thời gian: {appointment.AppointmentTime.ToString("HH:mm dd/MM/yyyy")}</h4>  
                                        <h4>Bác sĩ: {doctor.DoctorName}</h4>
                                        <h4>Khoa: {doctor.DoctorDepartment}</h4>
                                        <h4>Địa điểm: <strong>Thu Duc, Thanh Pho Ho Chi Minh</strong></h4>

                                        <p style='margin-top: 8%'>Nếu bạn cần thay đổi hoặc hủy lịch khám, vui lòng liên hệ với chúng tôi.</p>
                                        <p><img style='width: 2.5%; margin-right: 1%; margin-bottom: -0.4%;' src ='https://banner2.cleanpng.com/20180331/huq/avivwh2pr.webp'>0987654321 
                                           <img style='margin-bottom: -0.6%; width: 5.2%; margin-right: -0.8%; margin-left: 5%;' src='https://banner2.cleanpng.com/20180713/huy/aawa951nk.webp'>
                                           <a href='https://www.facebook.com/' style='text-decoration:none;' target='_blank'>FacebookLink</a> 
                                           <img src='https://banner2.cleanpng.com/20180715/hpo/aavibno7w.webp' style='margin-bottom: -0.8%; width: 3.2%; margin-right: 0%; margin-left: 5%'>
                                           <a href='https://www.instagram.com/' style='text-decoration:none' target='_blank'>InstagramLink</a>
                                        </p>
                                        <p>Cảm ơn bạn đã tin tưởng và sử dụng dịch vụ của chúng tôi.</p>
                                    </div>
                                </div>";
                        _emailSender.SendEmailAsync(mailrequest);
                    }
                }
                
                
                _appointment.AppointmentTime = appointment.AppointmentTime;
                _appointment.AppointmentStatus = appointment.AppointmentStatus;
                _appointment.AppointmentComment = appointment.AppointmentComment;
                _appointment.DoctorId = appointment.DoctorId;
                _context.SaveChanges();
            }
        }
        

        public async Task<AppointmentModelForAdd> Add(AppointmentModelForAdd appointment)
        {
            try
            {
                var _appointment = new Appointment
                {
                    AppointmentStatus = appointment.AppointmentStatus,
                    AppointmentComment = appointment.AppointmentComment,
                    AppointmentTime = appointment.AppointmentTime,
                    PatientId = appointment.PatientId,
                    DoctorId = appointment.DoctorId,
                };

                _context.Add(_appointment);
                await _context.SaveChangesAsync(); 

                return new AppointmentModelForAdd
                {
                    AppointmentTime = appointment.AppointmentTime,
                    AppointmentStatus = appointment.AppointmentStatus,
                    AppointmentComment = appointment.AppointmentComment,
                    PatientId = appointment.PatientId,
                    DoctorId = appointment.DoctorId,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding appointment: {ex.Message}");
                throw; 
            }
        }

        public void Delete(Guid id)
        {
            var appointment = _context.Appointments.SingleOrDefault(ap => ap.AppointmentId== id);
            if (appointment != null)
            {
                _context.Remove(appointment);
                _context.SaveChanges();
            }
        }
    }
}
