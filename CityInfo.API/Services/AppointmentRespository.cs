using CityInfo.API.DbContexts;
using CityInfo.API.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Numerics;

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
                allAppointment = allAppointment.Where(pt => pt.AppoitmentStatus.Contains(search));
            }
            #endregion

            #region Sorting
            allAppointment = allAppointment.OrderBy(pt => pt.AppoitmentTime);

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy)
                {
                    case "date_asc": allAppointment = allAppointment.OrderBy(pt => pt.AppoitmentTime); break;
                    case "date_desc": allAppointment = allAppointment.OrderByDescending(pt => pt.AppoitmentTime); break;
                }
            }
            #endregion

            #region Paging
            if (!int.IsNegative(page_size))
            {
                PAGE_SIZE = page_size;
            }
            allAppointment = allAppointment.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            #endregion
            var result = allAppointment.Select(ap => new AppointmentModel
            {
                AppoitmentTime = ap.AppoitmentTime,
                AppoitmentStatus = ap.AppoitmentStatus,
                PatientId = ap.PatientId,
                Gender = ap.Patient.Gender,
                Avatar = ap.Patient.Avatar,
                FullName    = ap.Patient.FullName,
                Date = ap.Patient.Date,
            });


            return result.ToList();
        }

        public AppointmentModel GetById(Guid id)
        {
            var appointment = _context.Appointments.Include(pt => pt.Patient).SingleOrDefault(ap => ap.AppoitmentId == id);
            if (appointment != null)
            {
                return new AppointmentModel
                {
                    AppoitmentTime = appointment.AppoitmentTime,
                    AppoitmentStatus = appointment.AppoitmentStatus,
                    PatientId = appointment.PatientId,
                    Gender = appointment.Patient.Gender,
                    Avatar = appointment.Patient.Avatar,
                    FullName = appointment.Patient.FullName,
                    Date = appointment.Patient.Date
                };
            }
            return null;
        }

        public void Update(AppointmentVM appointment)
        {       

            var _appointment = _context.Appointments.SingleOrDefault(ap => ap.AppoitmentId == appointment.AppoitmentId);
            if (_appointment != null)
            {
                _appointment.AppoitmentTime = appointment.AppoitmentTime;
                _appointment.AppoitmentStatus = appointment.AppoitmentStatus;

                _context.SaveChanges();
            }


        }

        public async Task<AppointmentModelForAdd> Add(AppointmentModelForAdd appointment)
        {
            try
            {
                var _appointment = new Appointment
                {
                    AppoitmentStatus = appointment.AppoitmentStatus,
                    AppoitmentTime = appointment.AppoitmentTime,
                    PatientId = appointment.PatientId,
                };


                if (appointment.AppoitmentStatus == "Confirmed")
                {
                    var patient = _context.Patients.SingleOrDefault(pt => pt.PatientId == appointment.PatientId);

                    if (patient.PatientId == appointment.PatientId)
                    {
                        EmailRequest mailrequest = new EmailRequest();
                        mailrequest.ToEmail = "formchuongtrinh@gmail.com";
                        mailrequest.Subject = "Xác nhận cuộc hẹn của bạn với Doctor Management";
                        mailrequest.Body =
                            $"<p>Kính gửi <strong>{patient.FullName}</strong>,</p>\r\n    " +
                            "<p>Cuộc hẹn của bạn đã được xác nhận thành công.</p>\r\n\r\n    " +
                            "<h3>Thông tin chi tiết cuộc hẹn:</h3>\r\n    " +
                            $"<ul>\r\n        " +
                            $"<li>Thời gian: <strong>{appointment.AppoitmentTime.ToString("HH:mm dd/MM/yyyy")}</strong></li>\r\n        " +
                            $"<li>Bác sĩ: <strong>Toan Nguyen</strong></li>\r\n        " +
                            $"<li>Địa điểm: <strong>Thu Duc, Thanh Pho Ho Chi Minh</strong></li>\r\n    " +
                            "</ul>\r\n\r\n    " +
                            "<p>Cảm ơn bạn đã tin tưởng và sử dụng dịch vụ của chúng tôi.</p>\r\n    " +
                            "<p>Trân trọng,</p>\r\n    " +
                            "<p><strong>Doctor Management Team</strong></p>";
                        await _emailSender.SendEmailAsync(mailrequest);
                    }
                    
                }
                _context.Add(_appointment);
                await _context.SaveChangesAsync(); 

                return new AppointmentModelForAdd
                {
                    AppoitmentTime = appointment.AppoitmentTime,
                    AppoitmentStatus = appointment.AppoitmentStatus,
                    PatientId = appointment.PatientId,
                };
            }
            catch (Exception ex)
            {
                // Ghi log lỗi và ném ngoại lệ
                Console.WriteLine($"Error adding appointment: {ex.Message}");
                throw; // Ném lại ngoại lệ để hệ thống xử lý tiếp
            }
        }

        public void Delete(Guid id)
        {
            var appointment = _context.Appointments.SingleOrDefault(ap => ap.AppoitmentId== id);
            if (appointment != null)
            {
                _context.Remove(appointment);
                _context.SaveChanges();
            }
        }
    }
}
