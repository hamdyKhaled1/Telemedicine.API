using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Telemedicine.API.Common;
using Telemedicine.API.Infrastructure.Data;

namespace Telemedicine.API.Features.Account.Register
{
    
        public class RegisterPatientHandler
       : IRequestHandler<RegisterPatientCommand, Result<string>>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly TelemedicineDbContext _context;

            public RegisterPatientHandler(
                UserManager<ApplicationUser> userManager,
                TelemedicineDbContext context)
            {
                _userManager = userManager;
                _context = context;
            }

            public async Task<Result<string>> Handle(
                RegisterPatientCommand request,
                CancellationToken cancellationToken)
            {
                // 1. Email موجود؟
                var emailExists = await _userManager.FindByEmailAsync(request.Email);
                if (emailExists is not null)
                    return Result<string>.Failure("Email already registered.");

                // 2. إنشاء Patient في جدول Patients تلقائي
                var patient = new Patient
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Phone = request.Phone
                };

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync(cancellationToken);

                // 3. إنشاء الـ User مرتبط بالـ Patient
                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    RefId = patient.Id,  // ← ربط تلقائي
                    Role = "Patient"
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    // لو فشل امسح الـ Patient اللي اتعمل
                    _context.Patients.Remove(patient);
                    await _context.SaveChangesAsync(cancellationToken);

                    return Result<string>.Failure(
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                // 4. أضيف الـ Role
                await _userManager.AddToRoleAsync(user, "Patient");

                return Result<string>.Ok(
                    user.Email!,
                    $"Registration successful. Your Patient ID is {patient.Id}.");
            }
        }

        public class RegisterDoctorHandler
            : IRequestHandler<RegisterDoctorCommand, Result<string>>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly TelemedicineDbContext _context;

            public RegisterDoctorHandler(
                UserManager<ApplicationUser> userManager,
                TelemedicineDbContext context)
            {
                _userManager = userManager;
                _context = context;
            }

            public async Task<Result<string>> Handle(
                RegisterDoctorCommand request,
                CancellationToken cancellationToken)
            {
                // 1. Email موجود؟
                var emailExists = await _userManager.FindByEmailAsync(request.Email);
                if (emailExists is not null)
                    return Result<string>.Failure("Email already registered.");

                // 2. إنشاء Doctor في جدول Doctors تلقائي
                var doctor = new Doctor
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Specialty = request.Specialty,
                    IsActive = true
                };

                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync(cancellationToken);

                // 3. إنشاء الـ User مرتبط بالـ Doctor
                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    RefId = doctor.Id,  // ← ربط تلقائي
                    Role = "Doctor"
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    // لو فشل امسح الـ Doctor اللي اتعمل
                    _context.Doctors.Remove(doctor);
                    await _context.SaveChangesAsync(cancellationToken);

                    return Result<string>.Failure(
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                // 4. أضيف الـ Role
                await _userManager.AddToRoleAsync(user, "Doctor");

                return Result<string>.Ok(
                    user.Email!,
                    $"Registration successful. Your Doctor ID is {doctor.Id}.");
            }
        }
    }
